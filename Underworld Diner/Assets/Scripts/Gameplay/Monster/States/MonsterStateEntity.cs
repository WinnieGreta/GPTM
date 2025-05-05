using Zenject;

namespace Gameplay.Monster.States
{
    public enum MonsterState
    {
        Enter,
        Sit,
        Leave
    }
    
    public abstract class MonsterStateEntity : IInitializable
    {
        public virtual void Initialize()
        {
            
        }

        public virtual void Start()
        {
            
        }

        public virtual void Dispose()
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