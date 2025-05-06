namespace Interfaces
{
    public interface IChair : IStation
    {
        bool IsTaken { get; }

        void TakeChair(IMonster occupant);

        void FreeChair();
    }
}