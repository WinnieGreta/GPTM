using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.Book.States
{
    public class BookOpenState : BaseBookState
    {
        [Inject] private BookContentSettings _bookContent;
        [Inject] private List<RectTransform> _pagesAnchors;
        [Inject] private List<BookPage> _pages;
        
        public override void Enter()
        {
            for (int i = 0; i < _pagesAnchors.Count; i++)
            {
                var page = _pages.FirstOrDefault(x => x.PageOrder == i);
                page.gameObject.SetActive(true);
                page.transform.position = _pagesAnchors[i].transform.position;
                
            }
        }
        
    }
}