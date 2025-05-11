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
        IDish ExpectedDish { get; }
        void Serve(IDish dish);
        
        public class Factory : PlaceholderFactory<MonsterType, Transform, IMonster>
        {
            
        }

    }
}