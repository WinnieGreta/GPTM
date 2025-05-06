﻿using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.States
{
    public class EnterState : MonsterStateEntity
    {
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        private bool _isDestinationSet;
        
        public override void Initialize()
        {
            
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            Debug.Log("EnterState exited");
        }
        

        public override void OnTick()
        {
            if (!_isDestinationSet)
            {
                _isDestinationSet = true;
                _navigation.ProcessMovement(Vector2.zero);
            }
            
            if (_navigation.HasReachedDestination())
            {
                _aiComponent.ChangeState(MonsterState.GoSit);
            }
        }
    }
}