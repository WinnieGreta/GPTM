using System;
using Interfaces;
using Zenject;

namespace Gameplay.GameManager
{
    public class ResourceManager : IResourceManager
    {
        [Inject] private LevelResourceSettings _levelResourceSettings;
        
        public int RedCount { get; private set; } = 101;
        public int GreenCount { get; private set; } = 102;
        public int BlueCount { get; private set; } = 103;
        
        public event Action ResourcesUpdatedEvent;

        public void InitializeResources()
        {
            RedCount = _levelResourceSettings.StartingRed;
            GreenCount = _levelResourceSettings.StartingGreen;
            BlueCount = _levelResourceSettings.StartingBlue;
            ResourcesUpdatedEvent?.Invoke();
        }
        
        public bool TrySpendResources(int red, int green, int blue)
        {
            if (RedCount > red && GreenCount > green && BlueCount > blue)
            {
                RedCount -= red;
                GreenCount -= green;
                BlueCount -= blue;
                ResourcesUpdatedEvent?.Invoke();
                return true;
            }

            return false;
        }

        public void GainResources(int red, int green, int blue)
        {
            RedCount += red;
            GreenCount += green;
            BlueCount += blue;
            ResourcesUpdatedEvent?.Invoke();
        }

    }
}