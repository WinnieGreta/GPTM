using System;
using Interfaces;
using Signals;
using Zenject;

namespace Gameplay.GameManager
{
    public class ScoringManager : IScoringManager
    {
        // TODO we might want to calculate score per full order on the scene level with control over all multipliers
        
        public float Score { get; private set; } = 99;
        
        public event Action ScoreUpdatedEvent;
        

        internal void UpdateScore(OnMonsterScoredSignal monsterScore)
        {
            Score += monsterScore.Score;
            ScoreUpdatedEvent?.Invoke();
        }
    }
}