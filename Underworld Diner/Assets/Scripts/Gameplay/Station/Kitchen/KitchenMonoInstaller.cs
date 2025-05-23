using System;
using Interfaces;
using UnityEngine;
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
        [SerializeField] private DishType _dishType;

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
            Container.BindInstance(_dishType).AsSingle();
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