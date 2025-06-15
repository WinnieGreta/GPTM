using Gameplay.GameManager;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.HUD
{
    public class DateDisplay : MonoBehaviour
    {
        [Inject] private LevelBasicSettings _basicSettings;

        [SerializeField] private TMP_Text _dateText;

        public void Awake()
        {
            _dateText.text = _basicSettings.LevelName;
        }
    }
}