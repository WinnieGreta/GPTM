﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Gameplay.Monster.Abstract;
using Gameplay.Monster.States;
using Interfaces;
using Signals;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.Installer
{
    public class MonsterMonoInstaller : MonoInstaller
    {
        [field:SerializeField] public MonsterType MonsterType { get; private set; }
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<DishType> _favoriteDishes;

        public override void InstallBindings()
        {
            Container.BindInstance(_animator).AsSingle();
            Container.BindInstance(_navMeshAgent).AsSingle();
            Container.BindInstance(_spriteRenderer).AsSingle();
            Container.BindInstance(MonsterType).AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterAnimatorComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterNavigationComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterAIComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterStatusComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterScoringComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterPatienceComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterFacade>().FromComponentOnRoot().AsSingle();
            Container.DeclareSignal<OnSpawnedSignal>();
            Container.DeclareSignal<OnDespawnedSignal>();
            Container.BindFactory<MonsterState, BaseMonsterState, BaseMonsterState.Factory>()
                .FromMethod(CreateMonsterState);
            
            Container.BindInstance(_favoriteDishes).AsSingle();
        }
        
        [ExcludeFromCodeCoverage]
        private BaseMonsterState CreateMonsterState(DiContainer container, MonsterState monsterState)
        {
            switch (monsterState)
            {
                case MonsterState.Enter:
                    return container.Instantiate<EnterState>();
                case MonsterState.GoSit:
                    return container.Instantiate<GoSitState>();
                case MonsterState.Sit:
                    return container.Instantiate<SitState>();
                case MonsterState.Order:
                    return container.Instantiate<OrderState>();
                case MonsterState.Eat:
                    return container.Instantiate<EatState>();
                case MonsterState.Leave:
                    return container.Instantiate<LeaveState>();
                case MonsterState.Die:
                    return container.Instantiate<DieState>();
                case MonsterState.Null:
                    return container.Instantiate<NullState>();
            }
            return null;
        }
    }
}