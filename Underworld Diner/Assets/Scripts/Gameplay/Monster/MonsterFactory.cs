using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterFactory : IFactory<MonsterType, Transform, IMonster>
    {
        public IMonster Create(MonsterType monsterType, Transform monsterTransform)
        {
            throw new System.NotImplementedException();
        }
    }
}