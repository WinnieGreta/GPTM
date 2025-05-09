using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterFactory : IFactory<MonsterType, Transform, IMonster>
    {
        readonly Dictionary<MonsterType, MonsterPool> _pools;
        
        [Inject]
        public MonsterFactory(List<MonsterPool> pools)
        {
            _pools = pools.ToDictionary(x => x.Type, x => x);
        }
        public IMonster Create(MonsterType monsterType, Transform anchorTransform)
        {
            if (!_pools.TryGetValue(monsterType, out var pool))
                throw new ArgumentException($"No pool for {monsterType}");
            return pool.Spawn(anchorTransform);
        }
    }
}