using UnityEngine;
using Zenject;

namespace Gameplay.Station.Sink
{
    public class SinkMonoinstaller : MonoInstaller
    {
        [SerializeField] private StationAnchorParameters _anchorParameters;

        public override void InstallBindings()
        {
            Container.BindInstance(_anchorParameters).AsSingle();
            Container.BindInterfacesAndSelfTo<SinkFacade>()
                .FromComponentOnRoot()
                .AsSingle()
                .NonLazy();
        }
    }
}