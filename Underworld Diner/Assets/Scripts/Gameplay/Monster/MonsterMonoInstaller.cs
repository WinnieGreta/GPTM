using Signals;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterMonoInstaller : MonoInstaller
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public override void InstallBindings()
        {
            Container.BindInstance(_animator).AsSingle();
            Container.BindInstance(_navMeshAgent).AsSingle();
            Container.BindInstance(_spriteRenderer).AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterAnimatorComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterNavigationComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterAIComponent>().AsSingle();
            Container.DeclareSignal<OnSpawnedSignal>();
            Container.DeclareSignal<OnDespawnedSignal>();
        }
    }
}