using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.LevelStatisticsDisplay
{
    public class StatisticsTitleDisplay : MonoBehaviour
    {
        [Inject] private IScoringManager _scoringManager;

        [SerializeField] private TMP_Text _winLoseText;

        public void OnEnable()
        {
            _winLoseText.text = WinLoseMessage();
        }

        private string WinLoseMessage()
        {
            if (_scoringManager.HasReachedFinalScore())
            {
                return "Victory!";
            }

            return "Defeat...";
        }
    }
}