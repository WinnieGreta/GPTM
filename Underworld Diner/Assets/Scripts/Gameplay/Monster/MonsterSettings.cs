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
            [SerializeField] private MonsterServiceSettings monsterServiceSettings;
            public override void InstallBindings()
            {
                Container.BindInstance(monsterServiceSettings).AsSingle();
            }
        }

        [Serializable]
        internal class MonsterServiceSettings
        {
            [field:SerializeField] public float OrderDowntime { get; private set; }
            [field:SerializeField] public float EatingDowntime { get; private set; }
            
            [field:SerializeField] public float StartingPatience { get; private set; }
            [field:SerializeField] public float PatienceDropSpeed { get; private set; }
        }
}