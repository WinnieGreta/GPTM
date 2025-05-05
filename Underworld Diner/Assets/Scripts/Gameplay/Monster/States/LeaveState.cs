using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class LeaveState : MonsterStateEntity
    {
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        
        public override void Initialize()
        {
            
        }

        public override void Start()
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

        public override void Dispose()
        {
            Debug.Log("LeaveState disposed");
        }
        
    }
}