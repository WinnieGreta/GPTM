using UnityEngine;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookAnimationComponent : IInitializable
    {
        [Inject] private RectTransform _rectTransform;
        [Inject] private Animator _animator;
        [Inject] private BookAnimationSettings _bookAnimationSettings;
        
        private Vector2 _endAnchor;
        private int _tweenId = -1;

        public void Initialize()
        {
            _endAnchor = _rectTransform.anchoredPosition;
            _animator.SetFloat("blendOpenBook", _bookAnimationSettings.StartOpened ? 1f : 0f);
        }
        
        public void SetToStartingPosition()
        {
            _rectTransform.anchoredPosition = _endAnchor + _bookAnimationSettings.StartAnchorOffset;
        }

        public void MoveUpOnPointerEnter()
        {
            CancelTween();
            _tweenId = LeanTween.move(_rectTransform,
                    _rectTransform.anchoredPosition + _bookAnimationSettings.MouseHoverAnimationOffset, 
                    _bookAnimationSettings.MouseHoverAnimationDuration)
                .setEase(LeanTweenType.easeOutBack)
                .id;
        }

        public void MoveDownOnPointerExit()
        {
            CancelTween();
            _tweenId = LeanTween.move(_rectTransform, 
                    _rectTransform.anchoredPosition - _bookAnimationSettings.MouseHoverAnimationOffset, 
                    _bookAnimationSettings.MouseHoverAnimationDuration)
                .setEase(LeanTweenType.easeOutBack)
                .id;
        }
        
        private void CancelTween()
        {
            if (LeanTween.isTweening(_tweenId))
            {
                LeanTween.cancel(_tweenId);
            }
        }

        public void MoveToPositionThenOpen()
        {
            LeanTween.move(_rectTransform, _endAnchor, _bookAnimationSettings.MoveToPositionAnimationDuration)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(OpenBook);
        }

        private void OpenBook()
        {
            _animator.SetTrigger("openBook");
        }

        // takes into account both animator animation and tween animation
        public bool IsAnimationPlaying()
        {
            if (LeanTween.isTweening(_tweenId) ||
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                return true;
            }

            return false;
        }

        public void PageEnter(CanvasGroup pageCanvasGroup)
        {
            LeanTween.alphaCanvas(pageCanvasGroup, 1f, 0.3f)
                .setEase(LeanTweenType.easeInQuad);
        }

        public void PageExit(Canvas page)
        {
            
        }
        
    }
}