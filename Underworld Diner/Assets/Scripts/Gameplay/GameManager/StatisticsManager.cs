using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Gameplay.GameManager
{
    internal class StatisticsManager : IStatisticsManager
    {
        private Dictionary<string, int> _statistics = new();

        public event Action StatisticsUpdatedEvent;

        public void IncrementStatistics(string id)
        {
            _statistics.TryAdd(id, 0);
            _statistics[id] = _statistics[id] + 1; 
            StatisticsUpdatedEvent?.Invoke();
            Debug.Log($"Incremented statistics {id}, value: {_statistics[id]}");
        }

        public int GetStatistics(string id)
        {
            int stat = 0;
            _statistics.TryGetValue(id, out stat);
            return stat;
        }
    }
}