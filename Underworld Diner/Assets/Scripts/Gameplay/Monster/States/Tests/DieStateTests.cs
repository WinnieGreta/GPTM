using System.Reflection;
using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class DieStateTests : ZenjectUnitTestFixture
    {
        private IStatisticsManager _statisticsManager;
        private IAiComponent _aiComponent;
        private INavigationComponent _navigationComponent;
        private IAnimatorComponent _animatorComponent;
        private IScoringComponent _scoringComponent;
        private IDespawnable _despawnable;
        private IResourceManager _resourceManager;
        private MonsterLootSettings _lootSettings;
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _statisticsManager = Substitute.For<IStatisticsManager>();
            _aiComponent = Substitute.For<IAiComponent>();
            _navigationComponent = Substitute.For<INavigationComponent>();
            _animatorComponent = Substitute.For<IAnimatorComponent>();
            _scoringComponent = Substitute.For<IScoringComponent>();
            _despawnable = Substitute.For<IDespawnable>();
            _resourceManager = Substitute.For<IResourceManager>();

            Container.BindInstance(_aiComponent);
            Container.BindInstance(_statisticsManager);
            Container.BindInstance(_navigationComponent);
            Container.BindInstance(_animatorComponent);
            Container.BindInstance(_scoringComponent);
            Container.BindInstance(_despawnable);
            Container.BindInstance(_resourceManager);

            Container.BindInstance(MonsterType.Skeleton);
            Container.Bind<DieState>().AsTransient();

            _lootSettings = new MonsterLootSettings();
            typeof(MonsterLootSettings).GetProperty("RedDrop",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(_lootSettings, 10);
            typeof(MonsterLootSettings).GetProperty("GreenDrop",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(_lootSettings, 20);
            typeof(MonsterLootSettings).GetProperty("BlueDrop",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(_lootSettings, 30);

            Container.Rebind<MonsterLootSettings>()
                .FromInstance(_lootSettings)
                .AsCached();
        }

        [Test]
        public void DieState_Enter()
        {
            var state = Container.Resolve<DieState>();
            state.Enter();
            _statisticsManager.Received(1).IncrementStatistics("MonsterKillSkeleton");
            _navigationComponent.Received(1).StopOnDeath();
            _aiComponent.Received(1).FreeChairByMonster();
            _animatorComponent.Received(1).DeathAnimation();
            _resourceManager.Received(1).GainResources(_lootSettings.RedDrop, _lootSettings.GreenDrop, _lootSettings.BlueDrop);
        }

        [Test]
        public void DieState_OnTick()
        {
            var dieState1 = Container.Resolve<DieState>();
            dieState1.Enter();
            
            var corpseTime = typeof(DieState).GetField("_corpseTime",
                BindingFlags.Instance | BindingFlags.NonPublic);
            corpseTime.SetValue(dieState1, 1f);
            
            var timer1 = typeof(DieState).GetField("_timerTime",
                BindingFlags.Instance | BindingFlags.NonPublic);
            timer1.SetValue(dieState1, 2f);
            
            dieState1.OnTick();
            _aiComponent.Received(1).ChangeState(MonsterState.Null);
            _despawnable.Received(1).Despawn();
            
            _aiComponent.ClearReceivedCalls();
            _despawnable.ClearReceivedCalls();

            var dieState2 = Container.Resolve<DieState>();
            dieState2.Enter();
            
            corpseTime.SetValue(dieState2, 1f);
            timer1.SetValue(dieState2, 0.5f);
            
            dieState2.OnTick();
            _aiComponent.DidNotReceive().ChangeState(MonsterState.Null);
            _despawnable.DidNotReceive().Despawn();
        }
        
    }
}