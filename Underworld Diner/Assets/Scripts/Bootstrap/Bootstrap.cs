using System;
using System.Runtime.InteropServices;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private IAppInitializedCallback _appInitializedCallback;

        private void Start()
        {
            _appInitializedCallback.OnAppInitialized();
        }
    }
}