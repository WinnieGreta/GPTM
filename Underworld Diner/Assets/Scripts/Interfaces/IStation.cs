using UnityEngine;
using UnityEngine.AI;

namespace Interfaces
{
    public interface IStation
    {
        public Vector2 GetClosestAnchorPosition(NavMeshAgent agent);
    }
}