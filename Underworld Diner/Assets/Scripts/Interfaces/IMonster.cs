﻿using UnityEngine;
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
        public class Factory : PlaceholderFactory<MonsterType, Transform, IMonster>
        {
            
        }
    }
}