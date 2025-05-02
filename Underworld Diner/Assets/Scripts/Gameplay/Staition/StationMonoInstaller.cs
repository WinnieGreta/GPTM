using System;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Gameplay.Staition
{
    public class StationMonoInstaller : MonoInstaller
    {
        [SerializeField] private StationAnchorParameters _anchorParameters;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Transform>().FromInstance(transform).AsSingle();
            Container.BindInstance(_anchorParameters).AsSingle();
            Container.BindInterfacesAndSelfTo<StationFacade>().FromNewComponentOnRoot().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StationAnchorsDetectionComponent>().AsSingle();
        }
    }
    
    [Serializable]
    public class StationAnchorParameters
    { 
        public List<Transform> Anchors;
    }
}