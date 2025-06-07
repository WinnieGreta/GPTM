using Gameplay.Monster.Abstract;
using Gameplay.Monster.Installer;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Signals;
using System.Linq;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterAiComponentTests : ZenjectUnitTestFixture
    {
        private BaseMonsterState.Factory _monsterStateFactory;
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Container.Bind<MonsterAIComponent>().AsSingle();
            SignalBusInstaller.Install(Container);
            // declare signals that will be used in MonsterAIComponent
            Container.DeclareSignal<OnDespawnedSignal>();
            Container.DeclareSignal<OnSpawnedSignal>();
            
            // bind mockup for all injected interfaces into MonsterAIComponent
            Container.Bind<MonsterStatusComponent>().FromInstance(Substitute.For<MonsterStatusComponent>()).AsSingle();
            Container.Bind<MonsterServiceSettings>().FromInstance(Substitute.For<MonsterServiceSettings>()).AsSingle();
            Container.Bind<IMonster>().FromInstance(Substitute.For<IMonster>()).AsSingle();
            
            // bind the factory for BaseMonsterState for ifactory of mockup
            _monsterStateFactory = Substitute.For<BaseMonsterState.Factory>();
            Container.BindFactory<MonsterState, BaseMonsterState, BaseMonsterState.Factory>()
                .FromIFactory(x => x
                    .FromInstance(_monsterStateFactory)
                    .AsCached());
            
        }

        [Test]
        public void MonsterAiComponent_Initialization()
        {
            var monsterAiComponent = Container.Resolve<MonsterAIComponent>();
            Assert.IsNotNull(monsterAiComponent);
            Assert.IsInstanceOf<MonsterAIComponent>(monsterAiComponent);
        }
        
        [Test]
        public void MonsterAiComponent_ChangeState()
        {
            var monsterAiComponent = Container.Resolve<MonsterAIComponent>();
            
            // we need to mock BaseMonsterState.Factory to return specific state for specific argument
            var enterState = Substitute.For<BaseMonsterState>();
            _monsterStateFactory.Create(MonsterState.Enter).Returns(enterState);
            var dieState = Substitute.For<BaseMonsterState>();
            _monsterStateFactory.Create(MonsterState.Die).Returns(dieState);
            
            monsterAiComponent.ChangeState(MonsterState.Enter);
            // cannot assert the state directly, but we can check if Enter method was called
            enterState.Received(1).Enter();
            
            monsterAiComponent.ChangeState(MonsterState.Die);
            // again, check if Die method was called
            dieState.Received(1).Enter();
        }
        
        [Test]
        public void MonsterAiComponent_Tick()
        {
            var monsterAiComponent = Container.Resolve<MonsterAIComponent>();
            var statusComponent = Container.Resolve<MonsterStatusComponent>();
            statusComponent.Health = 100f; // Set initial health
            
            // Mock the current state entity to return a specific state
            var tickState = Substitute.For<BaseMonsterState>();
            _monsterStateFactory.Create(MonsterState.Enter).Returns(tickState);
            var dieState = Substitute.For<BaseMonsterState>();
            _monsterStateFactory.Create(MonsterState.Die).Returns(dieState);
            
            monsterAiComponent.ChangeState(MonsterState.Enter);
            
            // Call Tick and check if OnTick was called
            monsterAiComponent.Tick();
            tickState.Received(1).OnTick();
            
            // Now set health to 0 to trigger death state
            statusComponent.Health = 0f;
            monsterAiComponent.Tick();
            
            // Check if the state changed to Die
            dieState.Received(1).Enter();
        }
        
        [Test]
        public void MonsterAiComponent_TakeChairByMonster()
        {
            var monsterAiComponent = Container.Resolve<MonsterAIComponent>();
            var chair = Substitute.For<IChair>();
            var statusComponent = Container.Resolve<MonsterStatusComponent>();

            monsterAiComponent.TakeChairByMonster(chair);
            
            // Check if the chair was taken by the monster
            chair.Received(1).TakeChair(Arg.Any<IMonster>());
            Assert.AreEqual(chair, statusComponent.MyChair);
        }
        
        [Test]
        public void MonsterAiComponent_FreeChairByMonster()
        {
            var monsterAiComponent = Container.Resolve<MonsterAIComponent>();
            var chair = Substitute.For<IChair>();
            var statusComponent = Container.Resolve<MonsterStatusComponent>();
            statusComponent.MyChair = chair;

            monsterAiComponent.FreeChairByMonster();
            
            // Check if the chair was freed
            chair.Received(1).FreeChair();
            Assert.IsNull(statusComponent.MyChair);
        }
        
        [Test]
        public void MonsterAiComponent_StartMonster()
        {
            var monsterAiComponent = Container.Resolve<MonsterAIComponent>();
            var statusComponent = Container.Resolve<MonsterStatusComponent>();
            var signalBus = Container.Resolve<SignalBus>();

            // Fire the OnSpawnedSignal to start the monster
            signalBus.Fire(new OnSpawnedSignal());

            // Check if the status component was reset
            Assert.IsEmpty(statusComponent.FullOrder);
            Assert.AreEqual(0, statusComponent.Patience);
            Assert.AreEqual(0, statusComponent.Health);
        }
    }
}