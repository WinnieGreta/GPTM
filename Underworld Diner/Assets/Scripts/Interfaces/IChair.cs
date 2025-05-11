namespace Interfaces
{
    public interface IChair : IStation
    {
        bool IsTaken { get; }
        bool IsClean { get; }
        bool IsFacingRight { get; }

        DishType ExpectedDish { get; }

        void TakeChair(IMonster occupant);

        void FreeChair();

        void PutDish(DishType dish);
    }
}