using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.LevelStatisticsDisplay
{
    public class StatisticsFinalScoreDisplay : MonoBehaviour
    {
        [Inject] private IScoringManager _scoringManager;
        
        [SerializeField] private TMP_Text _text;
        
        private void Reset()
        {
            _text = GetComponent<TMP_Text>();
        }
        
        private void Awake()
        {
            _scoringManager.ScoreUpdatedEvent += OnScoreUpdated;
        }

        private void Start()
        {
            OnScoreUpdated();
        }

        private void OnScoreUpdated()
        {
            _text.text = _scoringManager.Score.ToString();
        }
    }
}