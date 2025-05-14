using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager.LevelTimerManager
{
    public class LevelTimerComponent : ITickable, ITimeManager, IInitializable
    {
        [Inject] private IApplicationManager _applicationManager;
        [Inject] private SignalBus _signalBus;
        [Inject] private LevelBasicSettings _levelSettings;

        private bool _gamePaused;

        public float TimerTime { get; private set; }

        public void Initialize()
        {
            TimerTime = _levelSettings.LevelDuration;
        }

        public void Tick()
        {
            if (TimerTime > 0)
            {
                TimerTime -= Time.deltaTime;
            }
            else
            {
                _applicationManager.LoadMainMenu();
            }
        }

        public void TogglePause()
        {
            if (!_gamePaused)
            {
                PauseGame();
            }
            else
            {
                UnPauseGame();
            }

            _gamePaused = !_gamePaused;
        }

        private void PauseGame()
        {
            _signalBus.Fire<OnGamePauseSignal>();
            Time.timeScale = 0;
        }

        private void UnPauseGame()
        {
            _signalBus.Fire<OnGameUnpauseSignal>();
            Time.timeScale = 1;
        }
    }
}