using Interfaces.UI;
using UnityEngine;

namespace UI.MainMenu.Book
{
    public class BookPage : MonoBehaviour, IBookPage
    {
        [SerializeField] private CanvasGroup _pageCanvasGroup;
        // 0 - left, 1 - right
        [SerializeField] public int PageOrder;

        void Awake()
        {
            _pageCanvasGroup.alpha = 0;
            ShowPage(0.7f);
        }

        public void ShowPage(float delay)
        {
            LeanTween.alphaCanvas(_pageCanvasGroup, 1f, 0.4f)
                .setDelay(delay)
                .setEase(LeanTweenType.easeInQuad);
        }
        
        public void ShowPage()
        {
            LeanTween.alphaCanvas(_pageCanvasGroup, 1f, 0.4f)
                .setEase(LeanTweenType.easeInQuad);
        }

        public void HidePage()
        {
            LeanTween.alphaCanvas(_pageCanvasGroup, 0f, 0.3f)
                .setEase(LeanTweenType.easeInQuad);
        }
        
    }
}