using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu.Book.States
{
    // when the book is in main menu, before it is opened we want it to react to player
    // hovering their mouse pointer over the book
    public class BookWaitToEnterState : BaseBookState
    {
      
        [Inject] private BookAnimationComponent _animationComponent;
        [Inject] private BookAnimationSettings _bookAnimationSettings;
        [Inject] private BookStateControllerComponent _controllerComponent;
        [Inject] private Image _image;

        public override void Initialize()
        {
            _image.raycastTarget = true;
        }

        public override void Enter()
        {
            if (!_bookAnimationSettings.SkipMovingToPosition)
            {
                _animationComponent.SetToStartingPosition();
            }
        }
        
        public override void OnPointerEnter()
        {
            if (!_bookAnimationSettings.SkipMovingToPosition)
            {
                _animationComponent.MoveUpOnPointerEnter();
            }
        }

        public override void OnPointerExit()
        {
            if (!_bookAnimationSettings.SkipMovingToPosition)
            {
                _animationComponent.MoveDownOnPointerExit();
            }
        }

        public override void OnPointerClick()
        {
            _controllerComponent.ChangeState(BookState.Enter);
        }
        
        public override void Exit()
        {
            _image.raycastTarget = false;
        }
    }
}