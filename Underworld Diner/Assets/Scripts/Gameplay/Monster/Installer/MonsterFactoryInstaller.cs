using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.Installer
{
    [Serializable]
    public class MonsterPrefabPair
    {
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
                var type = pair.Prefab.GetComponent<MonsterMonoInstaller>().MonsterType;
                Container.BindMemoryPool<MonsterFacade, MonsterPool>()
                    // setting initial size to a non-zero value results in a bug: monsters other than default do not despawn
                    // caused by the nullref pool being injected before it is initialized
                    .WithInitialSize(0)
                    .WithFactoryArguments(type)
                    .FromComponentInNewPrefab(pair.Prefab)
                    .UnderTransformGroup("MonsterPools")
                    .OnInstantiated<MonsterFacade>(((context, o) =>
                    {
                        o.InjectPool(Container.ResolveAll<MonsterPool>().FirstOrDefault(x => x.Type == type));
                    }));
            }
            
            Container.BindFactory<MonsterType, Transform, IMonster, IMonster.Factory>()
                .FromFactory<MonsterFactory>();
        }
    }

}