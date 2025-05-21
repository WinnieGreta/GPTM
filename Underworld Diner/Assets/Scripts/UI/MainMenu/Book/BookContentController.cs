using UnityEngine;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookContentController : IInitializable
    {

        public void Initialize()
        {
            
        }
        
        private void ShowPageContent()
        {
            Debug.Log("Page content!");
        }

    }
}