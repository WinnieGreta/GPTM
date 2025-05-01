using System;
using Interfaces;
using UnityEngine;
using Zenject;

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
        }
    }
}