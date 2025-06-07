using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapon.Fireball
{
    [CreateAssetMenu(fileName = "FireballSettings", menuName = "Installers/WeaponSettings/ProjectilesSettings/Fireball")]
    public class FireballSettings : ScriptableObjectInstaller<FireballSettings>
    {
        [SerializeField] private FireballSimpleSettings _fireballSimpleSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_fireballSimpleSettings).AsSingle();
        }
    }

    [Serializable]
    internal class FireballSimpleSettings
    {
        [field:SerializeField] public float Speed { get; private set; }
        [field:SerializeField] public float Damage { get; private set; }
        [field:SerializeField] public int HitsLeft { get; private set; }
    }
}