using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.HUD.Buttons
{
    public class PauseButton : MonoBehaviour
    {
        [Inject] private ITimeManager _timeManager;
        [SerializeField] private Button _pauseButton;

        private bool _gamePaused;
        
        public void OnEnable()
        {
            _pauseButton.onClick.AddListener(PauseButtonOnClick);
            
        }

        private void PauseButtonOnClick()
        {
            _timeManager.TogglePause();
        }
        
    }
}