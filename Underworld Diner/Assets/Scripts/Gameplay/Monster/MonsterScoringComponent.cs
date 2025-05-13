﻿using System.Collections.Generic;
using Interfaces;
using Signals;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterScoringComponent
    {
        [Inject] private MonsterServiceSettings _serviceSettings;
        [Inject] private IRecipeBook _recipeBook;
        [Inject] private SignalBus _signalBus;
        [Inject] private MonsterStatusComponent _statusComponent;

        // TODO monster patience system
        private float _patienceMultiplier = 5;

        public void ScoreFood()
        {
            float score = 0;
            foreach (var dish in _statusComponent.FullOrder)
            {
                if (dish == DishType.None)
                {
                    continue;
                }
                score += _recipeBook[dish].CookingTime;
            }
            _signalBus.Fire(new OnMonsterScoredSignal() { Score = score * _serviceSettings.EatingDowntime * _patienceMultiplier});
        }
    }
}