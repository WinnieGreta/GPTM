using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Installers/PlayerSettings")]
    public class PlayerSettings : ScriptableObjectInstaller<PlayerSettings>
    {
        [SerializeField] private PlayerMovementSettings _playerMovementSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(_playerMovementSettings).AsSingle();
        }
    }

    [Serializable]
    internal class PlayerMovementSettings
    {
        [field:SerializeField] public float MaxSpeed { get; private set; }
    }
    
}