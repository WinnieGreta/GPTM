using System;
using Gameplay.Monster.States;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class EnterStateTests : ZenjectUnitTestFixture
    {
        /*[Inject] private IStatisticsManager _statisticsManager;
        
        [Inject] private MonsterNavigationComponent _navigation;
        [Inject] private MonsterAIComponent _aiComponent;
        [Inject] private MonsterType _monsterType;
        [Inject] private Transform _transform;*/
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            SignalBusInstaller.Install(Container);
            Container.BindInstance(MonsterType.Skeleton).AsSingle();
            Container.BindInstance(new MonsterNavigationComponent()).AsSingle();
            Container.BindInstance(new MonsterAIComponent()).AsSingle();
            Container.Bind<Transform>().FromNewComponentOnNewGameObject().AsSingle();

        }
        
        [Test]
        public void EnterState_Enter()
        {
            var statisticsManager = Substitute.For<IStatisticsManager>();
            Container.Bind<EnterState>().AsSingle();
            Container.BindInstance(statisticsManager).AsSingle();
            var enterState = Container.Resolve<EnterState>();
            enterState.Enter();
            statisticsManager.Received(1).IncrementStatistics(String.Format(EnterState.MONSTER_ENTER_ID_TEMPLATE, MonsterType.Skeleton.ToString()));
            statisticsManager.ReceivedWithAnyArgs(1).IncrementStatistics(default);
            //statisticsManager.DidNotReceive().IncrementStatistics(String.Format(EnterState.MONSTER_ENTER_ID_TEMPLATE, MonsterType.Skeleton.ToString()));
        }

        [Test]
        public void EnterState_OnTick()
        {
            
        }
    }
}