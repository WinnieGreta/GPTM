using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.Book
{

    public enum BookType
    {
        MainMenu,
        RecipeBook
    }

    [CreateAssetMenu(fileName = "BookSettings", menuName = "Installers/BookSettings")]
    public class BookSettings : ScriptableObjectInstaller<BookSettings>
    {
        [SerializeField] private BookAnimationSettings bookAnimationSettings;
        [SerializeField] private BookContentSettings bookContentSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(bookContentSettings).AsSingle();
            Container.BindInstance(bookAnimationSettings).AsSingle();
        }
    }

    [Serializable]
    internal class BookAnimationSettings
    {
        [field: Header("Enter Animation Settings")]
        [field: SerializeField] public Vector2 StartAnchorOffset { get; private set; } = new Vector2(0, -600);
        [field: SerializeField] public Vector2 MouseHoverAnimationOffset { get; private set; } = new Vector2(0, 30);
        [field: SerializeField] public float MouseHoverAnimationDuration { get; private set; }
        [field: SerializeField] public float MoveToPositionAnimationDuration { get; private set; }
        [field: SerializeField] public bool SkipMovingToPosition { get; private set; }
        [field: SerializeField] public bool StartOpened { get; private set; }
        
        
    }
        
    [Serializable]
    internal class BookContentSettings
    {
        [field:SerializeField] public BookType Type { get; private set; }
        [field:SerializeField] public GameObject CorePage { get; private set; }
        [field:SerializeField] public List<GameObject> Pages { get; private set; }
    }
}