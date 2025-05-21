using UI.MainMenu.Book.States;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookStateControllerComponent : IInitializable, ITickable
    {
        [Inject] private SignalBus _signalBus;
        
        private BaseBookState.Factory _bookStateFactory;
        internal BaseBookState CurrentStateEntity = null;
        private BookState _currentState;

        [Inject]
        public void Construct(BaseBookState.Factory bookStateFactory)
        {
            _bookStateFactory = bookStateFactory;
        }

        [Inject]
        private void OnInject()
        {
            
        }
        
        public void Initialize()
        {
            ChangeState(BookState.WaitToEnter);
        }

        internal void ChangeState(BookState bookState)
        {
            if (CurrentStateEntity != null)
            {
                CurrentStateEntity.Exit();
                CurrentStateEntity = null;
            }

            _currentState = bookState;
            CurrentStateEntity = _bookStateFactory.Create(bookState);
            CurrentStateEntity.Enter();
        }

        public void Tick()
        {
            CurrentStateEntity?.OnTick();
        }
    }
}