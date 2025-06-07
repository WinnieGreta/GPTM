using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookExitButton : MonoBehaviour
    {
        [Inject] private IApplicationManager _applicationManager; 
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _exitButton.onClick.AddListener(ExitApplication);
        }

        private void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            _applicationManager.OnAppExit();
        }
    }
}