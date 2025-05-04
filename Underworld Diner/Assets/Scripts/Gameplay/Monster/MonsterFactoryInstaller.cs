using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    [Serializable]
    public class MonsterPrefabPair
    {
        public MonsterType Type;
        public GameObject Prefab;
        public int InitialSize = 10;
    }

    public class MonsterFactoryInstaller : MonoInstaller
    {
        [SerializeField] List<MonsterPrefabPair> _monsterPrefabs;

        public override void InstallBindings()
        {
            foreach (var pair in _monsterPrefabs)
            {
                Container.BindMemoryPool<MonsterFacade, MonsterPool>()
                    .WithInitialSize(pair.InitialSize)
                    .WithFactoryArguments(pair.Type)
                    .FromComponentInNewPrefab(pair.Prefab)
                    .UnderTransformGroup("MonsterPools");
            }
            
            Container.BindFactory<MonsterType, Transform, IMonster, IMonster.Factory>()
                .FromFactory<MonsterFactory>();
        }
    }

}