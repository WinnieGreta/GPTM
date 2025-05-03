using Interfaces;
using UnityEngine;
using Zenject;


namespace Gameplay.GameManager
{
    public class GameManager : IGameManager, IFixedTickable, IInitializable
    {
        [Inject] private MonsterSpawnSettings _monsterSpawnSettings;
        private float _spawnPeriod;
        private float _spawnTimerTime;
        public void Initialize()
        {
            _spawnTimerTime = 0;
            _spawnPeriod = _monsterSpawnSettings.SpawnPeriod;
        }
        
        public void FixedTick()
        {
            _spawnTimerTime += Time.deltaTime;
            if (_spawnTimerTime > _spawnPeriod)
            {
                SpawnMonster();
                _spawnTimerTime -= _spawnPeriod;
            }
        }

        private void SpawnMonster()
        {
            Debug.Log("Spawn MONSTER!!!!");
        }
    }
}