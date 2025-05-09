namespace Interfaces
{
    public interface IChair : IStation
    {
        bool IsTaken { get; }
        bool IsFacingRight { get; }

        void TakeChair(IMonster occupant);

        void FreeChair();
    }
}