using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager
{
    public class LevelTimerComponent : ITickable, ITimeManager, IInitializable
    {
        [Inject] private IApplicationManager _applicationManager;
        [Inject] private SignalBus _signalBus;
        [Inject] private LevelBasicSettings _levelSettings;

        private bool _gamePaused;
        private bool _levelFinished;

        public float TimerTime { get; private set; }

        [Inject]
        private void OnInject()
        {
            _signalBus.Subscribe<OnLevelStartSignal>(PauseTimerOnLevelStart);
        }
        
        public void Initialize()
        {
            TimerTime = _levelSettings.LevelDuration + 1;
            _levelFinished = false;
        }

        public void Tick()
        {
            if (TimerTime > 0)
            {
                TimerTime -= Time.deltaTime;
            }
            else
            {
                if (!_levelFinished)
                {
                    _levelFinished = true;
                    FinishLevelByTimer();
                }
                //Debug.Log("LevelFinished!");
                //_applicationManager.LoadMainMenu();
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

        private void FinishLevelByTimer()
        {
            _signalBus.Fire<OnLevelFinishedSignal>();
            Time.timeScale = 0;
        }

        private void PauseTimerOnLevelStart()
        {
            _gamePaused = true;
            Time.timeScale = 0;
        }
    }
}