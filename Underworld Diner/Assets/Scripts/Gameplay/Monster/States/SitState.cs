using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States
{
    public class SitState : MonsterStateEntity
    {
        public override void Initialize()
        {
            
        }

        public override void Enter()
        {
            Debug.Log("I'm sitting");
        }
    }
}