namespace Interfaces
{
    public interface ITable
    {
        bool IsTaken { get; }
        void FreeTable();

        bool TryGivingDish(DishType dish);
        int CleanDirtyPlates(int freeHands);
    }
}