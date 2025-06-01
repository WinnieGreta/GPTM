using System;
using UI.MainMenu.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookPageButton : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private Button _bookButton;
        [SerializeField] private BookPage _bookPage;

        private void Start()
        {
            _bookButton.onClick.AddListener(OpenPage);
        }

        private void OpenPage()
        {
            Debug.Log("Book open new page button clicked!");
            _signalBus.Fire(new PageFromButtonSignal
            {
                PageOrder = _bookPage.PageOrder,
                Name = _bookPage.name + "(Clone)"
            });
        }
    }
}