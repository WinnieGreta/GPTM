using Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using Gameplay.Monster.Installer;
using UnityEngine;
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
            
            installer.SetSerializedField("_monsterPrefabs", new List<MonsterPrefabPair>
            {
                SetUpMonsterPrefabPair(MonsterType.Ork),
                SetUpMonsterPrefabPair(MonsterType.Skeleton)
            });
        }
        
        private MonsterPrefabPair SetUpMonsterPrefabPair(MonsterType monsterType)
        {
            var prefab = new GameObject("OrkPrefab");
            var installer = prefab.AddComponent<MonsterMonoInstaller>();
            
            installer.SetSerializedProperty("MonsterType", monsterType);
            
            return new MonsterPrefabPair
            {
                InitialSize = 0,
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
            Assert.AreEqual(2, monsterPools.Count); // Assuming we have 2 monster types in the installer
        }
    }
}