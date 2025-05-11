namespace Interfaces
{
    public interface IChair : IStation
    {
        bool IsTaken { get; }
        bool IsClean { get; }
        bool IsFacingRight { get; }

        IDish ExpectedDish { get; }

        void TakeChair(IMonster occupant);

        void FreeChair();

        void OrderDish(IDish dish);

        void PutDish(IDish dish);
        
        // !!!!!! TESTING REMOVE AFTER !!!!!!??
        IDish GetDishImEating();
    }
}