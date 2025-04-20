using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
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
