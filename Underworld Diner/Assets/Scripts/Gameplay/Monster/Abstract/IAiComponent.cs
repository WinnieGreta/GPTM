using Interfaces;

namespace Gameplay.Monster.Abstract
{
    internal interface IAiComponent
    {
        void ChangeState(MonsterState monsterState);
        void TakeChairByMonster(IChair chair);
        void FreeChairByMonster();
    }
}