using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerMovementComponent : ITickable, IInitializable, IFixedTickable
    {
        [Inject] private PlayerInput _playerInput;
        [Inject] private Rigidbody2D _playerRigidbody;
        [Inject] private PlayerMovementSettings _playerMovementSettings;

        private InputActionMap _playerDirect;
        private Vector2 _direction;
        private InputAction _moveAction;
        
        public void Initialize()
        {
            _playerDirect = _playerInput.actions.FindActionMap("PlayerDirect");
            _moveAction = _playerDirect.FindAction("Move");
        }
        
        public void Tick()
        {
            _direction = _moveAction.ReadValue<Vector2>();
        }

        public void FixedTick()
        {
            _playerRigidbody.linearVelocity = _direction.normalized * _playerMovementSettings.MaxSpeed;
        }
    }
}