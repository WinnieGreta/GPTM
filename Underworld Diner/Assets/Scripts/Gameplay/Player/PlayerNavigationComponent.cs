using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerNavigationComponent : IInitializable, ITickable
    {
        [Inject] private PlayerInput _playerInput;
        [Inject] private NavMeshAgent _navMeshAgent;

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
            if(_click.WasReleasedThisFrame())
            {
                _position = _pointAction.ReadValue<Vector2>();
                _worldPosition = Camera.main.ScreenToWorldPoint(_position);
                //_navMeshAgent.SetDestination(_worldPosition);
                
                RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log(hit.transform.name + " was hit");
                    MoveToPosition(hit.point);
                    
                }
                //Debug.Log(_worldPosition);
            }
        }

        private void MoveToPosition(Vector2 position)
        {
            NavMesh.SamplePosition(position, out var hit, 100, _navMeshAgent.areaMask);
            _navMeshAgent.SetDestination(hit.position);
            Debug.Log("Position " + position);
            Debug.Log("Hit position " + hit.position);
        }
    }
}