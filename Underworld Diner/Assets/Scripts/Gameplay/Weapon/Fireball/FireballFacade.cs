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
        [Inject] private FireballAnimatorComponent _animatorComponent;
        [Inject] private FireballSimpleSettings _fireballSettings;

        private float _speed;
        private float _damage;
        private int _hitsLeft;
        
        private int WALL_LAYER;
        
        private Vector3 _shootDirection;
        
        private void Awake()
        {
            WALL_LAYER = LayerMask.NameToLayer("Wall");
        }
        public void Shoot(Vector3 shootDirection)
        {
            transform.right = shootDirection;
            _shootDirection = shootDirection;
        }

        public void OnDespawned()
        {
            //Debug.Log("Fireball despawned");
        }

        public void OnSpawned(Vector3 position)
        {
            transform.position = position;
            _speed = _fireballSettings.Speed;
            _damage = _fireballSettings.Damage;
            _hitsLeft = _fireballSettings.HitsLeft;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Fireball hit {other}");
            var target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                //Debug.Log("Damaged something");
                _animatorComponent.ExplodeOnImpact();
                target.GetDamaged(_damage);
                _hitsLeft--;
            }
            else if (other.gameObject.layer == WALL_LAYER)
            {
                Debug.Log("WALL-E");
                _hitsLeft = 0;
            }
        }

        public void FixedTick()
        {
            if (_hitsLeft <= 0)
            {
                _animatorComponent.FinalExplosion();
                _rigidbody2D.position += (Vector2)_shootDirection * (Time.deltaTime * Time.deltaTime * _speed); // * _animatorComponent.FinalExplosionDeceleration());
                if (_animatorComponent.FinishedFinalExplosion())
                {
                    Despawn();
                }
            }
            else
            {
                _rigidbody2D.position += (Vector2)_shootDirection * (Time.deltaTime * _speed);
            }
        }

        public void Despawn()
        {
            _memoryPool.Despawn(this);
        }
    }
}