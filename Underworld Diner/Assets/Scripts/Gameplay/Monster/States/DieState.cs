using Gameplay.Monster.Abstract;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class DieState : BaseMonsterState
    {
        [Inject] private IAiComponent _aiComponent;
        [Inject] private INavigationComponent _navigationComponent;
        [Inject] private IAnimatorComponent _animatorComponent;
        [Inject] private IDespawnable _despawnable;
        
        private float _corpseTime;
        private float _timerTime;
        
        public override void Enter()
        {
            Debug.Log("I'm dead");
            _navigationComponent.StopOnDeath();
            _aiComponent.FreeChairByMonster();
            _animatorComponent.DeathAnimation();
            _corpseTime = 3;
            _timerTime = 0;
        }

        public override void OnTick()
        {
            _timerTime += Time.deltaTime;
            if (_timerTime >= _corpseTime)
            {
                _aiComponent.ChangeState(MonsterState.Null);
                _despawnable.Despawn();
            }
        }

        public override void Exit()
        {
            Debug.Log("Dead and despawned");
        }
    }
}