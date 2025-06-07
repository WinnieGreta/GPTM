using System;
using Gameplay.Monster.Abstract;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class EatState : BaseMonsterState
    {
        private const string MONSTER_FED_ID_TEMPLATE = "MonsterFed{0}";

        [Inject] private IStatisticsManager _statisticsManager;
        
        [Inject] private IAiComponent _aiComponent;
        [Inject] private IAnimatorComponent _animatorComponent;
        [Inject] private IScoringComponent _scoringComponent;
        [Inject] private MonsterServiceSettings _monsterServiceSettings;
        [Inject] private MonsterStatusComponent _statusComponent;
        [Inject] private MonsterType _monsterType;

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
            if (!_statusComponent.MyChair.IsTaken)
            {
                _animatorComponent.StopSit();
                _aiComponent.ChangeState(MonsterState.Leave);
                return;
            }
            
            _timerTime += Time.deltaTime;
            if (_timerTime > _eatingDowntime)
            {
                //Debug.Log("Eating downtime: " + _eatingDowntime + ", was eating for " + _timerTime);
                _timerTime -= _eatingDowntime;
                _aiComponent.FreeChairByMonster();
                _animatorComponent.StopSit();
                _statisticsManager.IncrementStatistics(String.Format(MONSTER_FED_ID_TEMPLATE, _monsterType.ToString()));
                _aiComponent.ChangeState(MonsterState.Leave);
            }
        }
        
        public override void Exit()
        {
            //Debug.Log("Exiting eat state");
            _scoringComponent.ScoreFood();
        }

    }
}