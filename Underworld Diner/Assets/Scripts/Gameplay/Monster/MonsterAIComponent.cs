using Gameplay.Monster.Abstract;
using Interfaces;
using Signals;
using Zenject;

namespace Gameplay.Monster
{
    internal class MonsterAIComponent : ITickable, IAiComponent
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private MonsterStatusComponent _statusComponent;
        [Inject] private MonsterServiceSettings _monsterSettings;
        [Inject] private IMonster _monster;

        private BaseMonsterState.Factory _monsterStateFactory;
        private BaseMonsterState _currentStateEntity;
        private MonsterState _currentState;
        private bool _isDead;
        //public IChair MyChair { get; private set; }

        [Inject]
        private void Construct(BaseMonsterState.Factory monsterStateFactory)
        {
            _monsterStateFactory = monsterStateFactory;
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

        public void ChangeState(MonsterState monsterState)
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
        
        public void TakeChairByMonster(IChair chair)
        {
            chair.TakeChair(_monster);
            _statusComponent.MyChair = chair;
        }

        public void FreeChairByMonster()
        {
            if (_statusComponent.MyChair != null)
            {
                _statusComponent.MyChair.FreeChair();
                _statusComponent.MyChair = null;
            }
        }
    }
    
}