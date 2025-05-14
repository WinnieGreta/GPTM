using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu
{
    public class LoadLevelButton : MonoBehaviour
    {
        [Inject] private IApplicationManager _gameManager;
        [SerializeField] private Button _startButton;
        [SerializeField] private string _sceneName;
        
        void Start()
        {
            _startButton.onClick.AddListener(() => _gameManager.LoadGameScene(_sceneName));
        }
    }
}