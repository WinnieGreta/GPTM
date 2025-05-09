using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Order
{
    public class OrderIconMonoInstaller : MonoInstaller
    {
        [SerializeField] private Transform _containerImageTransform;
        [SerializeField] private Image _dishImage;

        public override void InstallBindings()
        {
            Container.BindInstance(_containerImageTransform).AsSingle();
            Container.BindInstance(_dishImage).AsSingle();
            Container.BindInterfacesAndSelfTo<OrderIconFacade>().FromComponentOnRoot().AsSingle();

        }
    }
}