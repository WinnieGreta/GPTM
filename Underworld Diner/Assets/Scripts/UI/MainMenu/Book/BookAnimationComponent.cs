using System.ComponentModel;
using UI.MainMenu.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu.Book
{
    public class BookAnimationComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private SignalBus _signalBus; 
        [SerializeField] private Image _image;
        [SerializeField] private Animator _animator;

        private RectTransform _rectTransform;
        private Vector2 _endAnchor;
        private int _tweenId = -1;
        public void Awake()
        {
            _rectTransform = _image.rectTransform;
            _endAnchor = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = _endAnchor + new Vector2(0, -600);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CancelTween();
            _tweenId = LeanTween.move(_rectTransform, _rectTransform.anchoredPosition + new Vector2(0, 30), 0.5f)
                .setEase(LeanTweenType.easeOutBack)
                .id;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CancelTween();
            _tweenId = LeanTween.move(_rectTransform, _rectTransform.anchoredPosition + new Vector2(0, -30), 0.5f)
                .setEase(LeanTweenType.easeOutBack)
                .id;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CancelTween();
            MoveToPosition();
        }
        
        private void CancelTween()
        {
            if (LeanTween.isTweening(_tweenId))
            {
                LeanTween.cancel(_tweenId);
            }
        }

        private void MoveToPosition()
        {
            LeanTween.move(_rectTransform, _endAnchor, 0.8f)
                .setDelay(0.2f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(OnArrival);
        }

        private void OnArrival()
        {
            _animator.SetTrigger("openBook");
            _image.raycastTarget = false;
            _signalBus.TryFire<BookOnPositionSignal>();
        }

    }
}