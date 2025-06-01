using System.Collections.Generic;
using Gameplay.Player.Signals;
using Interfaces;
using Interfaces.Player;
using Signals;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerNavigationComponent : IInitializable, ITickable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private PlayerInput _playerInput;
        [Inject] private NavMeshAgent _navMeshAgent;
        [Inject] private PlayerStatusComponent _status;

        public IReadOnlyCollection<ICommand> CommandQueue => _commandQueue;
        
        private InputActionMap _playerIndirect;
        private InputAction _pointAction;
        private InputAction _click;
        private Queue<QueueCommand> _commandQueue = new ();
        
        private static readonly int GROUND_LAYER = LayerMask.NameToLayer("Ground");
        private static readonly int STATIONS_LAYER = LayerMask.NameToLayer("Stations");
        private static readonly int WALL_LAYER = LayerMask.NameToLayer("Wall");

        public void Initialize()
        {
            _playerIndirect = _playerInput.actions.FindActionMap("PlayerIndirect");
            _playerIndirect.Enable();
            _pointAction = _playerIndirect.FindAction("Point");
            _click = _playerIndirect.FindAction("Click");
            _signalBus.Subscribe<OnGamePauseSignal>(OnGamePaused);
            _signalBus.Subscribe<OnGameUnpauseSignal>(OnGameUnpaused);
            
        }

        public void Tick()
        {
            if(_click.WasReleasedThisFrame())
            {
                var position = _pointAction.ReadValue<Vector2>();
                var worldPosition = Camera.main.ScreenToWorldPoint(position);
                ProcessClick(worldPosition);
                //Debug.Log(_worldPosition);
            }

            ProcessDestination();
        }

        private void ProcessDestination()
        {
            if (!HasReachedDestination())
            {
                return;
            }
            _signalBus.Fire(new DestinationReachedSignal());
            _status.StationImMovingTo = null;
            if (_commandQueue.Count == 0)
            {
                return;
            }
            var element = _commandQueue.Dequeue();
            ProcessStationClick(element.TargetStation);
        }
        
        private void ProcessClick(Vector3 worldPosition)
        {
            // for our purposes does the same as raycast
            var clickableHit = Physics2D.OverlapPoint(worldPosition);
            if (clickableHit == null)
            {
                return;
            }
            
            // case guards to switch on non-constants
            switch (true)
            {
                case true when clickableHit.gameObject.layer == GROUND_LAYER:
                    MoveToPosition(worldPosition);
                    break;
                case true when clickableHit.gameObject.layer == STATIONS_LAYER:
                    //Debug.Log("Hit the station from switch");
                    EnqueueStation(clickableHit.GetComponent<IStation>());
                    break;
                case true when clickableHit.gameObject.layer == WALL_LAYER:
                default:
                    //Debug.Log("Hit the wall from switch");
                    break;
            }
        }

        private void MoveToPosition(Vector2 position)
        {
            if (_status.StationImMovingTo != null)
            {
                return;
            }
            NavMesh.SamplePosition(position, out var hit, 100, _navMeshAgent.areaMask);
            _navMeshAgent.SetDestination(hit.position);
            //Debug.Log("Position " + position);
            //Debug.Log("Hit position " + hit.position);
        }

        private void EnqueueStation(IStation station)
        {
            if (_status.StationImMovingTo != null)
            {
                _commandQueue.Enqueue(new QueueCommand { TargetStation = station});
            }
            else
            {
                ProcessStationClick(station);
            }
        }

        private void ProcessStationClick(IStation station)
        {
            var target = station.GetClosestAnchorPosition(_navMeshAgent);
            MoveToPosition(target);
            _status.StationImMovingTo = station;
            //_currentTargetStation = station;
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

        // we need this to not have any player actions registered while game in a pause mode
        // using signals instead of events to not depend on an object that invoked this event
        private void OnGamePaused()
        {
            _playerIndirect.Disable();
        }

        private void OnGameUnpaused()
        {
            _playerIndirect.Enable();
        }

        private class QueueCommand : ICommand
        {
            public IStation TargetStation { get; set; }
        }
        
    }
    
}