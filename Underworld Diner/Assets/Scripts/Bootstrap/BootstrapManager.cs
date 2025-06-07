using Cysharp.Threading.Tasks;
using Interfaces;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace Bootstrap
{
    public class BootstrapManager : MonoBehaviour
    {
        [Inject] private IAppInitializedCallback[] _appInitializedCallback;

        private void Start()
        {
            InitializeApplication().Forget();
        }

        private async UniTaskVoid InitializeApplication()
        {
            await  UnityServices.InitializeAsync();
            foreach (var callback in _appInitializedCallback)
            {
                callback.OnAppInitialized();
            }
        }
    }
}