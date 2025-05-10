using Gameplay.Dish;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    
    public class KitchenFacade : StationFacade, IKitchen, ITickable
    {
        [Inject] private KitchenParameters _kitchenParameters;
        [Inject] private DishRecipe _dishRecipe;
        [Inject] private KitchenStatusComponent _statusComponent;
        
        private float _cookingTimer;

        [Inject]
        private void OnInject()
        {
            _kitchenParameters.DishPosterSprite.sprite = _dishRecipe.MenuImage;
            _kitchenParameters.ReadyDishSprite.sprite = _dishRecipe.DishImage;
            _statusComponent.State = CookingState.Idle;
            _cookingTimer = 0;

        }

        public IDish PlayerInteraction(bool playerHasFreeHand)
        {
            StartCooking();
            if (playerHasFreeHand)
            {
                Debug.Log("Player has free hand");
                return GetDish();
            }

            return null;
        }

        private void StartCooking()
        {
            if (_statusComponent.State == CookingState.Idle)
            {
                //Debug.Log("Started cooking!");
                _statusComponent.State = CookingState.Cooking;
            }
        }

        private IDish GetDish()
        {
            if (_statusComponent.State == CookingState.Ready)
            {
                //Debug.Log("Dish is picked up!");
                _statusComponent.State = CookingState.Idle;
                return _dishRecipe;
            }
            return null;
        }

        public void Tick()
        {
            if (_statusComponent.State == CookingState.Cooking)
            {
                _cookingTimer += Time.deltaTime;
                if (_cookingTimer > _dishRecipe.CookingTime)
                {
                    Debug.Log($"{_dishRecipe.DishName} is cooked! Time: {_cookingTimer}");
                    _statusComponent.State = CookingState.Ready;
                    _cookingTimer = 0;
                }
            }
        }
        
    }
}