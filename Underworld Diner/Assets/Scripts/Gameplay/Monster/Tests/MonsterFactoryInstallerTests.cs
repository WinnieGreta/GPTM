using Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Gameplay.Monster.Installer;
using Interfaces.UI;
using NSubstitute;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterFactoryInstallerTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            SignalBusInstaller.Install(Container);
            ZenjectManagersInstaller.Install(Container);
            //create a new GameObject for the installer. use reflection to set up prefabs private serialized fields
            SetUpInstaller();
        }

        private void SetUpInstaller()
        {
            var go = new GameObject("MonsterFactoryInstaller");
            var installer = go.AddComponent<MonsterFactoryInstaller>();
            var context = go.AddComponent<GameObjectContext>();
            context.Installers = new MonoInstaller[] { installer };
            Container.BindInstance(context);
            Container.BindInstance(new MonsterServiceSettings());
            Container.BindFactory<Transform, int, IPatienceMeter, IPatienceMeter.Factory>()
                .FromMethod((c, _, __) => Substitute.For<IPatienceMeter>());


            installer.SetSerializedField("_monsterPrefabs", new List<MonsterPrefabPair>
            {
                SetUpMonsterPrefabPair(MonsterType.Ork),
                SetUpMonsterPrefabPair(MonsterType.Skeleton)
            });
        }

        private MonsterPrefabPair SetUpMonsterPrefabPair(MonsterType monsterType)
        {
            var prefab = new GameObject($"{monsterType}Prefab");

            var context = prefab.AddComponent<GameObjectContext>();
            var installer = prefab.AddComponent<MonsterMonoInstaller>();
            var facade = prefab.AddComponent<MonsterFacade>();

            installer.SetSerializedProperty("MonsterType", monsterType);
            installer.SetSerializedField("_animator", prefab.AddComponent<Animator>());
            installer.SetSerializedField("_transform", prefab.AddComponent<Transform>());
            installer.SetSerializedField("_navMeshAgent", prefab.AddComponent<NavMeshAgent>());
            installer.SetSerializedField("_spriteRenderer", prefab.AddComponent<SpriteRenderer>());
            installer.SetSerializedField("_favoriteDishes", new List<DishType>(0));
            context.Installers = new MonoInstaller[] { installer };

            return new MonsterPrefabPair
            {
                InitialSize = 1,
                Prefab = prefab
            };
        }

        [Test]
        public void MonsterFactoryInstaller_InstallBindings()
        {
            var context = Container.Resolve<GameObjectContext>();
            context.Install(Container);

            var monsterPools = context.Container.ResolveAll<MonsterPool>();
            Assert.IsNotEmpty(monsterPools);
            Assert.AreEqual(2, monsterPools.Count);
            // Assuming we have 2 monster types in the installer

            //spawn monster and check it's removed from pool by count
            var orkPool = monsterPools.FirstOrDefault(x => x.Type == MonsterType.Ork);
            Assert.IsNotNull(orkPool);
            var startingCount = orkPool.NumInactive;
            var orkMonster = orkPool.Spawn(context.transform);
            Assert.IsNotNull(orkMonster);
            Assert.AreEqual(startingCount - 1, orkPool.NumInactive);

            //despawn the monster and check it's returned to pool
            orkPool.Despawn(orkMonster);
            Assert.AreEqual(startingCount, orkPool.NumInactive);
        }
    }
}