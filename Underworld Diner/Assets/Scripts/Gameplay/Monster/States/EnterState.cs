using System;
using Interfaces;
using Zenject;

namespace Gameplay.Monster.States
{
    public class EnterState : BaseMonsterState
    {
        private const string MONSTER_ENTER_ID_TEMPLATE = "MonsterEnter{0}";
        
        [Inject] private IStatisticsManager _statisticsManager;
        
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private MonsterType _monsterType;
        
        private bool _isDestinationSet;
        
        public override void Initialize()
        {
            
        }

        public override void Enter()
        {
            _statisticsManager.IncrementStatistics(String.Format(MONSTER_ENTER_ID_TEMPLATE, _monsterType.ToString()));

        }

        public override void Exit()
        {
            //Debug.Log("EnterState exited");
        }
        

        public override void OnTick()
        {
            if (!_isDestinationSet)
            {
                _isDestinationSet = true;
                _navigation.ProcessMovement(new (-14, -3));
            }
            
            if (_navigation.HasReachedDestination())
            {
                _aiComponent.ChangeState(MonsterState.GoSit);
            }
        }
    }
}