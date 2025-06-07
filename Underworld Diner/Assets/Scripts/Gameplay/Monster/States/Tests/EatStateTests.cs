using System.Reflection;
using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class EatStateTests : ZenjectUnitTestFixture
    { 
        private IAiComponent _aiComponent;
        private IAnimatorComponent _animationComponent;
        private IStatisticsManager _statisticsManager;
        private IScoringComponent _scoringComponent;
        private MonsterStatusComponent _statusComponent;
        private MonsterServiceSettings _serviceSettings;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _aiComponent = Substitute.For<IAiComponent>();
            _animationComponent = Substitute.For<IAnimatorComponent>();
            _statisticsManager = Substitute.For<IStatisticsManager>();
            _scoringComponent = Substitute.For<IScoringComponent>();
            
            var chair = Substitute.For<IChair>();
            chair.IsTaken.Returns(true);
            _statusComponent = new MonsterStatusComponent { MyChair = chair };
            
            Container.BindInstance(_aiComponent);
            Container.BindInstance(_animationComponent);
            Container.BindInstance(_statisticsManager);
            Container.BindInstance(_scoringComponent);
            Container.BindInstance(_statusComponent);
            Container.BindInstance(MonsterType.Skeleton);
            Container.Bind<EatState>().AsTransient();
            
            _serviceSettings = new MonsterServiceSettings();
            typeof(MonsterServiceSettings).GetProperty("EatingDowntime",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(_serviceSettings, 2f);
            Container.Rebind<MonsterServiceSettings>()
                .FromInstance(_serviceSettings)
                .AsCached();
        }
        
        [Test]
        public void EatState_OnTick_LeaveIfNoChair()
        {
            _statusComponent.MyChair.IsTaken.Returns(false); 
    
            var state = Container.Resolve<EatState>();
            state.Enter();
            state.OnTick();
    
            _animationComponent.Received(1).StopSit();
            _aiComponent.Received(1).ChangeState(MonsterState.Leave);
        }
        
        [Test]
        public void EatState_OnTick_LeaveIfFinishedEating()
        {
            var eatState1 = Container.Resolve<EatState>();
            eatState1.Enter();
            
            var timer1 = typeof(EatState).GetField("_timerTime",
                              BindingFlags.Instance | BindingFlags.NonPublic);
            timer1.SetValue(eatState1, 2.1f);
    
            eatState1.OnTick();
    
            _aiComponent.Received(1).FreeChairByMonster();
            _animationComponent.Received(1).StopSit();
            _statisticsManager.Received(1).IncrementStatistics("MonsterFedSkeleton");
            _aiComponent.Received(1).ChangeState(MonsterState.Leave);
            
            _aiComponent.ClearReceivedCalls();
            _animationComponent.ClearReceivedCalls();
            _statisticsManager.ClearReceivedCalls();
            
            var eatState2 = Container.Resolve<EatState>();
            eatState2.Enter();
            
            var timer2 = typeof(EatState).GetField("_timerTime",
                BindingFlags.Instance | BindingFlags.NonPublic);
            timer2.SetValue(eatState2, 0f);
    
            eatState2.OnTick();
    
            _aiComponent.DidNotReceive().FreeChairByMonster();
            _animationComponent.DidNotReceive().StopSit();
            _statisticsManager.DidNotReceive().IncrementStatistics("MonsterFedSkeleton");
            _aiComponent.DidNotReceive().ChangeState(MonsterState.Leave);
        }
        
        [Test]
        public void EatState_Exit_ScoreFood()
        {
            var eatState = Container.Resolve<EatState>();
            eatState.Enter();
            eatState.Exit();
            _scoringComponent.Received(1).ScoreFood();
        }
    
    }
}