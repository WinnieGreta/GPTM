using Interfaces;
using UnityEngine;
using Zenject;


namespace Gameplay.GameManager
{
    public class GameManager : IGameManager, IFixedTickable, IInitializable
    {
        [Inject] private GameSpawnManager _spawnManager;
        public void Initialize()
        {
           _spawnManager.OnInitialize();
        }
        
        public void FixedTick()
        {
            _spawnManager.OnFixedTick();
        }
    }
}