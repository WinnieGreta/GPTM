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
        
        private InputActionMap _playerIndirect;
        private InputAction _shootAction;
        private InputAction _pointAction;
        
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
                ProcessAttack(worldPosition);
            }
        }

        private void ProcessAttack(Vector3 worldPosition)
        {
            var fireballDirection = worldPosition - _transform.position;
           /* var fireball = GameObject.Instantiate(_playerAttackWeapon.WeaponPrefab,
                _transform.position + fireballDirection,
                Quaternion.AngleAxis(Mathf.Atan2(fireballDirection.y, fireballDirection.x) * Mathf.Rad2Deg, Vector3.forward));
                //Quaternion.LookRotation(fireballDirection)); */
            //fireball.GetComponent<IProjectile>().Shoot(fireballDirection);
            fireballDirection.z = 0;
            fireballDirection = fireballDirection.normalized;
            var fireball = _projectileFactory.Create(_transform.position + fireballDirection * 2);
            fireball.Shoot(fireballDirection);


        }
    }
}