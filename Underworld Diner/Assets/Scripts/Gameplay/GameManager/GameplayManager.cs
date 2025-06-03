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
        [Inject] private IResourceManager _resourceManager;

        [Inject]
        private void OnInject()
        {
            _signalBus.Subscribe<OnLevelFinishedSignal>(DisplayScorePopup);
            _signalBus.Subscribe<OnGamePauseSignal>(DisplayPausePopup);
            _signalBus.Subscribe<OnGameUnpauseSignal>(HidePausePopup);
        }

        public void Initialize()
        {
           _spawnManager.OnInitialize();
           _resourceManager.InitializeResources();
           _signalBus.TryFire<AnalyticsLevelStartEvent>();
           _signalBus.TryFire<OnLevelStartSignal>();
           DisplayStartLevelPopup();
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
        
        private void DisplayScorePopup()
        {
            _uiCanvases[0].gameObject.SetActive(true);
        }
        
        private void DisplayPausePopup()
        {
            _uiCanvases[1].gameObject.SetActive(true);
        }
        
        private void HidePausePopup()
        {
            _uiCanvases[1].gameObject.SetActive(false);
            // start screen is basically a pause popup under the hood
            _uiCanvases[2].gameObject.SetActive(false);
        }

        private void DisplayStartLevelPopup()
        {
            _uiCanvases[2].gameObject.SetActive(true);
        }

    }
}
