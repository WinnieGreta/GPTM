using Gameplay.Dish;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    public enum KitchenState
    {
        Idle,
        Cooking,
        Ready
    }
    
    public class KitchenFacade : StationFacade, IKitchen, ITickable
    {
        [Inject] private KitchenParameters _kitchenParameters;
        [Inject] private DishRecipe _dishRecipe;
        private KitchenState _currentState;
        private float _cookingTimer;

        [Inject]
        private void OnInject()
        {
            _kitchenParameters.DishPosterSprite.sprite = _dishRecipe.MenuImage;
            _kitchenParameters.ReadyDishSprite.sprite = _dishRecipe.DishImage;
            _currentState = KitchenState.Idle;
            _cookingTimer = 0;

        }

        public IDish PlayerInteraction()
        {
            StartCooking();
            return GetDish();
        }

        private void StartCooking()
        {
            if (_currentState == KitchenState.Idle)
            {
                Debug.Log("Started cooking!");
                _currentState = KitchenState.Cooking;
            }
        }

        private IDish GetDish()
        {
            if (_currentState == KitchenState.Ready)
            {
                Debug.Log("Dish is picked up!");
                _currentState = KitchenState.Idle;
                return _dishRecipe;
            }
            return null;
        }

        public void Tick()
        {
            if (_currentState == KitchenState.Cooking)
            {
                _cookingTimer += Time.deltaTime;
                if (_cookingTimer > _dishRecipe.CookingTime)
                {
                    Debug.Log("Dish is cooked! Time: " + _cookingTimer);
                    _currentState = KitchenState.Ready;
                    _cookingTimer = 0;
                }
            }
        }
        
        
    }
}