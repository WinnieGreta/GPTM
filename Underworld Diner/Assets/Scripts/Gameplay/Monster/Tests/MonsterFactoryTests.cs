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
    public class MonsterFactoryTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            SignalBusInstaller.Install(Container);

            Container.Bind<MonsterFactory>().AsSingle();
            SetUp_BindFactory(MonsterType.Skeleton);
            SetUp_BindFactory(MonsterType.Ork);
        }

        private void SetUp_BindFactory(MonsterType monsterType)
        {
            //Creating subContainer for each monster type.
            //This is different from the original code as we had MonsterMonoInstaller there
            Container.BindMemoryPool<MonsterFacade, MonsterPool>()
                .WithInitialSize(1)
                .WithFactoryArguments(monsterType)
                .FromSubContainerResolve()
                .ByMethod(x=>SetUp_BindMonster(x, monsterType))
                .WithArguments(monsterType);
        }

        private void SetUp_BindMonster(DiContainer container, MonsterType monsterType)
        {
            Container.DeclareSignal<OnDespawnedSignal>();
            Container.DeclareSignal<OnSpawnedSignal>();
            container.Bind<INavigationComponent>()
                .FromInstance(Substitute.For<INavigationComponent>())
                .AsCached();
            container.Bind<IAiComponent>()
                .FromInstance(Substitute.For<IAiComponent>())
                .AsCached();
            container.Bind<IStatisticsManager>()
                .FromInstance(Substitute.For<IStatisticsManager>())
                .AsCached();
            container.Bind<IAnimatorComponent>()
                .FromInstance(Substitute.For<IAnimatorComponent>())
                .AsCached();
            container.Bind<MonsterStatusComponent>().AsSingle();
            
            var go = new GameObject($"{monsterType}Facade");
            container.BindInterfacesAndSelfTo<MonsterFacade>()
                .FromNewComponentOn(go)
                .AsSingle()
                .OnInstantiated<MonsterFacade>((_, x) =>
                {
                    
                    x.InjectPool(container.ResolveAll<MonsterPool>().FirstOrDefault(x => x.Type == monsterType));
                }).NonLazy();
            container.BindInstance(go.transform);
        }

        [Test]
        public void MonsterFactory_Initialization()
        {

            var monsterFactory = Container.Resolve<MonsterFactory>();

            Assert.IsNotNull(monsterFactory);
            Assert.IsInstanceOf<MonsterFactory>(monsterFactory);
        }
        
        [Test]
        public void MonsterFactory_CreateMonster()
        {
            var monsterFactory = Container.Resolve<MonsterFactory>();
            var monsterType = MonsterType.Skeleton;
            var monster = monsterFactory.Create(monsterType, new GameObject().transform);

            Assert.IsNotNull(monster);
            Assert.IsInstanceOf<IMonster>(monster);
            //monster type is not exposed in IMonster, so we check the facade name, as names are sorted by pools
            Assert.AreEqual($"{monsterType}Facade", (monster as MonsterFacade)?.name);
            
            monsterType = MonsterType.Ork;
            monster = monsterFactory.Create(monsterType, new GameObject().transform);

            Assert.IsNotNull(monster);
            Assert.IsInstanceOf<IMonster>(monster);
            Assert.AreEqual($"{monsterType}Facade", (monster as MonsterFacade)?.name);
        }

        [Test]
        public void MonsterFactory_CreateMonster_InvalidType()
        {
            var monsterFactory = Container.Resolve<MonsterFactory>();
            Assert.Throws<System.ArgumentException>(() => monsterFactory.Create((MonsterType)999, new GameObject().transform));
        }
    }
}