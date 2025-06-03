using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.HUD
{
    public class ResourceDisplay : MonoBehaviour
    {

        [Inject] private IResourceManager _resourceManager;

        [SerializeField] private TMP_Text _redText;
        [SerializeField] private TMP_Text _greenText;
        [SerializeField] private TMP_Text _blueText;

        public void OnEnable()
        {
            _resourceManager.ResourcesUpdatedEvent += UpdateResourcesDisplay;
            UpdateResourcesDisplay();
        }

        public void OnDisable()
        {
            _resourceManager.ResourcesUpdatedEvent -= UpdateResourcesDisplay;
        }

        private void UpdateResourcesDisplay()
        {
            _redText.text = _resourceManager.RedCount.ToString();
            _greenText.text = _resourceManager.GreenCount.ToString();
            _blueText.text = _resourceManager.BlueCount.ToString();
        }
    }
}