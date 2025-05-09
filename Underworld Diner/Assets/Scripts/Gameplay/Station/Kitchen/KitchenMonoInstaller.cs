using System;
using Gameplay.Dish;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    public class KitchenMonoInstaller : MonoInstaller
    {
        [SerializeField] private StationAnchorParameters _stationAnchorParameters;
        [SerializeField] private KitchenParameters kitchenParameters;
        [SerializeField] private DishRecipe _dishRecipe;

        public override void InstallBindings()
        {
            Container.BindInstance(_stationAnchorParameters).AsSingle();
            Container.BindInstance(kitchenParameters).AsSingle();
            Container.BindInstance(_dishRecipe).AsSingle();
            Container.BindInterfacesAndSelfTo<StationFacade>()
                .FromComponentOnRoot()
                .AsSingle()
                .NonLazy();
            
        }
    }

    [Serializable]
    public class KitchenParameters
    {
        public SpriteRenderer DishPosterSprite;
        public Transform CookingAnimationAnchor;
        public SpriteRenderer ReadyDishSprite;
    }
}