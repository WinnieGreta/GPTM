﻿using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class SitState : BaseMonsterState
    {
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private MonsterAnimatorComponent _animatorComponent;
        [Inject] private MonsterServiceSettings _monsterServiceSettings;

        private float _orderDowntime;
        private float _timerTime;
        public override void Initialize()
        {
            
        }

        public override void Enter()
        {
            if (_aiComponent.MyChair != null)
            {
                _orderDowntime = _monsterServiceSettings.OrderDowntime;
                _timerTime = 0;
                //Debug.Log("I'm sitting on a chair");
                _animatorComponent.StartSit(_aiComponent.MyChair.IsFacingRight);
            }
            else
            {
                //Debug.Log("I don't have a chair");
            }
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
            if (_timerTime > _orderDowntime)
            {
                Debug.Log("Order downtime: " + _orderDowntime + ", was waiting for " + _timerTime);
                _timerTime -= _orderDowntime;
                _aiComponent.ChangeState(MonsterState.Order);
            }
        }
    }
}