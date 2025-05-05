using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class LeaveState : MonsterStateEntity
    {
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private IDespawnable _despawnable;
        
        public override void Initialize()
        {
            
        }

        public override void Enter()
        {
            Debug.Log("Leaving");
            _navigation.ProcessMovement(new Vector2(2, 2));
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
            Debug.Log("LeaveState disposed");
        }
        
    }
}