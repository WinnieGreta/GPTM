using Gameplay.Monster.Installer;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterFacade : MonoBehaviour, IMonster, IPoolable<Transform>, IDespawnable, IDamagable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private MonsterStatusComponent _status;

        public DishType ExpectedDish => _status.ExpectedDish;

        private MonsterPool _pool;
        private bool _hasStarted;
        
        public void OnDespawned()
        {
            _signalBus.Fire<OnDespawnedSignal>();
        }

        public void OnSpawned(Transform transform)
        {
            gameObject.transform.position = transform.position;
            SendOnSpawnedSignal();
        }

        public void Start()
        {
            SendOnSpawnedSignal();
        }
        
        // to guarantee that inactive monsters in the spawn pool would fire signal on start
        // otherwise some of the components required would not be initialized yet
        private void SendOnSpawnedSignal()
        {
            if (_hasStarted)
            {
                _signalBus.Fire<OnSpawnedSignal>();
            }
            else
            {
                _hasStarted = true;
            }
            
        }
        
        public void Serve(DishType dish)
        {
            _status.ExpectedDish = DishType.None;
        }

        public void Despawn()
        {
            _pool.Despawn(this);
        }

        public void InjectPool(MonsterPool pool)
        {
            _pool = pool;
        }

        public bool GetDamaged(float damage)
        {
            _status.Health -= damage;
            return true;
        }
    }
}