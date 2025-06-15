using System;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Installers/LevelSettings")]
    public class LevelSettings : ScriptableObjectInstaller<LevelSettings>
    {
        [SerializeField] private LevelBasicSettings _levelSettings;
        [SerializeField] private LevelResourceSettings _levelResourceSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(_levelSettings).AsSingle();
            Container.BindInstance(_levelResourceSettings).AsSingle();
        }
    }

    [Serializable]
    public class LevelBasicSettings
    {
        [field:SerializeField] public float LevelDuration { get; private set; }
        [field:SerializeField] public string LevelName { get; private set; }
        [field:SerializeField] public int LevelScoreGoal { get; private set; }
    }

    [Serializable]
    public class LevelResourceSettings
    {
        [field:SerializeField] public int StartingRed { get; private set; }
        [field:SerializeField] public int StartingGreen { get; private set; }
        [field:SerializeField] public int StartingBlue { get; private set; }
    }
}