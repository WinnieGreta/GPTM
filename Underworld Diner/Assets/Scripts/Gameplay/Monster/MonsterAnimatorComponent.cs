using Gameplay.Monster.Abstract;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster
{
    internal class MonsterAnimatorComponent : IInitializable, ILateTickable, IFixedTickable, IAnimatorComponent
    {
        [Inject] private Animator _animator;
        [Inject] private SpriteRenderer _spriteRenderer;
        [Inject] private NavMeshAgent _navMeshAgent;
        
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int IsSitting = Animator.StringToHash("isSitting");
        
        private Transform _transform;
        private Vector3 _moveOffset;
        private Vector3 _lastPosition;
        private bool _isMoving;
        private bool _isSitting;
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
            MovementCheck();
        }
        
        public void LateTick()
        {
            if (_isFacingRight != _wasFacingRight)
            {
                _spriteRenderer.flipX = !_isFacingRight;
            }
            _animator.SetBool(IsMoving, _isMoving);
            _animator.SetBool(IsSitting, _isSitting);
        }

        private void MovementCheck()
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

        public void StartSit(bool isFacingRight)
        {
            _isSitting = true;
            _spriteRenderer.sortingOrder = 1;
            _navMeshAgent.enabled = false;
            _isFacingRight = isFacingRight;
        }

        public void StopSit()
        {
            _isSitting = false;
            //_spriteRenderer.sortingOrder = 0;
            _navMeshAgent.enabled = true;
        }

        public void DeathAnimation()
        {
            _animator.SetBool(IsDead, true);
        }

        public void Restart()
        {
            StopSit();
            _isMoving = false;
            _animator.SetBool(IsDead, false);
            _animator.Rebind();
        }

    }
}