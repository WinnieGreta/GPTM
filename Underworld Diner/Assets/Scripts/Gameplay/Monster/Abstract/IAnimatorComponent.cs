namespace Gameplay.Monster.Abstract
{
    internal interface IAnimatorComponent
    {
        void StartSit(bool isFacingRight);
        void StopSit();
        void DeathAnimation();
        void Restart();
    }
}