using System;
using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class EnterStateTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            SignalBusInstaller.Install(Container);
            Container.BindInstance(MonsterType.Skeleton).AsSingle();
            Container.Bind<INavigationComponent>()
                .FromInstance(Substitute.For<INavigationComponent>())
                .AsCached();
            Container.Bind<IAiComponent>()
                .FromInstance(Substitute.For<IAiComponent>())
                .AsCached();
            Container.Bind<IStatisticsManager>()
                .FromInstance(Substitute.For<IStatisticsManager>())
                .AsCached();
            Container.Bind<IAnimatorComponent>()
                .FromInstance(Substitute.For<IAnimatorComponent>())
                .AsCached();
            Container.Bind<Transform>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<EnterState>().AsSingle();

        }
        
        [Test]
        public void EnterState_Enter()
        {
            // test setup
            var animatorComponent = Substitute.For<IAnimatorComponent>();
            //Container.Unbind<IAnimatorComponent>();
            Container.Rebind<IAnimatorComponent>()
                .FromInstance(animatorComponent)
                .AsCached();
            
            var statisticsManager = Substitute.For<IStatisticsManager>();
            //Container.Unbind<IStatisticsManager>();
            Container.Rebind<IStatisticsManager>()
                .FromInstance(statisticsManager)
                .AsCached();
            
            var enterState = Container.Resolve<EnterState>();
            enterState.Enter();
            
            animatorComponent.Received(1).Restart();
            
            // does statistics manager increment statistics
            statisticsManager.Received(1).IncrementStatistics(String.Format(EnterState.MONSTER_ENTER_ID_TEMPLATE, MonsterType.Skeleton.ToString()));
            statisticsManager.ReceivedWithAnyArgs(1).IncrementStatistics(default);
            
        }

        [Test]
        public void EnterState_OnTick_SetDestination()
        {
            var navigationComponent = Substitute.For<INavigationComponent>();
            navigationComponent.HasReachedDestination().Returns(false);
            Container.Rebind<INavigationComponent>()
                .FromInstance(navigationComponent)
                .AsCached();
            
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var enterState = Container.Resolve<EnterState>();
            
            enterState.OnTick();
            navigationComponent.ReceivedWithAnyArgs(1).ProcessMovement(default);
            
            enterState.OnTick();
            navigationComponent.ReceivedWithAnyArgs(1).ProcessMovement(default);
        }

        [Test]
        public void EnterState_OnTick_ChangeStateGoSit()
        {
            var navigationComponent = Substitute.For<INavigationComponent>();
            navigationComponent.HasReachedDestination()
                .Returns(false, true);
            Container.Rebind<INavigationComponent>()
                .FromInstance(navigationComponent)
                .AsCached();
            
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var enterState = Container.Resolve<EnterState>();
            
            enterState.OnTick();
            aiComponent.DidNotReceive().ChangeState(MonsterState.GoSit);
            
            enterState.OnTick();
            aiComponent.Received(1).ChangeState(MonsterState.GoSit);
        }
    }
}