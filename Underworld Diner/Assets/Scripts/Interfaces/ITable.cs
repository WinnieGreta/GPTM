namespace Interfaces
{
    public interface ITable
    {
        bool IsTaken { get; }
        void FreeTable();
    }
}