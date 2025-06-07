using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Table
{
    public class TableFacade : StationFacade, ITable
    {
        [Inject] private StationAnchorParameters _parameters;
        public bool IsTaken { get; private set; }
        private List<IChair> _myChairs;
        
        private int PLAYER_HANDS = 3;
        
        [Inject]
        private void OnInject(List<IChair> chairs)
        {
            _myChairs = chairs;
            foreach (var c in chairs)
            {
                c.PlayerAnchors = _parameters.PlayerAnchors;
            }
            Debug.Log("CHAIRS " + _myChairs.Count);
        }

        public override LinkedList<DishType> PlayerStationInteraction(LinkedList<DishType> playerHands)
        {
            var newPlayerHands = TakeOrderFromHands(playerHands);
            
            // basically if something was taken from player's hands
            if (newPlayerHands.Count < playerHands.Count)
            {
                return newPlayerHands;
            }

            newPlayerHands = CleanDirtyPlates(playerHands);
            return newPlayerHands;
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

        private LinkedList<DishType> TakeOrderFromHands(LinkedList<DishType> playerHands)
        {
            var dishToGive = playerHands.First;
            while (dishToGive != null)
            {
                var nextDishToGive = dishToGive.Next;
                if (TryGivingDish(dishToGive.Value))
                {
                    playerHands.Remove(dishToGive.Value);
                }

                dishToGive = nextDishToGive;
            }

            return playerHands;
        }
        
        private bool TryGivingDish(DishType order)
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

        private LinkedList<DishType> CleanDirtyPlates(LinkedList<DishType> playerHands)
        {
            foreach (var chair in _myChairs)
            {
                if (playerHands.Count >= PLAYER_HANDS)
                {
                    return playerHands;
                }

                if (!chair.IsClean)
                {
                    chair.PutDish(DishType.None);
                    playerHands.AddLast(DishType.DirtyPlate);
                }
            }

            return playerHands;
        }
    }
}