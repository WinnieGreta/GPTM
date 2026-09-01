using System.Collections.Generic;
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
        private readonly Dictionary<MonsterType, float> _spawnTimers = new();
        
        public void OnInitialize()
        {
            foreach (var config in _monsterSpawnSettings.SpawnConfig)
            {
                _spawnTimers[config.Type] = 0;
            }
        }
        
        public void OnFixedTick()
        {
            foreach (var config in _monsterSpawnSettings.SpawnConfig)
            {
                _spawnTimers[config.Type] += Time.deltaTime;

                if (_spawnTimers[config.Type] >= config.SpawnPeriod)
                {
                    SpawnMonster(config.Type);
                    _spawnTimers[config.Type] -= config.SpawnPeriod;
                }
            }
        }

        private void SpawnMonster(MonsterType monsterType)
        {
            //Debug.Log("Spawn " + monsterType);
            _monsterFactory.Create(monsterType, _monsterSpawnAnchor);
        }

    }
}