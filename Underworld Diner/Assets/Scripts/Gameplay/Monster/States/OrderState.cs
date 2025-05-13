using Gameplay.Dish;
using Interfaces;
using Interfaces.UI;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.States
{
    public class OrderState : BaseMonsterState
    {
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private MonsterAnimatorComponent _animatorComponent;
        [Inject] private NavMeshAgent _navMeshAgent;
        [Inject] private DishType _favoriteDish;
        [Inject] private IOrderIcon.Factory _orderIconFactory;
        [Inject] private MonsterStatusComponent _status;

        private IOrderIcon _currentOrderIcon;
        
        public override void Enter()
        {
            Debug.Log("I'm ordering " + _favoriteDish);
            
            _currentOrderIcon = _orderIconFactory.Create(_favoriteDish, _navMeshAgent.transform);
            _status.ExpectedDish = _favoriteDish;
            _status.FullOrder.Add(_status.ExpectedDish);

        }

        public override void OnTick()
        {
            if (!_aiComponent.MyChair.IsTaken)
            {
                _animatorComponent.StopSit();
                _aiComponent.ChangeState(MonsterState.Leave);
            }

            if (_aiComponent.MyChair.ExpectedDish == DishType.None)
            {
                _aiComponent.ChangeState(MonsterState.Eat);
            }
        }

        public override void Exit()
        {
            _status.ExpectedDish = DishType.None;
            _currentOrderIcon.Despawn();
        }
    }
}