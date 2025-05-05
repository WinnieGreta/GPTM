using UnityEngine.AI;
using Zenject;
using UnityEngine;
using Interfaces;

namespace Gameplay.Monster
{
    public class MonsterNavigationComponent
    {
        [Inject] private NavMeshAgent _navMeshAgent;
        
        public Vector2 TargetPosition { get; private set; }
        private void MoveToPosition(Vector2 position)
        {
            NavMesh.SamplePosition(position, out var hit, 100, _navMeshAgent.areaMask);
            _navMeshAgent.SetDestination(hit.position);
        }

        public void ProcessMovement(Vector2 targetPosition)
        {
            TargetPosition.Set(targetPosition.x, targetPosition.y);
            MoveToPosition(TargetPosition);
        }

        private void ProcessStationMovement(IStation station)
        {
            var target = station.GetClosestAnchorPosition(_navMeshAgent.transform.position, _navMeshAgent.tag);
            MoveToPosition(target);
        }
    }
}