﻿namespace Interfaces
{
    public interface IChair : IStation
    {
        bool IsTaken { get; }
        bool IsClean { get; }
        bool IsFacingRight { get; }

        IDish ExpectedDish { get; }

        void TakeChair(IMonster occupant);

        void FreeChair();

        void PutDish(IDish dish);
        
        // TODO can we do without?
        IDish GetDishImEating();
    }
}