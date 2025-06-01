using Gameplay.Monster.Abstract;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class LeaveState : BaseMonsterState
    {
        [Inject] private INavigationComponent _navigation;
        [Inject] private IAiComponent _aiComponent;
        [Inject] private IDespawnable _despawnable;
        [Inject] private Transform _transform;
        
        public override void Enter()
        {
            //Debug.Log("Leaving");
            _navigation.ProcessMovement((Vector2)_transform.position + new Vector2(0, -1));
        }

        public override void OnTick()
        {
            if (_navigation.HasReachedDestination())
            {
                Leave();
            }
        }

        private void Leave()
        {
            _aiComponent.ChangeState(MonsterState.Null);
            _despawnable.Despawn();
            //Debug.Log("LeaveState disposed");
        }
        
    }
}