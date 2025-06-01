using System.Collections.Generic;
using Analytics.Signals;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;


namespace Gameplay.GameManager
{
    public class GameplayManager : IGameManager, IFixedTickable, IInitializable, IChairManager
    {
        [Inject] private GameSpawnManager _spawnManager;
        [Inject] private SignalBus _signalBus;
        [Inject] private List<Canvas> _uiCanvases;

        [Inject]
        private void OnInject()
        {
            _signalBus.Subscribe<OnLevelFinishedSignal>(DisplayScore);
            _signalBus.Subscribe<OnGamePauseSignal>(DisplayPause);
            _signalBus.Subscribe<OnGameUnpauseSignal>(HidePause);
        }

        public void Initialize()
        {
           _spawnManager.OnInitialize();
           _signalBus.TryFire<AnalyticsLevelStartEvent>();
        }
        
        public void FixedTick()
        {
            _spawnManager.OnFixedTick();
        }

        #region IChairManager

        public IReadOnlyList<IChair> Chairs => _chairs;
        private List<IChair> _chairs = new ();
        
        public void Register(IChair chair)
        {
            //Debug.Log("Registering chairs");
            if (_chairs.Contains(chair))
            {
                //Debug.LogError("The chair has already been added");
                return;
            }
            _chairs.Add(chair);
        }

        #endregion
        
        private void DisplayScore()
        {
            _uiCanvases[0].gameObject.SetActive(true);
        }
        
        private void DisplayPause()
        {
            _uiCanvases[1].gameObject.SetActive(true);
        }
        
        private void HidePause()
        {
            _uiCanvases[1].gameObject.SetActive(false);
        }
    }
}
