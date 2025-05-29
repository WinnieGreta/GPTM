using System.Collections.Generic;
using Gameplay.Monster.States;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterAIComponent : IInitializable, ITickable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private MonsterStatusComponent _statusComponent;
        [Inject] private MonsterServiceSettings _monsterSettings;

        private BaseMonsterState.Factory _monsterStateFactory;
        private BaseMonsterState _currentStateEntity = null;
        private MonsterState _currentState;
        private bool _isDead;
        //public IChair MyChair { get; private set; }

        [Inject]
        public void Construct(BaseMonsterState.Factory monsterStateFactory)
        {
            _monsterStateFactory = monsterStateFactory;
        }
        
        public void Initialize()
        {
            
        }

        [Inject]
        private void OnInject()
        {
            _signalBus.Subscribe<OnSpawnedSignal>(StartMonster);
        }
        
        private void StartMonster()
        {
            _statusComponent.FullOrder.Clear();
            _statusComponent.Patience = _monsterSettings.StartingPatience;
            _statusComponent.Health = _monsterSettings.StartingHealth;
            _isDead = false;
            ChangeState(MonsterState.Enter);
        }

        internal void ChangeState(MonsterState monsterState)
        {
            if (_currentStateEntity != null)
            {
                _currentStateEntity.Exit();
                _currentStateEntity = null;
            }

            _currentState = monsterState;
            _currentStateEntity = _monsterStateFactory.Create(monsterState);
            _currentStateEntity.Enter();
        }

        public void Tick()
        {
            _currentStateEntity?.OnTick();
            if (_statusComponent.Health <= 0.01 && !_isDead)
            {
                _isDead = true;
                ChangeState(MonsterState.Die);
            }
        }

        public void TakeChairByMonster(IChair chair, IMonster monster)
        {
            chair.TakeChair(monster);
            _statusComponent.MyChair = chair;
        }

        public void FreeChairByMonster()
        {
            _statusComponent.MyChair.FreeChair();
            _statusComponent.MyChair = null;
        }
    }
    
}