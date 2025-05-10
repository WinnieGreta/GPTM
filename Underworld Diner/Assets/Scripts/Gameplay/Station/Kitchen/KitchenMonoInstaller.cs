using System;
using Gameplay.Dish;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Gameplay.Station.Kitchen
{
    public enum CookingState
    {
        Idle,
        Cooking,
        Ready
    }
    
    public class KitchenMonoInstaller : MonoInstaller
    {
        [SerializeField] private StationAnchorParameters _stationAnchorParameters;
        [SerializeField] private KitchenParameters _kitchenParameters;
        [SerializeField] private DishRecipe _dishRecipe;

        public override void InstallBindings()
        {
            Container.BindInstance(_stationAnchorParameters).AsSingle();
            Container.BindInstance(_kitchenParameters).AsSingle();
            Container.Bind<KitchenStatusComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<KitchenFacade>()
                .FromComponentOnRoot()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<KitchenAnimatorComponent>().AsSingle();
            
            // Test dish
            Container.BindInstance(_dishRecipe).AsSingle();
        }
    }

    [Serializable]
    public class KitchenParameters
    {
        public SpriteRenderer DishPosterSprite;
        public Animator CookingAnimator;
        public SpriteRenderer ReadyDishSprite;
    }
}