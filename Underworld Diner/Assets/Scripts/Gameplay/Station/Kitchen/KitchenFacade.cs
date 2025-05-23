using System;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    
    public class KitchenFacade : StationFacade, IKitchen, ITickable
    {
        private const string DISH_STATISTICS_ID_TEMPLATE = "dish_prepared_{0}";
        
        [Inject] private IResourceManager _resourceManager;
        [Inject] private KitchenParameters _kitchenParameters;
        [Inject] private DishType _dishType;
        [Inject] private KitchenStatusComponent _statusComponent;
        [Inject] private IRecipeBook _recipeBook;
        [Inject] private IStatisticsManager _statisticsManager;

        
        private float _cookingTimer;
        private IDish _dish;
        private string _dishStatisticsId;

        [Inject]
        private void OnInject()
        {
            _dish = _recipeBook[_dishType];
            _kitchenParameters.DishPosterSprite.sprite = _dish.MenuImage;
            _kitchenParameters.ReadyDishSprite.sprite = _dish.DishImage;
            _statusComponent.State = CookingState.Idle;
            _cookingTimer = 0;

            _dishStatisticsId = String.Format(DISH_STATISTICS_ID_TEMPLATE, _dishType.ToString());
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
                if (_resourceManager.TrySpendResources(_dish.RedCost, _dish.GreenCost,
                        _dish.BlueCost))
                {
                    _statusComponent.State = CookingState.Cooking;
                }
                else
                {
                    Debug.Log($"Not enough resources for {_dishType}");
                }
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
            if (_statusComponent.State != CookingState.Cooking)
            {
                return;
            }
            _cookingTimer += Time.deltaTime;
            if (_cookingTimer > _dish.CookingTime)
            {
                Debug.Log($"{_dishType} is cooked! Time: {_cookingTimer}");
                _statusComponent.State = CookingState.Ready;
                _cookingTimer = 0;
                
                _statisticsManager.IncrementStatistics(_dishStatisticsId);
            }
        }
        
    }
}