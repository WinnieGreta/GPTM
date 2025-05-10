﻿using System.Collections.Generic;
using System.Linq;
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
                RenderDishes();
            }
        }

        private void ProcessTable(ITable table)
        {
            // !!!!!!! Test !!!!!!!
            table.FreeTable();
            _status.Hands.Clear();
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

            for (int i = 0; i < anchorGroup.Sprites.Count && i < _status.Hands.Count; i++)
            {
                anchorGroup.Sprites[i].sprite = _status.Hands[i].DishImage;
            }
        }
        
    }
}