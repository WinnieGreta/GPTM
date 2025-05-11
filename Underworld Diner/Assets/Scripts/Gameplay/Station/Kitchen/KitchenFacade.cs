using Gameplay.Dish;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    
    public class KitchenFacade : StationFacade, IKitchen, ITickable
    {
        [Inject] private KitchenParameters _kitchenParameters;
        [Inject] private DishType _dishType;
        [Inject] private KitchenStatusComponent _statusComponent;
        [Inject] private IRecipeBook _recipeBook;

        
        private float _cookingTimer;

        [Inject]
        private void OnInject()
        {
            _kitchenParameters.DishPosterSprite.sprite = _recipeBook[_dishType].MenuImage;
            _kitchenParameters.ReadyDishSprite.sprite = _recipeBook[_dishType].DishImage;
            _statusComponent.State = CookingState.Idle;
            _cookingTimer = 0;

        }

        public DishType PlayerInteraction(bool playerHasFreeHand)
        {
            StartCooking();
            if (playerHasFreeHand)
            {
                Debug.Log("Player has free hand");
                return GetDish();
            }

            return DishType.None;
        }

        private void StartCooking()
        {
            if (_statusComponent.State == CookingState.Idle)
            {
                //Debug.Log("Started cooking!");
                _statusComponent.State = CookingState.Cooking;
            }
        }

        private DishType GetDish()
        {
            if (_statusComponent.State == CookingState.Ready)
            {
                //Debug.Log("Dish is picked up!");
                _statusComponent.State = CookingState.Idle;
                return _dishType;
            }
            return DishType.None;
        }

        public void Tick()
        {
            if (_statusComponent.State == CookingState.Cooking)
            {
                _cookingTimer += Time.deltaTime;
                if (_cookingTimer > _recipeBook[_dishType].CookingTime)
                {
                    Debug.Log($"{_dishType} is cooked! Time: {_cookingTimer}");
                    _statusComponent.State = CookingState.Ready;
                    _cookingTimer = 0;
                }
            }
        }
        
    }
}