namespace Interfaces
{
    public interface ITable
    {
        bool IsTaken { get; }
        void FreeTable();

        bool TryGivingDish(IDish dish);
        int TryCleaningTable(int freeHands);
    }
}