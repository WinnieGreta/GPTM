using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerAnimatorComponent : IInitializable, ILateTickable, IFixedTickable
    {
        [Inject] private Animator _animator;
        //[Inject] private PlayerInput _playerInput;
        [Inject] private Transform _transform;
        
        private Vector3 _moveOffset;
        private Vector3 _lastPosition;
        private bool _isMoving;
        private const float ANIMATION_THRESHOLD = 0.01f;

        public void Initialize()
        {
            _lastPosition = _transform.position;
        }
        
        // By checking the character offset this frame we can determine its movement direction
        // Would work without relying on direct or indirect controls scheme
        // As we are checking for movement before addressing the animator, this wouldn't interfere with animation that is already playing
        public void FixedTick()
        {
            _isMoving = false;
            // with indirect control stopping could produce miniscule chaotic movement (fluctuations around stopping point)
            // to combat this problem we are setting a small movement threshold that would assure that we don't cycle through different animations when stopping
            if (_lastPosition != _transform.position && (_lastPosition - _transform.position).magnitude > ANIMATION_THRESHOLD)
            {
                _moveOffset = _transform.position - _lastPosition;
                _isMoving = true;
            }
            _lastPosition = _transform.position;
        }
        
        // We mustn't nullify the latest move offset because we need our idle animation have the same direction as the latest movement we had
        public void LateTick()
        {
            Vector2 moveInput = _moveOffset.normalized;
            _animator.SetFloat("InputX", moveInput.x);
            _animator.SetFloat("InputY", moveInput.y);
            _animator.SetBool("isMoving", _isMoving);
        }
        
    }
}