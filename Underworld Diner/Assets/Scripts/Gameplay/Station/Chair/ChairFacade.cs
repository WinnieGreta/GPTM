using Interfaces;

namespace Gameplay.Station.Chair
{
    public class ChairFacade : StationFacade, IChair
    {
        public bool IsTaken { get; set; }
        
    }
}