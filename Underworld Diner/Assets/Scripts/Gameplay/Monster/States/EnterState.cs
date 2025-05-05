using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.States
{
    public class EnterState : MonsterStateEntity
    {
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        
        public override void Initialize()
        {
            
        }

        public override void Start()
        {
            _navigation.ProcessMovement(Vector2.zero);
        }

        public override void Dispose()
        {
            Debug.Log("EnterState disposed");
        }
        

        public override void OnTick()
        {
            if (_navigation.HasReachedDestination())
            {
                _aiComponent.ChangeState(MonsterState.Leave);
            }
        }
    }
}