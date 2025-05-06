using Gameplay.Station.Chair;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class GoSitState : MonsterStateEntity
    {
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;

        private ChairFacade[] _chairs;
        private bool _isDestinationSet;
        
        public override void Enter()
        {
            _chairs = GameObject.FindObjectsByType<ChairFacade>(FindObjectsSortMode.None);
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
            foreach (var chair in _chairs)
            {
                if (!chair.IsTaken)
                {
                    _navigation.ProcessStationMovement(chair);
                    Debug.Log("Found free chair");
                    chair.IsTaken = true;
                    return true;
                }
            }
            Debug.Log("Couldn't find free chair");
            return false;
        }
    }
}