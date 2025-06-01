using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Signals;
using System.Linq;
using Gameplay.Monster.Installer;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterFacadeTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<OnDespawnedSignal>();
            Container.DeclareSignal<OnSpawnedSignal>();
            Container.BindInstance(MonsterType.Skeleton).AsSingle();
            Container.Bind<INavigationComponent>()
                .FromInstance(Substitute.For<INavigationComponent>())
                .AsCached();
            Container.Bind<IAiComponent>()
                .FromInstance(Substitute.For<IAiComponent>())
                .AsCached();
            Container.Bind<IStatisticsManager>()
                .FromInstance(Substitute.For<IStatisticsManager>())
                .AsCached();
            Container.Bind<IAnimatorComponent>()
                .FromInstance(Substitute.For<IAnimatorComponent>())
                .AsCached();
            Container.Bind<Transform>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<MonsterFacade>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<MonsterStatusComponent>().AsSingle();
        }

        [Test]
        public void MonsterFacade_Initialization()
        {
            var monsterFacade = Container.Resolve<MonsterFacade>();
            Assert.IsNotNull(monsterFacade);
        }

        [Test]
        public void MonsterFacade_OnSpawned()
        {
            var transform = Container.CreateEmptyGameObject("Test").transform;
            transform.position = Random.Range(0, 100f) * Vector3.one;

            var monsterFacade = Container.Resolve<MonsterFacade>();
            monsterFacade.OnSpawned(transform);
            Assert.AreEqual(transform.position, monsterFacade.transform.position);
        }

        [Test]
        public void MonsterFacade_OnDespawned()
        {
            var monsterFacade = Container.Resolve<MonsterFacade>();
            var signalBus = Container.Resolve<SignalBus>();
            bool signalFired = false;
            signalBus.Subscribe<OnDespawnedSignal>(() => signalFired = true);

            monsterFacade.OnDespawned();
            Assert.IsTrue(signalFired);
        }

        [Test]
        public void MonsterFacade_Serve()
        {
            var monsterStatus = Container.Resolve<MonsterStatusComponent>();
            monsterStatus.ExpectedDish = DishType.Burger;

            var monsterFacade = Container.Resolve<MonsterFacade>();
            var initialExpectedDish = monsterFacade.ExpectedDish;

            monsterFacade.Serve(initialExpectedDish);
            Assert.AreEqual(DishType.None, monsterFacade.ExpectedDish);
            Assert.AreNotEqual(initialExpectedDish, monsterFacade.ExpectedDish);
        }

        [Test]
        public void MonsterFacade_Despawn()
        {
            var monsterFacade = Container.Resolve<MonsterFacade>();
            Container.BindMemoryPool<MonsterFacade, MonsterPool>()
                .WithInitialSize(0)
                .WithFactoryArguments(MonsterType.Skeleton)
                .FromNewComponentOnNewGameObject()
                .UnderTransformGroup("MonsterPools")
                .OnInstantiated<MonsterFacade>(((context, o) =>
                {
                    o.InjectPool(Container.ResolveAll<MonsterPool>().FirstOrDefault(x => x.Type == MonsterType.Skeleton));
                }));

            var monsterPool = Container.Resolve<MonsterPool>();
            monsterFacade.InjectPool(monsterPool);
            monsterFacade.Despawn();
            Assert.AreEqual(monsterPool.NumInactive, 1);
        }

        [Test]
        public void MonsterFacade_GetDamaged()
        {
            var monsterFacade = Container.Resolve<MonsterFacade>();
            var monsterStatus = Container.Resolve<MonsterStatusComponent>();
            var initialHealth = Container.Resolve<MonsterStatusComponent>().Health;

            var damage = 10f;
            var result = monsterFacade.GetDamaged(damage);
            Assert.IsTrue(result);
            Assert.AreEqual(initialHealth - damage, monsterStatus.Health);
        }

        [Test]
        public void MonsterFacade_ExpectedDish()
        {
            var monsterFacade = Container.Resolve<MonsterFacade>();
            var monsterStatus = Container.Resolve<MonsterStatusComponent>();

            Assert.AreEqual(DishType.None, monsterFacade.ExpectedDish);

            monsterStatus.ExpectedDish = DishType.Pudding;
            Assert.AreEqual(DishType.Pudding, monsterFacade.ExpectedDish);
        }   
        
        [Test]
        public void MonsterFacade_Start_FiresOnSpawnedSignal()
        {
            var monsterFacade = Container.Resolve<MonsterFacade>();
            bool signalFired = false;
            Container.Resolve<SignalBus>().Subscribe<OnSpawnedSignal>(() => signalFired = true);

            monsterFacade.Start();
            Assert.IsFalse(signalFired);
            monsterFacade.OnSpawned(monsterFacade.transform);
            Assert.IsTrue(signalFired);
        }
    }
}