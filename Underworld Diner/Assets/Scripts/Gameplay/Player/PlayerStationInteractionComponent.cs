using System.Collections.Generic;
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
        [Inject] private PlayerAnimatorComponent _animatorComponent;
        [Inject] private IRecipeBook _recipeBook;
        
        private PlayerHandlingParameters.AnchorGroup _anchorGroup;
        //private int PLAYER_HANDS = 3;
        
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

            
            var newHands = _status.StationImMovingTo.PlayerStationInteraction(_status.Hands);
            
            //_status.StationImMovingTo.PlayerStationInteraction(_status.Hands);
            //_animatorComponent.PutDown();
            if (newHands.Count <= _status.Hands.Count)
            {
                //Debug.Log($"{newHands.Count} <= {_status.Hands.Count}");
                _animatorComponent.PutDown();
            }
            else
            {
                //Debug.Log($"{newHands.Count} > {_status.Hands.Count}");
                _animatorComponent.PickUp();
            }

            _status.Hands = newHands;
            _status.StationImMovingTo = null;
            RenderDishes();
            //Debug.Log(_status.ToString());
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
            var statusHands = _status.Hands.ToList();

            for (int i = 0; i < anchorGroup.Sprites.Count && i < statusHands.Count; i++)
            {
                anchorGroup.Sprites[i].sprite = _recipeBook[statusHands[i]].DishImage;
            }
        }
        
    }
}