using UnityEngine;
using Zenject;

namespace UI.MainMenu.Book.States
{
    public class BookOpenState : BaseBookState
    {
        [Inject] private BookContentSettings _bookContent;
        
        public override void Enter()
        {
            Debug.Log("Book Opened");
        }
    }
}