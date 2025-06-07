﻿using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace Gameplay.Monster.Abstract
{
    public enum MonsterState
    {
        Enter,
        GoSit,
        Sit,
        Order,
        Eat,
        Leave,
        Die,
        Null
    }
    
    // default implementations are empty methods
    [ExcludeFromCodeCoverage]
    public abstract class BaseMonsterState : IInitializable
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
        
        public class Factory : PlaceholderFactory<MonsterState, BaseMonsterState>
        {
            
        }
    }
}