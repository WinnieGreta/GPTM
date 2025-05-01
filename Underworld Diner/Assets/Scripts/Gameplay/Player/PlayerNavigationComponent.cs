using System.Collections.Generic;
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
        private int _clickableLayoutLayerMask;
        private int _notClickableLayerMask;

        public void Initialize()
        {
            _playerIndirect = _playerInput.actions.FindActionMap("PlayerIndirect");
            _playerIndirect.Enable();
            _pointAction = _playerIndirect.FindAction("Point");
            _click = _playerIndirect.FindAction("Click");
            _clickableLayoutLayerMask = LayerMask.GetMask("Ground", "Stations");
            _notClickableLayerMask = LayerMask.GetMask("Wall");
        }

        public void Tick()
        {
            if(_click.WasReleasedThisFrame())
            {
                _position = _pointAction.ReadValue<Vector2>();
                _worldPosition = Camera.main.ScreenToWorldPoint(_position);
                
                //RaycastHit2D clickableHit = Physics2D.Raycast(_worldPosition, Vector2.zero, Mathf.Infinity, _clickableLayoutLayerMask);
                var clickableHit = Physics2D.OverlapPoint(_worldPosition, _clickableLayoutLayerMask);
                var nonClickableHit = Physics2D.OverlapPoint(_worldPosition, _notClickableLayerMask);
                if (clickableHit != null && nonClickableHit == null)
                {
                    Debug.Log(clickableHit.transform.name + " was hit");
                    MoveToPosition(_worldPosition);
                    
                }
                Debug.Log(_worldPosition);
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