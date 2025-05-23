using System;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Installers/LevelSettings")]
    public class LevelSettings : ScriptableObjectInstaller<LevelSettings>
    {
        [SerializeField] private LevelBasicSettings _levelSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(_levelSettings).AsSingle();
        }
    }

    [Serializable]
    internal class LevelBasicSettings
    {
        [field:SerializeField] public float LevelDuration { get; private set; }
    }
}