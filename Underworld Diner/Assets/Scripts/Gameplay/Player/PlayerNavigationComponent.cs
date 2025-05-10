using System;
using System.Collections.Generic;
using Interfaces;
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
        private IStation _stationImMovingTo;
        
        private static readonly int GROUND_LAYER = LayerMask.NameToLayer("Ground");
        private static readonly int STATIONS_LAYER = LayerMask.NameToLayer("Stations");
        private static readonly int WALL_LAYER = LayerMask.NameToLayer("Wall");
        

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
                ProcessClick();
                //Debug.Log(_worldPosition);
            }

            if (HasReachedDestination() && _stationImMovingTo != null)
            {
                switch (_stationImMovingTo)
                {
                    case ITable table:
                        table.FreeTable();
                        break;
                    case IKitchen kitchen:
                        kitchen.PlayerInteraction();
                        break;
                    default:
                        Debug.Log("No station");
                        break;
                }
                _stationImMovingTo = null;
            }
        }

        private void ProcessClick()
        {
            _position = _pointAction.ReadValue<Vector2>();
            _worldPosition = Camera.main.ScreenToWorldPoint(_position);
                
            // for our purposes does the same as raycast
            var clickableHit = Physics2D.OverlapPoint(_worldPosition);
            if (clickableHit == null)
            {
                return;
            }
            // case guards to switch on non-constants
            switch (true)
            {
                case true when clickableHit.gameObject.layer == GROUND_LAYER:
                    MoveToPosition(_worldPosition);
                    break;
                case true when clickableHit.gameObject.layer == STATIONS_LAYER:
                    //Debug.Log("Hit the station from switch");
                    ProcessStationClick(clickableHit.GetComponent<IStation>());
                    break;
                case true when clickableHit.gameObject.layer == WALL_LAYER:
                default:
                    //Debug.Log("Hit the wall from switch");
                    break;
            }
        }

        private void MoveToPosition(Vector2 position)
        {
            NavMesh.SamplePosition(position, out var hit, 100, _navMeshAgent.areaMask);
            _navMeshAgent.SetDestination(hit.position);
            //Debug.Log("Position " + position);
            //Debug.Log("Hit position " + hit.position);
        }

        private void ProcessStationClick(IStation station)
        {
            var target = station.GetClosestAnchorPosition(_navMeshAgent);
            MoveToPosition(target);
            _stationImMovingTo = station;
        }

        private void FreeTable(ITable table)
        {
            table.FreeTable();
        }

        private bool HasReachedDestination()
        {
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}