using Zenject;

namespace UI.MainMenu.Book.States
{
    public enum BookState
    {
        WaitToEnter,
        Enter,
        Open,
        Null
    }
    
    public abstract class BaseBookState : IInitializable
    {
        public virtual void Initialize()
        {
            
        }
        
        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual void OnTick()
        {
            
        }

        public virtual void OnPointerEnter()
        {
            
        }
        public virtual void OnPointerExit()
        {
            
        }
        public virtual void OnPointerClick()
        {
            
        }
        

        public class Factory : PlaceholderFactory<BookState, BaseBookState>
        {
            
        }
    }
}