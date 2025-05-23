﻿using System;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Chair
{
    public class ChairMonoInstaller : MonoInstaller
    {
        [SerializeField] private StationAnchorParameters _anchorParameters;
        [SerializeField] private ChairParameters _chairParameters;
        [Inject] private IChairManager _chairManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Transform>().FromInstance(transform).AsSingle();
            Container.BindInstance(_anchorParameters).AsSingle();
            Container.BindInstance(_chairParameters).AsSingle();
            //Container.BindInterfacesAndSelfTo<DishRecipe>().FromInstance(_dirtyDish).AsSingle();
            Container.BindInterfacesAndSelfTo<ChairFacade>()
                .FromComponentOnRoot()
                .AsSingle()
                .OnInstantiated<ChairFacade>((_, x) => _chairManager.Register(x))
                .NonLazy();
            Container.BindInterfacesAndSelfTo<StationAnchorsDetectionComponent>().AsSingle();
        }
    }
    
    [Serializable]
    public class ChairParameters
    {
        public SpriteRenderer DishSprite;
    }
}
