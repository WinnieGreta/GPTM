using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.HUD.Buttons
{
    public class PauseButton : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private Button _pauseButton;

        private bool _gamePaused;
        
        public void OnEnable()
        {
            _pauseButton.onClick.AddListener(PauseButtonOnClick);
            _signalBus.DeclareSignal<OnGamePauseSignal>();
            _signalBus.DeclareSignal<OnGameUnpauseSignal>();
        }

        private void PauseButtonOnClick()
        {
            if (!_gamePaused)
            {
                //Debug.Log("Pause button pressed (PAUSED)");
                _signalBus.Fire<OnGamePauseSignal>();
                Time.timeScale = 0;
                
            }
            else
            {
                //Debug.Log("Pause button pressed again (UNPAUSED)");
                _signalBus.Fire<OnGameUnpauseSignal>();
                Time.timeScale = 1;
            }

            _gamePaused = !_gamePaused;
        }
        
    }
}