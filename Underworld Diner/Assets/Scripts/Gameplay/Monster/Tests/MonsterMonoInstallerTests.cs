using Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using Gameplay.Monster.Installer;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterMonoInstallerTests: ZenjectUnitTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            var go = new GameObject("MonsterMonoInstaller");
            var animator = go.AddComponent<Animator>();
            var navMeshAgent = go.AddComponent<NavMeshAgent>();
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            var favoriteDishes = new List<DishType> { DishType.ChocolateCake, DishType.Burger };
            
            var installer = go.AddComponent<MonsterMonoInstaller>();
            installer.SetSerializedProperty("MonsterType", MonsterType.Skeleton);
            installer.SetSerializedField("_animator", animator);
            installer.SetSerializedField("_navMeshAgent", navMeshAgent);
            installer.SetSerializedField("_spriteRenderer", spriteRenderer);
            installer.SetSerializedField("_favoriteDishes", favoriteDishes);

            var context = go.AddComponent<GameObjectContext>();
            context.Installers = new MonoInstaller[] { installer };
            Container.BindInstance(context);
        }

        [Test]
        public void MonsterMonoInstaller_Initialization()
        {
            var gameObjectContext = Container.Resolve<GameObjectContext>();
            gameObjectContext.Install(Container);
            var container = gameObjectContext.Container;
            
            //resolve and assert installed components
            var animator = container.Resolve<Animator>();
            var navMeshAgent = container.Resolve<NavMeshAgent>();
            var spriteRenderer = container.Resolve<SpriteRenderer>();
            var monsterType = container.Resolve<MonsterType>();
            var favoriteDishes = container.Resolve<List<DishType>>();
            Assert.IsNotNull(animator);
            Assert.IsNotNull(navMeshAgent);
            Assert.IsNotNull(spriteRenderer);
            Assert.AreEqual(MonsterType.Skeleton, monsterType);
            Assert.IsNotNull(favoriteDishes);
            Assert.IsTrue(favoriteDishes.Contains(DishType.ChocolateCake));
            Assert.IsTrue(favoriteDishes.Contains(DishType.Burger));
            
        }
    }
}