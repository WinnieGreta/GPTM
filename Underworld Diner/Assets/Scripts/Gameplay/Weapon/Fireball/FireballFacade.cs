using System;
using Interfaces;
using Interfaces.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapon.Fireball
{
    public class FireballFacade : DamageDealerFacade, IProjectile, IPoolable<Vector3>, IFixedTickable
    {
        [Inject] private MonoPoolableMemoryPool<Vector3, FireballFacade> _memoryPool;
        [Inject] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _damage = 1;
        
        private Vector3 _shootDirection;
        
        private void Awake()
        {
                
        }
        public void Shoot(Vector3 shootDirection)
        {
            transform.right = shootDirection;
            _shootDirection = shootDirection;
        }

        public void OnDespawned()
        {
            _memoryPool.Despawn(this);
        }

        public void OnSpawned(Vector3 position)
        {
            transform.position = position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log($"Fireball hit {other}");
            var target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                //Debug.Log("Damaged something");
                target.GetDamaged(_damage);
            }
        }

        public void FixedTick()
        {
            _rigidbody2D.position += (Vector2)_shootDirection * (Time.deltaTime * _speed);
        }
    }
}