using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager
{
        [CreateAssetMenu(fileName = "MonsterSpawnSettings", menuName = "Installers/SpawnSettings")]
        public class SpawnSettings : ScriptableObjectInstaller<SpawnSettings>
        {
            [SerializeField] private MonsterSpawnSettings _monsterSpawnSettings;
            public override void InstallBindings()
            {
                Container.BindInstance(_monsterSpawnSettings).AsSingle();
            }
        }

        [Serializable]
        internal class MonsterSpawnSettings
        {
            [field:SerializeField] public List<MonsterSpawnConfig> SpawnConfig { get; private set; }
        }

        [Serializable]
        public class MonsterSpawnConfig
        {
            [field: SerializeField] public MonsterType Type { get; private set; }
            [field: SerializeField] public float SpawnPeriod { get; private set; }
        }
}