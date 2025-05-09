using Zenject;

namespace ApplicationManager
{
    public class ApplicationManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ApplicationManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoadingManager>().AsSingle();
        }
    }
}