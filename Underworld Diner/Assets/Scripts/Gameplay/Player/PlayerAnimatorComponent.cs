using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerAnimatorComponent : IInitializable
    {
        [Inject] private Animator _animator;
        [Inject] private PlayerInput _playerInput;

        private InputAction _moveAction;
        private Vector2 _moveInput;

        public void Initialize()
        {
            _moveAction = _playerInput.actions.FindAction("Move");
            _moveAction.started += StartMove;
            _moveAction.performed += Move;
            _moveAction.canceled += StopMove;
        }
        public void Move(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            Debug.Log(_moveInput);
            _animator.SetFloat("InputX", _moveInput.x);
            _animator.SetFloat("InputY", _moveInput.y);
        }

        public void StopMove(InputAction.CallbackContext context)
        {
            _animator.SetBool("isMoving", false);
        }
        
        private void StartMove(InputAction.CallbackContext obj)
        {
            _animator.SetBool("isMoving", true);
        }


    }
}