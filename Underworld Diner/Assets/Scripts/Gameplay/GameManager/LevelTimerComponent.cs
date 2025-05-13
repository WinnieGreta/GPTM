using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager.LevelTimerManager
{
    public class LevelTimerComponent : ITickable
    {
        [Inject] private IApplicationManager _applicationManager;

        public float timerTime = 70;
        
        public void Tick()
        {
            if (timerTime > 0)
            {
                timerTime -= Time.deltaTime;
            }
            else
            {
                _applicationManager.LoadMainMenu();
            }
        }
    }
}