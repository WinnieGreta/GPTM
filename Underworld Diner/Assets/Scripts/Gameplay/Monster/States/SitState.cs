using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class SitState : MonsterStateEntity
    {
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private MonsterAnimatorComponent _animatorComponent;
        public override void Initialize()
        {
            
        }

        public override void Enter()
        {
            Debug.Log("I'm sitting");
            if (_aiComponent.MyChair != null)
            {
                Debug.Log("I'm sitting on a chair");
                _animatorComponent.StartSit();
            }
            else
            {
                Debug.Log("I don't have a chair");
            }
        }

        public override void OnTick()
        {
            if (!_aiComponent.MyChair.IsTaken)
            {
                _animatorComponent.StopSit();
                _aiComponent.ChangeState(MonsterState.Leave);
            }
        }
    }
}