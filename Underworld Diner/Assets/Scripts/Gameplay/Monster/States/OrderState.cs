using System.Collections.Generic;
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
        [Inject] private List<DishType> _favoriteDishes;
        [Inject] private IOrderIcon.Factory _orderIconFactory;
        [Inject] private MonsterStatusComponent _status;
        [Inject] private MonsterServiceSettings _settings;

        private IOrderIcon _currentOrderIcon;
        
        public override void Enter()
        {
            DishType orderedDish = SelectDish();
            Debug.Log("I'm ordering " + orderedDish);
            _currentOrderIcon = _orderIconFactory.Create(orderedDish, _navMeshAgent.transform);
            _status.ExpectedDish = orderedDish;
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
            
            if (_status.Patience > 0)
            {
                _status.Patience -= _settings.PatienceDropSpeed * Time.deltaTime;
            }
            else
            {
                Debug.Log("I'm out of patience!");
                _animatorComponent.StopSit();
                _aiComponent.ChangeState(MonsterState.Leave);
            }
        }

        public override void Exit()
        {
            _status.ExpectedDish = DishType.None;
            _currentOrderIcon.Despawn();
        }

        private DishType SelectDish()
        {
            int i = Random.Range(0, _favoriteDishes.Count);
            return _favoriteDishes[i];
        }
    }
}