using System;
using Gameplay.Monster.Abstract;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class EnterState : BaseMonsterState
    {
        internal const string MONSTER_ENTER_ID_TEMPLATE = "MonsterEnter{0}";
        
        [Inject] private IStatisticsManager _statisticsManager;
        
        [Inject] private INavigationComponent _navigation;
        [Inject] private IAiComponent _aiComponent;
        [Inject] private MonsterType _monsterType;
        [Inject] private Transform _transform;

        [Inject] private MonsterAnimatorComponent _animatorComponent;
        
        private bool _isDestinationSet;

        public override void Enter()
        {
            _animatorComponent.Restart();
            _statisticsManager.IncrementStatistics(String.Format(MONSTER_ENTER_ID_TEMPLATE, _monsterType.ToString()));

        }

        public override void OnTick()
        {
            if (!_isDestinationSet)
            {
                _isDestinationSet = true;
                _navigation.ProcessMovement((Vector2)_transform.position + new Vector2(0, -5));
            }
            
            if (_navigation.HasReachedDestination())
            {
                _aiComponent.ChangeState(MonsterState.GoSit);
            }
        }
    }
}