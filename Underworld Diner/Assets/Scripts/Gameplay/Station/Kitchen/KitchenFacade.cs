using Gameplay.Dish;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    public class KitchenFacade : StationFacade, IKitchen
    {
        [Inject] private KitchenParameters _kitchenParameters;
        [Inject] private DishRecipe _dishRecipe;

        [Inject]
        private void OnInject()
        {
            _kitchenParameters.DishPosterSprite.sprite = _dishRecipe.MenuImage;
            _kitchenParameters.ReadyDishSprite.sprite = _dishRecipe.DishImage;

        }
    }
}