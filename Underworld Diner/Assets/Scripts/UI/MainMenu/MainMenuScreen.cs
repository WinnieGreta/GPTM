using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
        [Inject] private IGameManager _gameManager;
        [SerializeField] private Button _startButton;
        void Start()
        {
            _startButton.onClick.AddListener(_gameManager.LoadGameScene);
        }

    }
}
