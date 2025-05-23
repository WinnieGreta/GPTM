﻿using Cysharp.Threading.Tasks;
using Interfaces;
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
            _sceneLoadingManager.LoadScene(name).Forget();
        }

        public void LoadMainMenu()
        {
            _sceneLoadingManager.LoadScene("MainMenu").Forget();
        }
    }
}