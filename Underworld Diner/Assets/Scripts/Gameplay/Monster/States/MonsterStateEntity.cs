using Zenject;

namespace Gameplay.Monster.States
{
    public enum MonsterState
    {
        Enter,
        GoSit,
        Sit,
        Leave,
        Null
    }
    
    public abstract class MonsterStateEntity : IInitializable
    {
        public virtual void Initialize()
        {
            
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual void OnTick()
        {
            
        }
        
        public class Factory : PlaceholderFactory<MonsterState, MonsterStateEntity>
        {
            
        }
    }
}