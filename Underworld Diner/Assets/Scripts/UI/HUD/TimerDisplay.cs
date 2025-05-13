using System;
using Gameplay.GameManager.LevelTimerManager;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.HUD
{
    public class TimerDisplay : MonoBehaviour
    {
        [Inject] private LevelTimerComponent _timerComponent;

        [SerializeField] private TMP_Text _minutesText ;
        [SerializeField] private TMP_Text _secondsText;

        public void Update()
        {
            _minutesText.text = string.Format("{0:0}", (int)_timerComponent.timerTime / 60);
            _secondsText.text = string.Format("{0:00}", (int)_timerComponent.timerTime % 60);
        }
    }
}