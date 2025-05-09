using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Table
{
    public class TableFacade : StationFacade, ITable
    {
        public bool IsTaken { get; private set; }
        private List<IChair> _myChairs;
        
        [Inject]
        private void OnInject(List<IChair> chairs)
        {
            _myChairs = chairs;
            Debug.Log("CHAIRS " + _myChairs.Count);
        }

        private void FreeTableChairs()
        {
            foreach (var chair in _myChairs)
            {
                chair.FreeChair();
            }
        }

        public void FreeTable()
        {
            FreeTableChairs();
            IsTaken = false;
        }
    }
}