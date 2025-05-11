using UnityEngine;
using Zenject;

namespace Interfaces
{
    public enum MonsterType
    {
        Skeleton,
        Ork
    }
    public interface IMonster
    {
        DishType ExpectedDish { get; }
        void Serve(DishType dish);
        
        public class Factory : PlaceholderFactory<MonsterType, Transform, IMonster>
        {
            
        }

    }
}