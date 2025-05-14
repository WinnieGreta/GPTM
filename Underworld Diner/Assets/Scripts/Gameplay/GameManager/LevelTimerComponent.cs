using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager.LevelTimerManager
{
    public class LevelTimerComponent : ITickable, ITimeManager
    {
        [Inject] private IApplicationManager _applicationManager;
        [Inject] private SignalBus _signalBus;

        private bool _gamePaused;

        public float timerTime = 3;
        
        public void Tick()
        {
            if (timerTime > 0)
            {
                timerTime -= Time.deltaTime;
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