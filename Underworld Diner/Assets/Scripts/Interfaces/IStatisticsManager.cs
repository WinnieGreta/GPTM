using System;

namespace Interfaces
{
    public interface IStatisticsManager
    {
        public event Action StatisticsUpdatedEvent; 
        
        public void IncrementStatistics(string id);

        public int GetStatistics(string id);
    }
}