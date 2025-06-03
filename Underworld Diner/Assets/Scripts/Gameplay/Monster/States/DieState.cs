using System;
using Gameplay.Monster.Abstract;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class DieState : BaseMonsterState
    {
        internal const string MONSTER_KILL_ID_TEMPLATE = "MonsterKill{0}";
        
        [Inject] private IStatisticsManager _statisticsManager;
        
        [Inject] private IAiComponent _aiComponent;
        [Inject] private INavigationComponent _navigationComponent;
        [Inject] private IAnimatorComponent _animatorComponent;
        [Inject] private IDespawnable _despawnable;
        [Inject] private IResourceManager _resourceManager;
        [Inject] private MonsterLootSettings _monsterLootSettings;
        [Inject] private MonsterType _monsterType;
        
        // time from death to corpse despawn 
        private float _corpseTime;
        private float _timerTime;
        
        public override void Enter()
        {
            //Debug.Log("I'm dead");
            _statisticsManager.IncrementStatistics(String.Format(MONSTER_KILL_ID_TEMPLATE, _monsterType.ToString()));
            _navigationComponent.StopOnDeath();
            _aiComponent.FreeChairByMonster();
            _animatorComponent.DeathAnimation();
            _resourceManager.GainResources(_monsterLootSettings.RedDrop, _monsterLootSettings.GreenDrop, _monsterLootSettings.BlueDrop);
            _corpseTime = 3;
            _timerTime = 0;
        }

        public override void OnTick()
        {
            _timerTime += Time.deltaTime;
            if (_timerTime >= _corpseTime)
            {
                _aiComponent.ChangeState(MonsterState.Null);
                _despawnable.Despawn();
            }
        }
        
    }
}