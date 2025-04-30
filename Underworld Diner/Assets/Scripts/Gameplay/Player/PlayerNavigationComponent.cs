using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerNavigationComponent : IInitializable, ITickable
    {
        [Inject] private PlayerInput _playerInput;

        private InputActionMap _playerIndirect;
        private InputAction _pointAction;
        private InputAction _click;
        private Vector2 _position;
        private Vector2 _worldPosition;
        public void Initialize()
        {
            _playerIndirect = _playerInput.actions.FindActionMap("PlayerIndirect");
            _playerIndirect.Enable();
            _pointAction = _playerIndirect.FindAction("Point");
            _click = _playerIndirect.FindAction("Click");
        }

        public void Tick()
        {
            if(_click.WasPerformedThisFrame())
            {
                _position = _pointAction.ReadValue<Vector2>();
                _worldPosition = Camera.main.ScreenToWorldPoint(_position);
                Debug.Log(_worldPosition);
            }
        }
        
    }
}