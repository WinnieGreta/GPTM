using System.Reflection;
using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class SitStateTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Container.Bind<IAiComponent>()
                .FromInstance(Substitute.For<IAiComponent>())
                .AsCached();
            Container.Bind<IAnimatorComponent>()
                .FromInstance(Substitute.For<IAnimatorComponent>())
                .AsCached();
            Container.Bind<SitState>().AsTransient();
        }

        [Test]
        public void SitState_Enter()
        {
            var animatorComponent = Substitute.For<IAnimatorComponent>();
            Container.Rebind<IAnimatorComponent>()
                .FromInstance(animatorComponent)
                .AsCached();

            var statusComponent1 = new MonsterStatusComponent { MyChair = null };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent1)
                .AsCached();

            // needs reflection as order downtime has private setter
            var serviceSettings = new MonsterServiceSettings();
            typeof(MonsterServiceSettings).GetProperty("OrderDowntime",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(serviceSettings, 5f);
            Container.Rebind<MonsterServiceSettings>()
                .FromInstance(serviceSettings)
                .AsCached();
            
            // no chair means no sitting for monster
            var sitState1 = Container.Resolve<SitState>();
            sitState1.Enter();
            animatorComponent.DidNotReceiveWithAnyArgs().StartSit(default);
            
            animatorComponent.ClearReceivedCalls();

            var chair = Substitute.For<IChair>();
            chair.IsFacingRight.Returns(true);
    
            // if chair fits monster sits
            var statusComponent2 = new MonsterStatusComponent { MyChair = chair };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent2)
                .AsCached();
            
            var sitState2 = Container.Resolve<SitState>();
            sitState2.Enter();
            animatorComponent.Received(1).StartSit(statusComponent2.MyChair.IsFacingRight);
            animatorComponent.Received(1).StartSit(true);
            
        }

        [Test]
        public void SitState_OnTick_Leave_If_Chair_Frees()
        {
            var serviceSettings = new MonsterServiceSettings();
            typeof(MonsterServiceSettings).GetProperty("OrderDowntime",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(serviceSettings, 5f);
            Container.Rebind<MonsterServiceSettings>()
                .FromInstance(serviceSettings)
                .AsCached();
            
            var chair1 = Substitute.For<IChair>();
            chair1.IsTaken.Returns(false);
            
            var statusComponent1 = new MonsterStatusComponent { MyChair = chair1 };
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

            // if chair gets untaken monster leaves
            var sitState1 = Container.Resolve<SitState>();
            sitState1.OnTick();
            animatorComponent.Received(1).StopSit();
            aiComponent.Received(1).ChangeState(MonsterState.Leave);
            
            animatorComponent.ClearReceivedCalls();
            aiComponent.ClearReceivedCalls();
            
            var chair2 = Substitute.For<IChair>();
            chair2.IsTaken.Returns(true);
            
            var statusComponent2 = new MonsterStatusComponent { MyChair = chair2 };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent2)
                .AsCached();
            
            // chair didn't throw monster away = monster stays
            var sitState2 = Container.Resolve<SitState>();
            sitState2.OnTick();
            animatorComponent.DidNotReceive().StopSit();
            aiComponent.DidNotReceive().ChangeState(MonsterState.Leave);
        }

        [Test]
        public void SitState_OnTick_Change_State_Order()
        {
            // PITA
            var serviceSettings1 = new MonsterServiceSettings();
            typeof(MonsterServiceSettings).GetProperty("OrderDowntime",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(serviceSettings1, 5f);
            Container.Rebind<MonsterServiceSettings>()
                .FromInstance(serviceSettings1)
                .AsCached();
            
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var chair = Substitute.For<IChair>();
            chair.IsTaken.Returns(true);
            
            var statusComponent = new MonsterStatusComponent { MyChair = chair };
            Container.Rebind<MonsterStatusComponent>()
                .FromInstance(statusComponent)
                .AsCached();
            
            // the order downtime is bigger than current timer value
            var sitState1 = Container.Resolve<SitState>();
            sitState1.Enter();
            aiComponent.ClearReceivedCalls();
            sitState1.OnTick();
            aiComponent.DidNotReceive().ChangeState(MonsterState.Order);
            
            var serviceSettings2 = new MonsterServiceSettings();
            typeof(MonsterServiceSettings).GetProperty("OrderDowntime",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .SetValue(serviceSettings2, -5f);
            Container.Rebind<MonsterServiceSettings>()
                .FromInstance(serviceSettings2)
                .AsCached();
            
            // order downtime has passed
            var sitState2 = Container.Resolve<SitState>();
            sitState2.Enter();
            aiComponent.ClearReceivedCalls();
            sitState2.OnTick();
            aiComponent.Received(1).ChangeState(MonsterState.Order);
        }

    }
}