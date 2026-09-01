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
                    _text.text = (_resourceManager.RedCount - _resourceSettings.StartingRed).ToString();
                    break;
                case ResourceType.Green:
                    _text.text = (_resourceManager.GreenCount - _resourceSettings.StartingGreen).ToString();
                    break;
                case ResourceType.Blue:
                    _text.text = (_resourceManager.BlueCount - _resourceSettings.StartingBlue).ToString();
                    break;
                default:
                    Debug.LogError("No resource type");
                    break;
            }
            
        }
    }
}