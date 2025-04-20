using Zenject;

namespace GameManager
{
    public class GameManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoadingManager>().AsSingle();
        }
    }
}