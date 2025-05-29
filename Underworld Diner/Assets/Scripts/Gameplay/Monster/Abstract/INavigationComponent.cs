using Interfaces;
using UnityEngine;

namespace Gameplay.Monster.Abstract
{
    internal interface INavigationComponent
    {
        void ProcessMovement(Vector2 targetPosition);
        void ProcessStationMovement(IStation station);
        bool HasReachedDestination();
        void StopOnDeath();
    }
}