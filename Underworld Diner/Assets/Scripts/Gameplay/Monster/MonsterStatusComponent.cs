﻿using System.Collections.Generic;
using Interfaces;

namespace Gameplay.Monster
{
    internal class MonsterStatusComponent
    {
        public DishType ExpectedDish { get; set; }

        public List<DishType> FullOrder { get; } = new();
        
        public float Patience { get; set; }
    }
}