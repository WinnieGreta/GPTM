﻿using UnityEngine;
using Zenject;

namespace Interfaces.UI
{
    public interface IOrderIcon : IDespawnable
    {
        public class Factory : PlaceholderFactory<IDish, Transform, IOrderIcon>
        {
            
        }
    }
}