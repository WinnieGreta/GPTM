using UnityEngine;
using Zenject;

namespace Gameplay.Staition
{
    public class StationMonoInstaller : MonoInstaller
    {
        [SerializeField] private Transform _stationTransform;
        [SerializeField] private Transform _playerTransform;

        public override void InstallBindings()
        {
            Container.BindInstance(_stationTransform).WithId("station").AsCached();
            Container.BindInstance(_playerTransform).WithId("player").AsCached();
            Container.BindInterfacesAndSelfTo<StationDetectionComponent>().AsSingle();
        }
    }
}