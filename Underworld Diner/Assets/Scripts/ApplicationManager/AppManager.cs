using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;

namespace ApplicationManager
{
    
    public class AppManager: IApplicationManager, IAppInitializedCallback
    {
        [Inject] private SceneLoadingManager _sceneLoadingManager;

        public void OnAppInitialized()
        {
            LoadMainMenu();
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

        public void OnAppExit()
        {
            Application.Quit();
        }
    }
}