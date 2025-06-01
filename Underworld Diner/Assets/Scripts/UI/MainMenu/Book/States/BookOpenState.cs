using System.Collections.Generic;
using System.Linq;
using UI.MainMenu.Signals;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.Book.States
{
    public class BookOpenState : BaseBookState
    {
        [Inject] private BookContentSettings _bookContent;
        [Inject] private List<RectTransform> _pagesAnchors;
        [Inject] private List<BookPage> _pages;
        [Inject] private SignalBus _signalBus;
        [Inject] private BookStatusComponent _bookStatus;
        
        public override void Initialize()
        {
            
        }


        public override void Enter()
        {
            _signalBus.Subscribe<PageFromButtonSignal>(OnNewPageFromButton);
            
            for (int i = 0; i < _pagesAnchors.Count; i++)
            {
                var page = _pages.FirstOrDefault(x => x.PageOrder == i);
                page.gameObject.SetActive(true);
                page.transform.position = _pagesAnchors[i].transform.position;
                _bookStatus.ActivePages.Add(page);
            }
        }
        
        private void OnNewPageFromButton(PageFromButtonSignal args)
        {
            //Debug.Log("New page from button received!");
            int pageNumber = args.PageOrder;

            var pageToRemove = _bookStatus.ActivePages[pageNumber];
            pageToRemove.HidePage();
            pageToRemove.gameObject.SetActive(false);

            var pageToShow = _pages.FirstOrDefault(p => p.name == args.Name);
            pageToShow.gameObject.SetActive(true);
            pageToShow.transform.position = _pagesAnchors[pageNumber].transform.position;
            pageToShow.ShowPage();
            _bookStatus.ActivePages[pageNumber] = pageToShow;

        }
        
    }
}