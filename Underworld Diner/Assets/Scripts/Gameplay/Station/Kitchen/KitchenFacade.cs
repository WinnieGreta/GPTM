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
        [Inject] private KitchenState _currentState;
        
        private float _cookingTimer;

        [Inject]
        private void OnInject()
        {
            _kitchenParameters.DishPosterSprite.sprite = _dishRecipe.MenuImage;
            _kitchenParameters.ReadyDishSprite.sprite = _dishRecipe.DishImage;
            _currentState.State = CookingState.Idle;
            _cookingTimer = 0;

        }

        public IDish PlayerInteraction()
        {
            StartCooking();
            return GetDish();
        }

        private void StartCooking()
        {
            if (_currentState.State == CookingState.Idle)
            {
                Debug.Log("Started cooking!");
                _currentState.State = CookingState.Cooking;
            }
        }

        private IDish GetDish()
        {
            if (_currentState.State == CookingState.Ready)
            {
                Debug.Log("Dish is picked up!");
                _currentState.State = CookingState.Idle;
                return _dishRecipe;
            }
            return null;
        }

        public void Tick()
        {
            if (_currentState.State == CookingState.Cooking)
            {
                _cookingTimer += Time.deltaTime;
                if (_cookingTimer > _dishRecipe.CookingTime)
                {
                    Debug.Log("Dish is cooked! Time: " + _cookingTimer);
                    _currentState.State = CookingState.Ready;
                    _cookingTimer = 0;
                }
            }
        }
        
        
    }
}