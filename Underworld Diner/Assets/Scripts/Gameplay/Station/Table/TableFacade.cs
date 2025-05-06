using Interfaces;

namespace Gameplay.Station.Table
{
    public class TableFacade : StationFacade, ITable
    {
        public bool IsTaken { get; private set; }
        
        
    }
}