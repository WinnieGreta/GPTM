using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class LeaveState : BaseMonsterState
    {
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private IDespawnable _despawnable;
        [Inject] private Transform _transform;
        
        public override void Initialize()
        {
            
        }

        public override void Enter()
        {
            //Debug.Log("Leaving");
            _navigation.ProcessMovement((Vector2)_transform.position + new Vector2(0, -1));
        }

        public override void OnTick()
        {
            if (_navigation.HasReachedDestination())
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _aiComponent.ChangeState(MonsterState.Null);
            _despawnable.Despawn();
            //Debug.Log("LeaveState disposed");
        }
        
    }
}