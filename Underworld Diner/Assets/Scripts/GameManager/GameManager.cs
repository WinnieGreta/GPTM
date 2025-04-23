using Cysharp.Threading.Tasks;
using Interfaces;
using Zenject;

namespace GameManager
{
    internal enum GameState
    {
        Bootstrap,
        MainMenu,
        GamePlay
    }
    
    public class GameManager: IGameManager, IAppInitializedCallback
    {
        [Inject] private SceneLoadingManager _sceneLoadingManager;

        public void OnAppInitialized()
        {
            _sceneLoadingManager.LoadScene("MainMenu").Forget();
        }

        public void LoadGameScene()
        {
            _sceneLoadingManager.LoadScene("GameplayScene").Forget();
        }
    }
}