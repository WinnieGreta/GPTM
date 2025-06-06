using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface IChair : IStation
    {
        bool IsTaken { get; }
        bool IsClean { get; }
        bool IsFacingRight { get; }

        DishType ExpectedDish { get; }

        List<Transform> PlayerAnchors { get; set; }

        void TakeChair(IMonster occupant);

        void FreeChair();

        void PutDish(DishType dish);
    }
}