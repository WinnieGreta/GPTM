using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapon
{
    public class DamageDealerMonoInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Transform>().FromInstance(transform).AsSingle();
            Container.BindInterfacesAndSelfTo<DamageDealerFacade>()
                .FromNewComponentOnRoot()
                .AsSingle()
                .NonLazy();
        }
    }
    
}