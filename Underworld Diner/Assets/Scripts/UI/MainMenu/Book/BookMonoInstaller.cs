using System;
using System.Collections.Generic;
using UI.MainMenu.Book.States;
using UI.MainMenu.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookMonoInstaller : MonoInstaller
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private List<RectTransform> _pagesAnchors;
        [Inject] private BookContentSettings _bookContentSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_animator).AsSingle();
            Container.BindInstance(_rectTransform).AsSingle();
            Container.BindInstance(_image).AsSingle();
            Container.BindInstance(_pagesAnchors).AsSingle();

            Container.BindInterfacesAndSelfTo<BookAnimationComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<BookStateControllerComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<BookStatusComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<BookFacade>().FromComponentOnRoot().AsSingle();

            Container.DeclareSignal<PageFromButtonSignal>();
            
            Container.BindFactory<BookState, BaseBookState, BaseBookState.Factory>()
                .FromMethod(CreateBookState);

            foreach (var page in _bookContentSettings.Pages)
            {
                Container.BindInterfacesAndSelfTo<BookPage>()
                    .FromComponentInNewPrefab(page)
                    .AsTransient()
                    .OnInstantiated<BookPage>((_, x) => x.gameObject.SetActive(false));
            }
        }

        private BaseBookState CreateBookState(DiContainer container, BookState bookState)
        {
            switch (bookState)
            {
                case BookState.WaitToEnter:
                    return container.Instantiate<BookWaitToEnterState>();
                case BookState.Enter:
                    return container.Instantiate<BookEnterState>();
                case BookState.Open:
                    return container.Instantiate<BookOpenState>();
                case BookState.Null:
                    return container.Instantiate<BookNullState>();
            }

            throw new Exception("No book state!");
        }
    }
}