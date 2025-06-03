using Interfaces.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerAttackComponent : IInitializable, ITickable
    {
        [Inject] private PlayerInput _playerInput;
        [Inject] private Transform _transform;
        [Inject(Id = "FireballFactory")] private IProjectile.Factory _projectileFactory;
        [Inject] private PlayerStatusComponent _status;
        
        private InputActionMap _playerIndirect;
        private InputAction _shootAction;
        private InputAction _pointAction;

        // TODO fireball cost settings
        private float FIREBALL_PLACEHOLDER_COST = 50f;
        
        public void Initialize()
        {
            _playerIndirect = _playerInput.actions.FindActionMap("PlayerIndirect");
            _shootAction = _playerIndirect.FindAction("Attack");
            _pointAction = _playerIndirect.FindAction("Point");
        }


        public void Tick()
        {
            if (_shootAction.WasReleasedThisFrame())
            {
                var position = _pointAction.ReadValue<Vector2>();
                var worldPosition = Camera.main.ScreenToWorldPoint(position);
                if (TryShootWithMana())
                {
                    ProcessAttack(worldPosition);
                }
            }
        }

        private void ProcessAttack(Vector3 worldPosition)
        {
            var fireballDirection = worldPosition - _transform.position;
            fireballDirection.z = 0;
            fireballDirection = fireballDirection.normalized;
            var fireball = _projectileFactory.Create(_transform.position + fireballDirection * 2);
            fireball.Shoot(fireballDirection);
            
        }

        private bool TryShootWithMana()
        {
            if (_status.Mana - FIREBALL_PLACEHOLDER_COST > 0)
            {
                _status.Mana -= FIREBALL_PLACEHOLDER_COST;
                //Debug.Log("Mana lost from player status");
                return true;
            }
            //Debug.Log("Not enough mana!");
            return false;
        }
    }
}