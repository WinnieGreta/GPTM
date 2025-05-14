using Analytics.Signals;
using Interfaces;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace Analytics
{
    public class AppAnalytiscService : IAppInitializedCallback
    {
        private IAnalyticsService _serviceInstance;
        
        public void OnAppInitialized()
        {
            _serviceInstance = AnalyticsService.Instance;
            _serviceInstance.StartDataCollection();
            Debug.Log("Analytics service initialized");
        }

        internal void OnLevelStart(AnalyticsLevelStartEvent analyticsLevelStartInfo)
        {
            Debug.Log("Analytics levelStarted event sent");
            _serviceInstance.RecordEvent("levelStarted");
        }
    }
}