using System.Collections.Generic;
using System.Reflection;
using Gameplay.Monster.Abstract;
using Interfaces;
using Interfaces.UI;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class OrderTests : ZenjectUnitTestFixture
    {

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Container.Bind<INavigationComponent>()
                .FromInstance(Substitute.For<INavigationComponent>())
                .AsCached();
            Container.Bind<IAiComponent>()
                .FromInstance(Substitute.For<IAiComponent>())
                .AsCached();
            Container.Bind<IAnimatorComponent>()
                .FromInstance(Substitute.For<IAnimatorComponent>())
                .AsCached();
            Container.Bind<IStatisticsManager>()
                .FromInstance(Substitute.For<IStatisticsManager>())
                .AsCached();
            Container.Bind<MonsterServiceSettings>()
                .FromInstance(Substitute.For<MonsterServiceSettings>())
                .AsCached();
            Container.Bind<NavMeshAgent>()
                .FromNewComponentOnNewGameObject()
                .AsCached();
            Container.BindInstance(MonsterType.Skeleton).AsSingle();
            Container.Bind<OrderState>().AsTransient();
            
        }

        [Test]
        public void OrderState_Enter()
        {
            var favoriteDishes = new List<DishType> { DishType.Burger, DishType.Pudding, DishType.Pudding };
            Container.Rebind<List<DishType>>()
                .FromInstance(favoriteDishes)
                .AsCached();
            
            var orderIcon = Substitute.For<IOrderIcon>();
            
            var orderIconFactory = Substitute.For<IOrderIcon.Factory>();
            orderIconFactory.Create(
                    Arg.Is<DishType>(d => favoriteDishes.Contains(d)),
                    Arg.Any<Transform>())
                .Returns(orderIcon);
            
            Container.BindInstance(orderIconFactory);

            var statusComponent = Substitute.For<MonsterStatusComponent>();
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent)
                .AsCached();
            
            var navMeshAgent = Container.Resolve<NavMeshAgent>();
            
            // check if everything was initialized correctly and the order icon was spawned
            var orderState = Container.Resolve<OrderState>();
            orderState.Enter();
            orderIconFactory.Received(1).Create(Arg.Is<DishType>(x => favoriteDishes.Contains(x)), 
                Arg.Any<Transform>());
            statusComponent.Received(1).ExpectedDish = Arg.Is<DishType>(x => favoriteDishes.Contains(x));
            statusComponent.Received(1).FullOrder.Add(Arg.Is<DishType>(x => favoriteDishes.Contains(x)));
            
        }

        [Test]
        public void OrderState_OnTick_LeaveIfChairFrees()
        {
            var favoriteDishes = new List<DishType> { DishType.Burger, DishType.Pudding, DishType.Pudding };
            Container.Rebind<List<DishType>>()
                .FromInstance(favoriteDishes)
                .AsCached();
            
            var chair1 = Substitute.For<IChair>();
            chair1.IsTaken.Returns(false);
            
            var statusComponent1 = new MonsterStatusComponent { MyChair = chair1};
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent1)
                .AsCached();
            
            var animatorComponent = Substitute.For<IAnimatorComponent>();
            Container.Rebind<IAnimatorComponent>()
                .FromInstance(animatorComponent)
                .AsCached();
            
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var orderIcon = Substitute.For<IOrderIcon>();
            
            var orderIconFactory = Substitute.For<IOrderIcon.Factory>();
            orderIconFactory.Create(
                    Arg.Is<DishType>(d => favoriteDishes.Contains(d)),
                    Arg.Any<Transform>())
                .Returns(orderIcon);
            
            Container.BindInstance(orderIconFactory);
            
            var orderState1 = Container.Resolve<OrderState>();
            orderState1.OnTick();
            animatorComponent.Received(1).StopSit();
            aiComponent.Received(1).ChangeState(MonsterState.Leave);
            
            animatorComponent.ClearReceivedCalls();
            aiComponent.ClearReceivedCalls();
            
            var chair2 = Substitute.For<IChair>();
            chair2.IsTaken.Returns(true);
            
            var statusComponent2 = new MonsterStatusComponent { MyChair = chair2, Patience = 5 };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent2)
                .AsCached();
            
            var orderState2 = Container.Resolve<OrderState>();
            orderState2.OnTick();
            animatorComponent.DidNotReceive().StopSit();
            aiComponent.DidNotReceive().ChangeState(MonsterState.Leave);

        }

        [Test]
        public void OrderState_OnTick_EatIfFoodGiven()
        {
            var favoriteDishes = new List<DishType> { DishType.Burger, DishType.Pudding, DishType.Pudding };
            Container.Rebind<List<DishType>>()
                .FromInstance(favoriteDishes)
                .AsCached();
            
            var orderIcon = Substitute.For<IOrderIcon>();
            
            var orderIconFactory = Substitute.For<IOrderIcon.Factory>();
            orderIconFactory.Create(
                    Arg.Is<DishType>(d => favoriteDishes.Contains(d)),
                    Arg.Any<Transform>())
                .Returns(orderIcon);
            
            Container.BindInstance(orderIconFactory);
            
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var chair1 = Substitute.For<IChair>();
            chair1.IsTaken.Returns(true);
            chair1.ExpectedDish.Returns(DishType.None);
            
            var statusComponent1 = new MonsterStatusComponent { MyChair = chair1, Patience = 1 };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent1)
                .AsCached();
            
            var orderState1 = Container.Resolve<OrderState>();
            orderState1.OnTick();
            aiComponent.Received(1).ChangeState(MonsterState.Eat);
            
            aiComponent.ClearReceivedCalls();
            
            var chair2 = Substitute.For<IChair>();
            chair2.IsTaken.Returns(true);
            chair2.ExpectedDish.Returns(DishType.Burger);
            
            var statusComponent2 = new MonsterStatusComponent { MyChair = chair2, Patience = 1 };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent2)
                .AsCached();
            
            var orderState2 = Container.Resolve<OrderState>();
            orderState2.OnTick();
            aiComponent.DidNotReceive().ChangeState(MonsterState.Eat);
            
        }

        [Test]
        public void OrderState_OnTick_EatPatience()
        {
            var favoriteDishes = new List<DishType> { DishType.Burger, DishType.Pudding, DishType.Pudding };
            Container.Rebind<List<DishType>>()
                .FromInstance(favoriteDishes)
                .AsCached();
            
            var orderIcon = Substitute.For<IOrderIcon>();
            
            var orderIconFactory = Substitute.For<IOrderIcon.Factory>();
            orderIconFactory.Create(
                    Arg.Is<DishType>(d => favoriteDishes.Contains(d)),
                    Arg.Any<Transform>())
                .Returns(orderIcon);
            
            Container.BindInstance(orderIconFactory);
            
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var chair1 = Substitute.For<IChair>();
            chair1.IsTaken.Returns(true);
            chair1.ExpectedDish.Returns(DishType.Burger);

            const float STARTING_PATIENCE_1 = 1f;
            const float PATIENCE_DROP_1 = 0.5f;
            
            var statusComponent1 = new MonsterStatusComponent { MyChair = chair1, Patience = STARTING_PATIENCE_1 };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent1)
                .AsCached();
            
            var serviceSettings1 = new MonsterServiceSettings();
            typeof(MonsterServiceSettings).GetProperty("PatienceDropSpeed",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(serviceSettings1, PATIENCE_DROP_1);
            Container.Rebind<MonsterServiceSettings>()
                .FromInstance(serviceSettings1)
                .AsCached();

            var statisticsManager = Substitute.For<IStatisticsManager>();
            Container.Rebind<IStatisticsManager>()
                .FromInstance(statisticsManager)
                .AsCached();
            
            var animatorComponent = Substitute.For<IAnimatorComponent>();
            Container.Rebind<IAnimatorComponent>()
                .FromInstance(animatorComponent)
                .AsCached();
            
            var orderState1 = Container.Resolve<OrderState>();
            orderState1.OnTick();
            statisticsManager.DidNotReceiveWithAnyArgs().IncrementStatistics(default);
            aiComponent.DidNotReceive().FreeChairByMonster();
            animatorComponent.DidNotReceive().StopSit();
            aiComponent.DidNotReceive().ChangeState(MonsterState.Leave);
            
            statisticsManager.ClearReceivedCalls();
            aiComponent.ClearReceivedCalls();
            animatorComponent.ClearReceivedCalls();
            
            var statusComponent2 = new MonsterStatusComponent { MyChair = chair1, Patience = 0 };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent2)
                .AsCached();
            
            var orderState2 = Container.Resolve<OrderState>();
            orderState2.OnTick();
            statisticsManager.ReceivedWithAnyArgs(1).IncrementStatistics(default);
            aiComponent.Received(1).FreeChairByMonster();
            animatorComponent.Received(1).StopSit();
            aiComponent.Received(1).ChangeState(MonsterState.Leave);
        }

        [Test]
        public void OrderState_Exit()
        {
            var statusComponent1 = new MonsterStatusComponent { ExpectedDish = DishType.Burger };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent1)
                .AsCached();
            
            var favoriteDishes = new List<DishType> { DishType.Burger, DishType.Pudding, DishType.Pudding };
            Container.Rebind<List<DishType>>()
                .FromInstance(favoriteDishes)
                .AsCached();
            
            var orderIcon = Substitute.For<IOrderIcon>();
            
            var orderIconFactory = Substitute.For<IOrderIcon.Factory>();
            orderIconFactory.Create(
                    Arg.Is<DishType>(d => favoriteDishes.Contains(d)),
                    Arg.Any<Transform>())
                .Returns(orderIcon);
            
            Container.BindInstance(orderIconFactory);
            
            var orderState1 = Container.Resolve<OrderState>();
            typeof(OrderState).GetProperty("_currentOrderIcon",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(orderState1, orderIcon);
            orderState1.Exit();
            Assert.Equals(statusComponent1.ExpectedDish, DishType.None);
        }
    }
}