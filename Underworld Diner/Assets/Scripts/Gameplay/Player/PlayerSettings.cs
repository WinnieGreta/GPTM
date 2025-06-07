using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Installers/PlayerSettings")]
    public class PlayerSettings : ScriptableObjectInstaller<PlayerSettings>
    {
        [SerializeField] private PlayerMovementSettings _playerMovementSettings;
        [SerializeField] private PlayerHealthManaSettings _playerHealthManaSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(_playerMovementSettings).AsSingle();
            Container.BindInstance(_playerHealthManaSettings).AsSingle();
        }
    }

    [Serializable]
    internal class PlayerMovementSettings
    {
        [field:SerializeField] public float MaxSpeed { get; private set; }
    }

    [Serializable]
    internal class PlayerHealthManaSettings
    {
        [field:SerializeField] public float HpMax { get; private set; }
        [field:SerializeField] public float ManaMax { get; private set; }
        [field:SerializeField] public float ManaRegainSpeed { get; private set; }
    }
    
}