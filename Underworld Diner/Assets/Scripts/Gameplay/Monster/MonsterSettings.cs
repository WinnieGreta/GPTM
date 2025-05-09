using System;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
        [CreateAssetMenu(fileName = "MonsterSettings", menuName = "Installers/MonsterSettings")]
        public class MonsterSettings : ScriptableObjectInstaller<MonsterSettings>
        {
            [SerializeField] private MonsterDowntimeSettings _monsterDowntimeSettings;
            public override void InstallBindings()
            {
                Container.BindInstance(_monsterDowntimeSettings).AsSingle();
            }
        }

        [Serializable]
        internal class MonsterDowntimeSettings
        {
            [field:SerializeField] public float OrderDowntime { get; private set; }
            [field:SerializeField] public float EatingDowntime { get; private set; }
        }
}