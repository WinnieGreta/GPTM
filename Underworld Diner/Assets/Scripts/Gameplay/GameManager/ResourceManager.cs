using System;
using Interfaces;

namespace Gameplay.GameManager
{
    public class ResourceManager : IResourceManager
    {
        public int RedCount { get; private set; } = 101;
        public int GreenCount { get; private set; } = 102;
        public int BlueCount { get; private set; } = 103;
        
        public event Action ResourcesUpdatedEvent;
        
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

    }
}