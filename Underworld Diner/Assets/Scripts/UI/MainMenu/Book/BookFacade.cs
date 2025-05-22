using Interfaces.UI;
using UI.MainMenu.Book.States;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookFacade : MonoBehaviour, IBook, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private BookStateControllerComponent _controllerComponent;

        private BaseBookState _currentStateEntity;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _controllerComponent.CurrentStateEntity.OnPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _controllerComponent.CurrentStateEntity.OnPointerExit();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _controllerComponent.CurrentStateEntity.OnPointerClick();
        }
    }
}