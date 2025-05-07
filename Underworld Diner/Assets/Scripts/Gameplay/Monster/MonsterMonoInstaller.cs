using System;
using Gameplay.Monster.States;
using Interfaces;
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
            Container.BindInterfacesAndSelfTo<MonsterFacade>().FromComponentOnRoot().AsSingle();
            Container.DeclareSignal<OnSpawnedSignal>();
            Container.DeclareSignal<OnDespawnedSignal>();
            Container.BindFactory<MonsterState, MonsterStateEntity, MonsterStateEntity.Factory>()
                .FromMethod(CreateMonsterState);
        }

        private MonsterStateEntity CreateMonsterState(DiContainer container, MonsterState monsterState)
        {
            switch (monsterState)
            {
                case MonsterState.Enter:
                    return container.Instantiate<EnterState>();
                case MonsterState.GoSit:
                    return container.Instantiate<GoSitState>();
                case MonsterState.Sit:
                    return container.Instantiate<SitState>();
                case MonsterState.Leave:
                    return container.Instantiate<LeaveState>();
                case MonsterState.Null:
                    return container.Instantiate<NullState>();
            }
            throw new Exception("No monster state!");
        }
    }
}