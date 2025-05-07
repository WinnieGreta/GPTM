using Gameplay.Station.Chair;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class GoSitState : MonsterStateEntity
    {
        [Inject] private IMonster _monster;
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private IChairManager _chairManager;
        
        private bool _isDestinationSet;
        
        public override void Enter()
        {
            
        }

        public override void OnTick()
        {
            if (!_isDestinationSet)
            {
                _isDestinationSet = true;
                if (!FindFreeChair())
                {
                    _aiComponent.ChangeState(MonsterState.Leave);
                }
            }

            if (_navigation.HasReachedDestination())
            {
                _aiComponent.ChangeState(MonsterState.Sit);
            }
        }

        private bool FindFreeChair()
        {
            foreach (var chair in _chairManager.Chairs)
            {
                if (!chair.IsTaken)
                {
                    _aiComponent.TakeChairByMonster(chair, _monster);
                    _navigation.ProcessStationMovement(chair);
                    Debug.Log("Found free chair");
                    return true;
                }
            }
            Debug.Log("Couldn't find free chair");
            return false;
        }
    }
}