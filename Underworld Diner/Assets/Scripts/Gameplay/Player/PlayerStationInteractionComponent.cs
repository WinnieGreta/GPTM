using Gameplay.Player.Signals;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerStationInteractionComponent: IInitializable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private PlayerStatusComponent _status;
        
        private int PLAYER_HANDS = 2;
        
        public void Initialize()
        {
            _signalBus.Subscribe<DestinationReachedSignal>(OnDestinationReached);
        }
        
        private void OnDestinationReached()
        {
            if (_status.StationImMovingTo == null)
            {
                return;
            }
            
            switch (_status.StationImMovingTo)
            {
                case ITable table:
                    table.FreeTable();
                    // !!!!!!! Test !!!!!!!
                    _status.Hands.Clear();
                    break;
                case IKitchen kitchen:
                    ProcessKitchen(kitchen);
                    break;
                default:
                    Debug.Log("No station");
                    break;
            }
            _status.StationImMovingTo = null;
            Debug.Log(_status.ToString());
        }
        
        private void ProcessKitchen(IKitchen kitchen)
        {
            var dish = kitchen.PlayerInteraction(_status.Hands.Count < PLAYER_HANDS);
            if (dish != null)
            {
                Debug.Log($"Player got {dish.DishName} from kitchen");
                _status.Hands.Add(dish);
            }
        }

    }
}