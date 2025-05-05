using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterAnimatorComponent : IInitializable, ILateTickable, IFixedTickable
    {
        [Inject] private Animator _animator;
        [Inject] private SpriteRenderer _spriteRenderer;
        
        private Transform _transform;
        private Vector3 _moveOffset;
        private Vector3 _lastPosition;
        private bool _isMoving;
        private bool _isFacingRight;
        private bool _wasFacingRight;
        private const float ANIMATION_THRESHOLD = 0.03f;
        private const float FLIP_ANIMATION_THRESHOLD = 0.015f;

        public void Initialize()
        {
            _transform = _animator.transform;
            _lastPosition = _transform.position;
        }

        public void FixedTick()
        {
            _wasFacingRight = _isFacingRight;
            _isMoving = false;
            if (_lastPosition != _transform.position && (_lastPosition - _transform.position).magnitude > ANIMATION_THRESHOLD)
            {
                _moveOffset = _transform.position - _lastPosition;
                _isMoving = true;
                
                if ((_moveOffset.x > FLIP_ANIMATION_THRESHOLD) || !(_moveOffset.x < -FLIP_ANIMATION_THRESHOLD))
                {
                    _isFacingRight = true;
                }
                else if (!(_moveOffset.x > FLIP_ANIMATION_THRESHOLD) || (_moveOffset.x < -FLIP_ANIMATION_THRESHOLD))
                {
                    _isFacingRight = false;
                }
            }
            _lastPosition = _transform.position;
        }
        
        public void LateTick()
        {
            if (_isFacingRight != _wasFacingRight)
            {
                _spriteRenderer.flipX = !_isFacingRight;
            }
            _animator.SetBool("isMoving", _isMoving);
        }

    }
}