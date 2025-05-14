using Interfaces.UI;
using Signals;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterPatienceComponent : IInitializable
    {
        [Inject] private MonsterServiceSettings _serviceSettings;
        [Inject] private SignalBus _signalBus;
        [Inject] private MonsterStatusComponent _statusComponent;
        [Inject] private IPatienceMeter.Factory _patienceMeterFactory;
        [Inject] private NavMeshAgent _navMeshAgent;

        private IPatienceMeter _patienceMeter;


        public void Initialize()
        {
            _signalBus.Subscribe<OnSpawnedSignal>(OnSpawn);
            _signalBus.Subscribe<OnDespawnedSignal>(OnDespawn);
        }

        private void OnSpawn()
        {
            _patienceMeter = _patienceMeterFactory.Create(_navMeshAgent.transform, (int)_serviceSettings.StartingPatience);
        }


        public void UpdatePatience()
        {
            _patienceMeter.UpdatePatienceMeter(_statusComponent.Patience, _navMeshAgent.transform);
        }

        private void OnDespawn()
        {
            _patienceMeter.Despawn();
        }

    }
}