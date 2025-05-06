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
            TargetPosition = targetPosition;
            MoveToPosition(TargetPosition);
        }

        private void ProcessStationMovement(IStation station)
        {
            var target = station.GetClosestAnchorPosition(_navMeshAgent);
            MoveToPosition(target);
        }

        public bool HasReachedDestination()
        {
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}