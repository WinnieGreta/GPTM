using Gameplay.GameManager;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.LevelStartPopupDisplay
{
    public class StartScoreGoalDisplay : MonoBehaviour
    {
        [Inject] private LevelBasicSettings _levelSettings;

        [SerializeField] private TMP_Text _levelNameText;
        [SerializeField] private TMP_Text _goalText;

        public void OnEnable()
        {
            _levelNameText.text = _levelNameText.text + " " + _levelSettings.LevelName;
            _goalText.text = _goalText.text + " " + _levelSettings.LevelScoreGoal;
        }
    }
}