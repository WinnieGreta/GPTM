using System.Collections.Generic;
using System.Linq;
using Gameplay.Dish;
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
        [Inject] private PlayerHandlingParameters _playerHandlingParameters;
        [Inject] private PlayerAnimatorComponent _animatorComponent;
        [Inject] private IRecipeBook _recipeBook;
        
        private PlayerHandlingParameters.AnchorGroup _anchorGroup;
        private int PLAYER_HANDS = 3;
        
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
                    ProcessTable(table);
                    break;
                case IKitchen kitchen:
                    ProcessKitchen(kitchen);
                    break;
                case ISink sink:
                    ProcessSink(sink);
                    break;
                default:
                    Debug.Log("No station");
                    break;
            }
            _status.StationImMovingTo = null;
            //Debug.Log(_status.ToString());
        }
        
        private void ProcessKitchen(IKitchen kitchen)
        {
            var dish = kitchen.PlayerInteraction(_status.Hands.Count < PLAYER_HANDS);
            if (dish != DishType.None)
            {
                //Debug.Log($"Player got {dish} from kitchen");
                _animatorComponent.PickUp();
                _status.Hands.AddLast(dish);
                RenderDishes();
            }
        }

        private void ProcessTable(ITable table)
        {
            if (TryGiveOrder(table))
            {
                _animatorComponent.PutDown();
            }
            else if (TryCleanTable(table))
            {
                _animatorComponent.PickUp();
            }
            RenderDishes();
        }

        private void ProcessSink(ISink sink)
        {
            var dishInHand = _status.Hands.First;
            while (dishInHand != null)
            { 
                var nextDishInHand = dishInHand.Next;
                if (dishInHand.Value == DishType.DirtyPlate)
                {
                    _status.Hands.Remove(dishInHand.Value);
                }
                dishInHand = nextDishInHand;
            }
            sink.WashDirtyPlates();
            RenderDishes();
        }

        private void RenderDishes()
        {
            foreach (var pair in _playerHandlingParameters.AnchorGroups)
            {
                var isCurrent = _status.Hands.Count == pair.Key;
                pair.Value.GroupParent.SetActive(isCurrent);
                if (isCurrent)
                {
                    SetSpritesForGroup(pair.Value);
                }
            }
        }

        private void SetSpritesForGroup(PlayerHandlingParameters.AnchorGroup anchorGroup)
        {
            /* var spritesAndRenderers =
                anchorGroup.Sprites.Zip(_status.Hands, (sr, d) => new { Renderer = sr, Dish = d });
            foreach (var pair in spritesAndRenderers)
            {
                pair.Renderer.sprite = pair.Dish.DishImage;
            } */

            var statusHands = _status.Hands.ToList();

            for (int i = 0; i < anchorGroup.Sprites.Count && i < statusHands.Count; i++)
            {
                anchorGroup.Sprites[i].sprite = _recipeBook[statusHands[i]].DishImage;
            }
        }

        private bool TryGiveOrder(ITable table)
        {
            bool success = false;
            var dishInHand = _status.Hands.First;
            while (dishInHand != null)
            { 
                var nextDishInHand = dishInHand.Next;
                if (table.TryGivingDish(dishInHand.Value))
                {
                    _status.Hands.Remove(dishInHand.Value);
                    success = true;
                }
                dishInHand = nextDishInHand;
            }

            return success;
        }

        private bool TryCleanTable(ITable table)
        {
            int freeHands = PLAYER_HANDS - _status.Hands.Count;
            if (freeHands > 0)
            {
                int dirtyDishesTaken = freeHands - table.TryCleaningTable(freeHands);
                if (dirtyDishesTaken == 0)
                {
                    return false;
                }
                for (int i = 0; i < dirtyDishesTaken; i++)
                {
                    _status.Hands.AddLast(DishType.DirtyPlate);
                }
                return true;
            }
            return false;
        }
        
    }
}