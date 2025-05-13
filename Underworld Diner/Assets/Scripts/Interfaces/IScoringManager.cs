using System;

namespace Interfaces
{
    public interface IScoringManager
    {
        float Score { get; }

        event Action ScoreUpdatedEvent;
    }
}