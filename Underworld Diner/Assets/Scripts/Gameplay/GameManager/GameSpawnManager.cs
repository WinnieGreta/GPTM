using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.GameManager
{
    public class GameSpawnManager
    {
        [Inject] private MonsterSpawnSettings _monsterSpawnSettings;
        [Inject] private IMonster.Factory _monsterFactory;
        [Inject] private Transform _monsterSpawnAnchor;
        private float _spawnPeriod;
        private float _spawnTimerTime;
        
        public void OnInitialize()
        {
            _spawnTimerTime = 0;
            _spawnPeriod = _monsterSpawnSettings.SpawnPeriod;
        }
        
        public void OnFixedTick()
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
            //Debug.Log("Spawn MONSTER!!!!");
            _monsterFactory.Create(MonsterType.Skeleton, _monsterSpawnAnchor);
        }
        
    }
}