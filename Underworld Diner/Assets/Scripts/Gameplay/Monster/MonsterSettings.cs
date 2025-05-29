using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Monster
{
        [CreateAssetMenu(fileName = "MonsterSettings", menuName = "Installers/MonsterSettings")]
        public class MonsterSettings : ScriptableObjectInstaller<MonsterSettings>
        {
            [SerializeField] private MonsterServiceSettings _monsterServiceSettings;
            [SerializeField] private MonsterLootSettings _monsterLootSettings;
            
            public override void InstallBindings()
            {
                Container.BindInstance(_monsterServiceSettings).AsSingle();
                Container.BindInstance(_monsterLootSettings).AsSingle();
            }
        }

        [Serializable]
        internal class MonsterServiceSettings
        {
            [field:SerializeField] public float OrderDowntime { get; private set; }
            [field:SerializeField] public float EatingDowntime { get; private set; }
            
            [field:SerializeField] public float StartingPatience { get; private set; }
            [field:SerializeField] public float PatienceDropSpeed { get; private set; }
            
            [field:SerializeField] public float StartingHealth { get; private set; }
        }

        [Serializable]
        internal class MonsterLootSettings
        {
            [field: SerializeField] public int RedDrop { get; private set; }
            [field: SerializeField] public int GreenDrop { get; private set; }
            [field: SerializeField] public int BlueDrop { get; private set; }
        }
}