using System;

namespace Interfaces
{
    
    public interface IResourceManager
    {
        int RedCount { get; }
        int GreenCount { get; }
        int BlueCount { get; }

        void InitializeResources();
        bool TrySpendResources(int red, int green, int blue);
        void GainResources(int red, int green, int blue);

        event Action ResourcesUpdatedEvent;
    }
}