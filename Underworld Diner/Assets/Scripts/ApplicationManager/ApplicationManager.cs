using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;

namespace ApplicationManager
{
    internal enum GameState
    {
        Bootstrap,
        MainMenu,
        GamePlay
    }
    
    public class ApplicationManager: IApplicationManager, IAppInitializedCallback
    {
        [Inject] private SceneLoadingManager _sceneLoadingManager;

        public void OnAppInitialized()
        {
            _sceneLoadingManager.LoadScene("MainMenu").Forget();
        }

        public void LoadGameScene(string name)
        {
            Time.timeScale = 1f;
            _sceneLoadingManager.LoadScene(name).Forget();
        }

        public void LoadMainMenu()
        {
            _sceneLoadingManager.LoadScene("MainMenu").Forget();
        }
    }
}