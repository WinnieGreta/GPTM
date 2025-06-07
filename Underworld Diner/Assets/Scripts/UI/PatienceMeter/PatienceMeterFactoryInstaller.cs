using Interfaces.UI;
using UnityEngine;
using Zenject;

namespace UI.PatienceMeter
{
    public class PatienceMeterFactoryInstaller : MonoInstaller
    {
        [SerializeField] private PatienceMeterFacade _patienceMeterFacade;
        [SerializeField] private Transform _meterRoot;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<PatienceMeterFacade, PatienceMeterPool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_patienceMeterFacade)
                .UnderTransform(_ => _meterRoot);
            Container.BindFactory<Transform, int, IPatienceMeter, IPatienceMeter.Factory>()
                .FromFactory<PatienceMeterFactory>();
        }

        internal class PatienceMeterFactory : IFactory<Transform, int, IPatienceMeter>
        {
            [Inject] private PatienceMeterPool _pool;

            public IPatienceMeter Create(Transform anchor, int hearts)
            {
                return _pool.Spawn(anchor, hearts);
            }
        }

        internal class PatienceMeterPool : MonoPoolableMemoryPool<Transform, int, PatienceMeterFacade>
        {
            
        }

    }
}