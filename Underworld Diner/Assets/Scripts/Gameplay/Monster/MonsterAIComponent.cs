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

        private BaseMonsterState.Factory _monsterStateFactory;
        private BaseMonsterState _currentStateEntity = null;
        private MonsterState _currentState;
        public IChair MyChair { get; private set; }

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
        }

        public void TakeChairByMonster(IChair chair, IMonster monster)
        {
            chair.TakeChair(monster);
            MyChair = chair;
        }

        public void FreeChairByMonster(IChair chair)
        {
            chair.FreeChair();
            MyChair = null;
        }
    }
    
}