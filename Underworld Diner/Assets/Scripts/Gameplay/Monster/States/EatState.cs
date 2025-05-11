using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class EatState : BaseMonsterState
    {
        [Inject] private MonsterAIComponent _aiComponent;
        public override void Enter()
        {
            
            Debug.Log("I'm EATING " + _aiComponent.MyChair.GetDishImEating().DishName);

        }
        
    }
}