using System;
using Interfaces;
using Signals;
using Zenject;

namespace Gameplay.GameManager
{
    public class ScoringManager : IScoringManager
    {
        // TODO we might want to calculate score per full order on the scene level with control over all multipliers
        [Inject] private LevelBasicSettings _basicSettings;
        
        public float Score { get; private set; }
        public float GoalScore { get; private set; } = 100;

        public event Action ScoreUpdatedEvent;

        [Inject]
        private void OnInject()
        {
            GoalScore = _basicSettings.LevelScoreGoal;
        }
        
        internal void UpdateScore(OnMonsterScoredSignal monsterScore)
        {
            Score += monsterScore.Score;
            ScoreUpdatedEvent?.Invoke();
        }

        public bool HasReachedFinalScore()
        {
            return Score >= GoalScore;
        }
    }
}