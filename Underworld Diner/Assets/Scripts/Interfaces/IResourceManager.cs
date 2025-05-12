using System;

namespace Interfaces
{
    public enum ResourceType
    {
        Red,
        Green,
        Blue
    }
    
    public interface IResourceManager
    {
        int RedCount { get; }
        int GreenCount { get; }
        int BlueCount { get; }

        bool TrySpendResources(int red, int green, int blue);

        event Action ResourcesUpdatedEvent;
    }
}