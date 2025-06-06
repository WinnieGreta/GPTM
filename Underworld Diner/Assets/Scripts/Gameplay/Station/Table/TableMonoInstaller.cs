using Gameplay.Station.Chair;
using UnityEngine;
using Zenject;

namespace Gameplay.Station.Table
{
    public class TableMonoInstaller : MonoInstaller
    {
        [SerializeField] private StationAnchorParameters _anchorParameters;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Transform>().FromInstance(transform).AsSingle();
            Container.BindInstance(_anchorParameters).AsSingle();
            Container.BindInterfacesAndSelfTo<TableFacade>()
                .FromComponentOnRoot()
                .AsSingle()
                .NonLazy();
            Container.BindInterfacesAndSelfTo<ChairFacade>().FromComponentsInChildren().AsTransient();
        }
    }
    
}