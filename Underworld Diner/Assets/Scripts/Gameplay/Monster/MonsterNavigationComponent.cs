using System.Diagnostics.CodeAnalysis;
using Gameplay.Monster.Abstract;
using UnityEngine.AI;
using Zenject;
using UnityEngine;
using Interfaces;

namespace Gameplay.Monster
{
    // wrapper component over navmesh agent
    [ExcludeFromCodeCoverage]
    internal class MonsterNavigationComponent : INavigationComponent
    {
        [Inject] private NavMeshAgent _navMeshAgent;

        private Vector2 TargetPosition { get; set; }
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

        public void ProcessStationMovement(IStation station)
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

        public void StopOnDeath()
        {
            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.ResetPath();
            }
        }
    }
}