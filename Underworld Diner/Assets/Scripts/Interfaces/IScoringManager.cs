using System;

namespace Interfaces
{
    public interface IScoringManager
    {
        float Score { get; }
        float GoalScore { get; }

        bool HasReachedFinalScore();
        event Action ScoreUpdatedEvent;
    }
}