using Interfaces;
using Interfaces.UI;
using NSubstitute;
using NUnit.Framework;
using Signals;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterPatienceComponentTests : ZenjectUnitTestFixture
    {
        private IPatienceMeter.Factory _patienceMeterFactory;
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Container.Bind<MonsterPatienceComponent>().AsSingle();
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<OnDespawnedSignal>();
            Container.DeclareSignal<OnSpawnedSignal>();
            
            //bind mockup for all injected interfaces into MonsterPatienceComponent
            Container.Bind<MonsterStatusComponent>().AsSingle();
            Container.Bind<MonsterServiceSettings>().FromInstance(Substitute.For<MonsterServiceSettings>()).AsSingle();
            Container.Bind<IMonster>().FromInstance(Substitute.For<IMonster>()).AsSingle();
            Container.Bind<NavMeshAgent>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();
            
            //bind the factory for IPatienceMeter from ifactory
            _patienceMeterFactory = Substitute.For<IPatienceMeter.Factory>();
            Container.BindFactory<Transform, int, IPatienceMeter, IPatienceMeter.Factory>()
                .FromIFactory(x=>x.FromInstance(_patienceMeterFactory).AsCached());
        }

        [Test]
        public void MonsterPatienceComponent_Initialization()
        {
            var monsterPatienceComponent = Container.Resolve<MonsterPatienceComponent>();
            Assert.IsNotNull(monsterPatienceComponent);
            Assert.IsInstanceOf<MonsterPatienceComponent>(monsterPatienceComponent);
            
            //call Initialize method to ensure all dependencies are set
            monsterPatienceComponent.Initialize();
            
            //check signals are subscribed by NumSubscribers.
            //we have 2 subscriptions: OnSpawnedSignal and OnDespawnedSignal
            var signalBus = Container.Resolve<SignalBus>();
            Assert.AreEqual(2, signalBus.NumSubscribers);
        }
        
        [Test]
        public void MonsterPatienceComponent_OnSpawned()
        {
            //call initialization method
            var monsterPatienceComponent = Container.Resolve<MonsterPatienceComponent>();
            monsterPatienceComponent.Initialize();
            
            //set navigation component transform
            var navMeshAgent = Container.Resolve<NavMeshAgent>();
            
            //set starting patience value
            var startingPatience = 25;
            var settings = Container.Resolve<MonsterServiceSettings>();
            settings.SetSerializedProperty("StartingPatience", startingPatience);
            
            //fire signal OnSpawnedSignal
            var signalBus = Container.Resolve<SignalBus>();
            signalBus.Fire(new OnSpawnedSignal());
            
            //check factory was called with correct parameters
            _patienceMeterFactory.Received(1).Create(navMeshAgent.transform, startingPatience);
        }

        [Test]
        public void MonsterPatienceComponent_OnDespawned()
        {
            //call initialization method
            var monsterPatienceComponent = Container.Resolve<MonsterPatienceComponent>();
            monsterPatienceComponent.Initialize();

            //create mockup for IPatienceMeter
            var patienceMeter = Substitute.For<IPatienceMeter>();
            _patienceMeterFactory.Create(Arg.Any<Transform>(), Arg.Any<int>()).Returns(patienceMeter);

            //fire signal OnSpawnedSignal to create patience meter
            var signalBus = Container.Resolve<SignalBus>();
            signalBus.Fire(new OnSpawnedSignal());
            //check if patience meter was created
            _patienceMeterFactory.Received(1).Create(Arg.Any<Transform>(), Arg.Any<int>());
            
            //fire signal OnDespawnedSignal to despawn patience meter
            signalBus.Fire(new OnDespawnedSignal());

            //check if patience meter was despawned
            patienceMeter.Received(1).Despawn();
        }
        
        [Test]
        public void MonsterPatienceComponent_Tick()
        {
            //call initialization method
            var monsterPatienceComponent = Container.Resolve<MonsterPatienceComponent>();
            monsterPatienceComponent.Initialize();

            //create mockup for IPatienceMeter
            var patienceMeter = Substitute.For<IPatienceMeter>();
            _patienceMeterFactory.Create(Arg.Any<Transform>(), Arg.Any<int>()).Returns(patienceMeter);

            //fire signal OnSpawnedSignal to create patience meter
            var signalBus = Container.Resolve<SignalBus>();
            signalBus.Fire(new OnSpawnedSignal());
            
            //set status component and patience meter values
            var statusComponent = Container.Resolve<MonsterStatusComponent>();
            statusComponent.Patience = 10f;
            statusComponent.Health = 100f;
            
            //resolve nav mesh agent transform
            var navMeshAgent = Container.Resolve<NavMeshAgent>();
            
            //call Tick method
            monsterPatienceComponent.Tick();
            
            //check if UpdateHealth and UpdatePatience methods were called
            patienceMeter.Received(1).UpdateHeartAmount(10f, navMeshAgent.transform, 100f);
            patienceMeter.Received(1).UpdatePatienceMeter(10f, navMeshAgent.transform);
        }
    }
}