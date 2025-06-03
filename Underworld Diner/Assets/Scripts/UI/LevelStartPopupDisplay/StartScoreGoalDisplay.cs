using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.LevelStartPopupDisplay
{
    public class StartScoreGoalDisplay : MonoBehaviour
    {
        [Inject] private IScoringManager _scoringManager;

        [SerializeField] private TMP_Text _goalText;

        public void OnEnable()
        {
            _goalText.text = _goalText.text + " " + _scoringManager.GoalScore;
        }
    }
}