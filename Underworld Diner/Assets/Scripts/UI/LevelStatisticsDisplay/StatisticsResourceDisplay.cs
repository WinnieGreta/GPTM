using Gameplay.GameManager;
using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.LevelStatisticsDisplay
{
    internal enum ResourceType
    {
        Red,
        Green,
        Blue
    }
    
    public class StatisticsResourceDisplay: MonoBehaviour
    {
        [Inject] private IResourceManager _resourceManager;
        [Inject] private LevelResourceSettings _resourceSettings;
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private ResourceType _resourceType;
        
        private void Reset()
        {
            _text = GetComponent<TMP_Text>();
        }
        
        private void Awake()
        {
            _resourceManager.ResourcesUpdatedEvent += OnResourcesUpdated;
        }
        
        private void Start()
        {
            OnResourcesUpdated();
        }

        private void OnResourcesUpdated()
        {
            switch (_resourceType)
            {
                case ResourceType.Red:
                    _text.text = (_resourceSettings.StartingRed - _resourceManager.RedCount).ToString();
                    break;
                case ResourceType.Green:
                    _text.text = (_resourceSettings.StartingGreen - _resourceManager.GreenCount).ToString();
                    break;
                case ResourceType.Blue:
                    _text.text = (_resourceSettings.StartingBlue - _resourceManager.BlueCount).ToString();
                    break;
                default:
                    Debug.LogError("No resource type");
                    break;
            }
            
        }
    }
}