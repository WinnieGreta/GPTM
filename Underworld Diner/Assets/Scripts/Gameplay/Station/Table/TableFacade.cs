using System.Collections.Generic;
using System.Linq;
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
            //Debug.Log("CHAIRS " + _myChairs.Count);
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

        public bool TryGivingDish(DishType order)
        {
            for (int i = 0; i < _myChairs.Count; i++)
            {
                if (_myChairs[i].ExpectedDish == order)
                {
                    _myChairs[i].PutDish(order);
                    return true;
                }
            }
            return false;
        }

        public int TryCleaningTable(int freeHands)
        {
            int freeHandsLeft = freeHands;
            for (int i = 0; i < _myChairs.Count && freeHandsLeft > 0; i++)
            {
                if (!_myChairs[i].IsClean)
                {
                    freeHandsLeft--;
                    _myChairs[i].PutDish(DishType.None);
                }
            }
            return freeHandsLeft;
        }
    }
}