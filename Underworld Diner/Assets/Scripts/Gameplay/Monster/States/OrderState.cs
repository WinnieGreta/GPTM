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
        [Inject] private DishRecipe _favoriteDish;
        [Inject] private IOrderIcon.Factory _orderIconFactory;

        private IOrderIcon _currentOrderIcon;
        
        public override void Enter()
        {
            Debug.Log("I'm ordering " + _favoriteDish.DishName);
            
            _currentOrderIcon = _orderIconFactory.Create(_favoriteDish, _navMeshAgent.transform);

        }

        public override void OnTick()
        {
            if (!_aiComponent.MyChair.IsTaken)
            {
                _animatorComponent.StopSit();
                _aiComponent.ChangeState(MonsterState.Leave);
            }
        }

        public override void Exit()
        {
            _currentOrderIcon.Despawn();
        }
    }
}