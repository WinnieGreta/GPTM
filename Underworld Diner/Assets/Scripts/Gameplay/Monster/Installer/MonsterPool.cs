using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.Installer
{
    public class MonsterPool : MonoPoolableMemoryPool<Transform, MonsterFacade>
    {
        public MonsterType Type { get; private set; }
        
        [Inject]
        public void OnInject(MonsterType type)
        {
            Type = type;
        }
        
    }
}