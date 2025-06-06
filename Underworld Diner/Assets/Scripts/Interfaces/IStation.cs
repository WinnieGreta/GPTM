using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Interfaces
{
    public interface IStation
    {
        Vector2 GetClosestAnchorPosition(NavMeshAgent agent);
        LinkedList<DishType> PlayerStationInteraction(LinkedList<DishType> playerHands);
    }
}