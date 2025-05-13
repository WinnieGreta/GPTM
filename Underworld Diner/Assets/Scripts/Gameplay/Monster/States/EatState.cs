using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class EatState : BaseMonsterState
    {
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private MonsterServiceSettings _monsterServiceSettings;
        [Inject] private MonsterAnimatorComponent _animatorComponent;
        [Inject] private MonsterScoringComponent _scoringComponent;
        [Inject] private MonsterStatusComponent _statusComponent;

        private float _eatingDowntime;
        private float _timerTime;
        
        public override void Enter()
        {
            //Debug.Log("I'm EATING " + _aiComponent.MyChair.GetDishImEating().DishName);
            _eatingDowntime = _monsterServiceSettings.EatingDowntime;
            _timerTime = 0;
        }

        public override void OnTick()
        {
            if (!_aiComponent.MyChair.IsTaken)
            {
                _animatorComponent.StopSit();
                _aiComponent.ChangeState(MonsterState.Leave);
                return;
            }
            
            _timerTime += Time.deltaTime;
            if (_timerTime > _eatingDowntime)
            {
                Debug.Log("Eating downtime: " + _eatingDowntime + ", was eating for " + _timerTime);
                _timerTime -= _eatingDowntime;
                _aiComponent.MyChair.FreeChair();
                _animatorComponent.StopSit();
                _aiComponent.ChangeState(MonsterState.Leave);
            }
        }
        
        public override void Exit()
        {
            Debug.Log("Exiting eat state");
            _scoringComponent.ScoreFood();
        }

    }
}