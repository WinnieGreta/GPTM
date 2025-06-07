using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.HUD
{
    public class ScoreDisplay : MonoBehaviour
    {
        [Inject] private IScoringManager _scoringManager;
        
        [SerializeField] private TMP_Text _scoreText;
        
        public void OnEnable()
        {
            _scoringManager.ScoreUpdatedEvent += UpdateScoreDisplay;
            UpdateScoreDisplay();
        }

        public void OnDisable()
        {
            _scoringManager.ScoreUpdatedEvent -= UpdateScoreDisplay;
        }

        private void UpdateScoreDisplay()
        {
            int score = (int)_scoringManager.Score;
            _scoreText.text = score.ToString();
        }

    }
}