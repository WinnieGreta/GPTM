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

        
        // a crutch to stop monsters mosh pit around exit
        public bool HasReachedDestination(float offset = 0.01f)
        {
            if (!_navMeshAgent.pathPending)
            {
                if (Vector2.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) < offset)
                {
                    return true;
                }
                /*if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }*/
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