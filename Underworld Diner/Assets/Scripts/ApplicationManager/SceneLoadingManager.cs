using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ApplicationManager
{

    public class SceneLoadingManager
    {
        public async UniTask LoadScene(string sceneType)
        {
            await SceneManager.LoadSceneAsync(sceneType);
        }
    }
    
}
