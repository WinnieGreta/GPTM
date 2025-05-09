using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Chair
{
    public class ChairMonoInstaller : MonoInstaller
    {
        [SerializeField] private Station.StationAnchorParameters _anchorParameters;
        [Inject] private IChairManager _chairManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Transform>().FromInstance(transform).AsSingle();
            Container.BindInstance(_anchorParameters).AsSingle();
            Container.BindInterfacesAndSelfTo<ChairFacade>()
                .FromComponentOnRoot()
                .AsSingle()
                .OnInstantiated<ChairFacade>((_, x) => _chairManager.Register(x))
                .NonLazy();
            Container.BindInterfacesAndSelfTo<StationAnchorsDetectionComponent>().AsSingle();
        }
    }
    
    [Serializable]
    public class StationAnchorParameters
    { 
        public List<Transform> PlayerAnchors;
        public List<Transform> MonsterAnchors;
    }
}
