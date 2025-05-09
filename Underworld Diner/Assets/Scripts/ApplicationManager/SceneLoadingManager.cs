using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ApplicationManager
{
    public enum SceneType
    { 
        MainMenu
    }

    public class SceneLoadingManager
    {
        public async UniTask LoadScene(string sceneType)
        {
            await SceneManager.LoadSceneAsync(sceneType);
        }
    }
    
}
