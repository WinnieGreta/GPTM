using Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterAIComponent : IInitializable
    {
        [Inject] private MonsterNavigationComponent _navigator;
        [Inject] private SignalBus _signalBus;
        
        public void Initialize()
        {
            
        }

        [Inject]
        private void OnInject()
        {
            _signalBus.Subscribe<OnSpawnedSignal>(TestMove);
        }

        public void TestMove()
        {
            _navigator.ProcessMovement(Vector2.zero);
        }
    }
}