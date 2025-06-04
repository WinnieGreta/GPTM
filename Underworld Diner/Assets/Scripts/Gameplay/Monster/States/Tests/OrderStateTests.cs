using System.Collections.Generic;
using System.Reflection;
using Gameplay.Monster.Abstract;
using Interfaces;
using Interfaces.UI;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class OrderStateTests : ZenjectUnitTestFixture
    {
        private List<DishType> _favoriteDishes;
        private IOrderIcon _orderIcon;
        private IOrderIcon.Factory _orderIconFactory;
        private IAiComponent _aiComponent;
        private MonsterStatusComponent _statusComponent;
        private IChair _chair;
        private IAnimatorComponent _animatorComponent;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Container.Bind<INavigationComponent>()
                .FromInstance(Substitute.For<INavigationComponent>())
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
            
            _favoriteDishes = new List<DishType> { DishType.Burger };
            _orderIcon = Substitute.For<IOrderIcon>();
            _orderIconFactory = Substitute.For<IOrderIcon.Factory>();
            _aiComponent = Substitute.For<IAiComponent>();
            _chair = Substitute.For<IChair>();
            _statusComponent = new MonsterStatusComponent { MyChair = _chair };
            _animatorComponent = Substitute.For<IAnimatorComponent>();
            
            Container.BindInstance(_favoriteDishes);
            Container.BindInstance(_orderIconFactory);
            Container.BindInstance(_aiComponent);
            Container.BindInstance(_chair);
            Container.BindInstance(_animatorComponent);

        }

        [Test]
        public void OrderState_Enter()
        {
            Container.BindInstance(_statusComponent);
            // check if everything was initialized correctly and the order icon was spawned
            var orderState = Container.Resolve<OrderState>();
            orderState.Enter();
            _orderIconFactory.Received(1).Create(Arg.Any<DishType>(), 
                Arg.Any<Transform>());
            Assert.That(_statusComponent.ExpectedDish != DishType.None);
            Assert.That(_statusComponent.FullOrder.Count > 0);
            
        }

        [Test]
        public void OrderState_OnTick_LeaveIfChairFrees()
        {
            _chair.IsTaken.Returns(false);
            _statusComponent = new MonsterStatusComponent { MyChair = _chair };
            Container.BindInstance(_statusComponent);
            
            var orderState1 = Container.Resolve<OrderState>();
            orderState1.Enter();
            orderState1.OnTick();
            _animatorComponent.Received(1).StopSit();
            _aiComponent.Received(1).ChangeState(MonsterState.Leave);
            
            _animatorComponent.ClearReceivedCalls();
            _aiComponent.ClearReceivedCalls();
            
            _chair.IsTaken.Returns(true);
            _statusComponent = new MonsterStatusComponent { MyChair = _chair, Patience = 5};
            
            var orderState2 = Container.Resolve<OrderState>();
            orderState2.OnTick();
            _animatorComponent.DidNotReceive().StopSit();
            _aiComponent.DidNotReceive().ChangeState(MonsterState.Leave);
            
        }

        [Test]
        public void OrderState_OnTick_EatIfFoodGiven()
        {
            _chair.IsTaken.Returns(false);
            _chair.ExpectedDish.Returns(DishType.None);
            _statusComponent = new MonsterStatusComponent { MyChair = _chair };
            Container.BindInstance(_statusComponent);

            var orderState1 = Container.Resolve<OrderState>();
            
            orderState1.OnTick();
            _aiComponent.Received(1).ChangeState(MonsterState.Eat);
            
            _aiComponent.ClearReceivedCalls();

            Container.Unbind<MonsterStatusComponent>();
            _chair.IsTaken.Returns(true);
            _chair.ExpectedDish.Returns(DishType.Burger);
            _statusComponent = new MonsterStatusComponent { MyChair = _chair, Patience = 1 };
            Container.BindInstance(_statusComponent);
            
            var orderState2 = Container.Resolve<OrderState>();
            orderState2.OnTick();
            _aiComponent.DidNotReceive().ChangeState(MonsterState.Eat);
            
        }

        [Test]
        public void OrderState_OnTick_EatPatience()
        {
            _chair.IsTaken.Returns(true);
            _chair.ExpectedDish.Returns(DishType.Burger);
            _statusComponent = new MonsterStatusComponent { MyChair = _chair, Patience = 1 };
            Container.BindInstance(_statusComponent);
            
            var serviceSettings1 = new MonsterServiceSettings();
            typeof(MonsterServiceSettings).GetProperty("PatienceDropSpeed",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(serviceSettings1, 0.5f);
            Container.Rebind<MonsterServiceSettings>()
                .FromInstance(serviceSettings1)
                .AsCached();

            var statisticsManager = Container.Resolve<IStatisticsManager>();
            
            var orderState1 = Container.Resolve<OrderState>();
            orderState1.OnTick();
            statisticsManager.DidNotReceiveWithAnyArgs().IncrementStatistics(default);
            _aiComponent.DidNotReceive().FreeChairByMonster();
            _animatorComponent.DidNotReceive().StopSit();
            _aiComponent.DidNotReceive().ChangeState(MonsterState.Leave);
            
            statisticsManager.ClearReceivedCalls();
            _aiComponent.ClearReceivedCalls();
            _animatorComponent.ClearReceivedCalls();
            
            Container.Unbind<MonsterStatusComponent>();
            _statusComponent = new MonsterStatusComponent { MyChair = _chair, Patience = 0 };
            Container.BindInstance(_statusComponent);
            
            var orderState2 = Container.Resolve<OrderState>();
            orderState2.OnTick();
            statisticsManager.ReceivedWithAnyArgs(1).IncrementStatistics(default);
            _aiComponent.Received(1).FreeChairByMonster();
            _animatorComponent.Received(1).StopSit();
            _aiComponent.Received(1).ChangeState(MonsterState.Leave);
            
        }

        [Test]
        public void OrderState_Exit()
        {
            _statusComponent = new MonsterStatusComponent { ExpectedDish = DishType.Burger };
            Container.BindInstance(_statusComponent);

            _orderIconFactory.ClearReceivedCalls();
            var orderState = Container.Resolve<OrderState>();
            
            var orderIcon = typeof(OrderState).GetField("_currentOrderIcon",
                BindingFlags.Instance | BindingFlags.NonPublic);
            orderIcon?.SetValue(orderState, _orderIcon);
            
            orderState.Exit();
            Assert.That(_statusComponent.ExpectedDish, Is.EqualTo(DishType.None));
            _orderIcon.Received().Despawn();
            
        }
    }
}