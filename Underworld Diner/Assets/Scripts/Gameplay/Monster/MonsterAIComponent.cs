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

        private MonsterStateEntity.Factory _monsterStateFactory;
        private MonsterStateEntity _currentStateEntity = null;
        private MonsterState _currentState;
        public IChair MyChair { get; private set; }

        [Inject]
        public void Construct(MonsterStateEntity.Factory monsterStateFactory)
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