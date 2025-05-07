using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class OrderState : BaseMonsterState
    {
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private MonsterAnimatorComponent _animatorComponent;
        public override void Enter()
        {
            Debug.Log("I'm ordering");
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