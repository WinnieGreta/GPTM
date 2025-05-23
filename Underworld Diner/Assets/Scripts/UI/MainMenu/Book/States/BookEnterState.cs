using Zenject;

namespace UI.MainMenu.Book.States
{
    public class BookEnterState : BaseBookState
    {
        [Inject] private BookAnimationComponent _animationComponent;
        [Inject] private BookStateControllerComponent _controllerComponent;
        [Inject] private BookContentSettings _bookSettings;
        
        public override void Enter()
        {
            _animationComponent.MoveToPositionThenOpen();
        }

        public override void OnTick()
        {
            if (!_animationComponent.IsAnimationPlaying())
            {
                if (_bookSettings.Type == BookType.MainMenu)
                {
                    _controllerComponent.ChangeState(BookState.Open);
                }
            }
        }
    }
}