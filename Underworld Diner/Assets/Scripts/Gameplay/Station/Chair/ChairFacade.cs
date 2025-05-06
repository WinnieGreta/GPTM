using Interfaces;

namespace Gameplay.Station.Chair
{
    public class ChairFacade : StationFacade, IChair
    {
        public bool IsTaken => _occupant != null;

        private IMonster _occupant;

        public void TakeChair(IMonster occupant)
        {
            _occupant = occupant;
        }

        public void FreeChair()
        {
            _occupant = null;
        }
        

    }
}