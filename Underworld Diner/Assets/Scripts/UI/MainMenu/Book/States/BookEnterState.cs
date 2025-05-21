using Zenject;

namespace UI.MainMenu.Book.States
{
    public class BookEnterState : BaseBookState
    {
        [Inject] private BookAnimationComponent _animationComponent;
        [Inject] private BookStateControllerComponent _controllerComponent;
        
        public override void Enter()
        {
            _animationComponent.MoveToPositionThenOpen();
        }

        public override void OnTick()
        {
            if (!_animationComponent.IsAnimationPlaying())
            {
                _controllerComponent.ChangeState(BookState.Open);
            }
        }
    }
}