﻿using System.Collections.Generic;
using Analytics.Signals;
using Interfaces;
using UnityEngine;
using Zenject;


namespace Gameplay.GameManager
{
    public class GameManager : IGameManager, IFixedTickable, IInitializable, IChairManager
    {
        [Inject] private GameSpawnManager _spawnManager;
        [Inject] private SignalBus _signalBus;
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
    }
}