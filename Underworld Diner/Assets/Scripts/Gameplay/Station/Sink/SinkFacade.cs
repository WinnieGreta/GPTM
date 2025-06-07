using System.Collections.Generic;
using Interfaces;

namespace Gameplay.Station.Sink
{
    public class SinkFacade : StationFacade, ISink
    {
        public override LinkedList<DishType> PlayerStationInteraction(LinkedList<DishType> playerHands)
        {
            var dishInHand = playerHands.First;
            while (dishInHand != null)
            { 
                var nextDishInHand = dishInHand.Next;
                if (dishInHand.Value == DishType.DirtyPlate)
                {
                    playerHands.Remove(dishInHand.Value);
                }
                dishInHand = nextDishInHand;
            }

            return playerHands;
        }
  
    }
}