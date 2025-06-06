using Analytics.Signals;
using Interfaces;
using Unity.Services.Analytics;
using UnityEngine;

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
            _serviceInstance?.RecordEvent("levelStarted");
        }

        internal void OnLevelEnd(AnalyticsLevelEndEvent analyticsLevelEndEvent)
        {
            CustomEvent levelEnd = new CustomEvent("levelEnded")
            {
                { "levelFinalScore", analyticsLevelEndEvent.Score }
            };
            _serviceInstance?.RecordEvent(levelEnd);
        }
    }
}