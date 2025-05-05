using UnityEngine;

namespace Interfaces
{
    public interface IStation
    {
        public Vector2 GetClosestAnchorPosition(Vector2 agentPosition, string tag);
    }
}