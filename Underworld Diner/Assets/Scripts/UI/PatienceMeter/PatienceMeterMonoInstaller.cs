using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.PatienceMeter
{
    public class PatienceMeterMonoInstaller : MonoInstaller
    {
        [SerializeField] private RectTransform _containerTransform;
        [SerializeField] private Image _mask;

        public override void InstallBindings()
        {
            Container.BindInstance(_containerTransform).AsSingle();
            Container.BindInstance(_mask).AsSingle();
            Container.BindInterfacesAndSelfTo<PatienceMeterFacade>().FromComponentOnRoot().AsSingle();
        }
    }
}