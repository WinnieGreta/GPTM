using System.Collections.Generic;
using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class GoSitStateTests : ZenjectUnitTestFixture
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
            Container.Bind<IChairManager>()
                .FromInstance(Substitute.For<IChairManager>())
                .AsCached();
            Container.Bind<GoSitState>().AsTransient();
        }

        [Test]
        public void GoSitState_Find_Free_Chair()
        {
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var navigationComponent = Substitute.For<INavigationComponent>();
            Container.Rebind<INavigationComponent>()
                .FromInstance(navigationComponent)
                .AsCached();
            
            var chairManager = Substitute.For<IChairManager>();
            Container.Rebind<IChairManager>()
                .FromInstance(chairManager)
                .AsCached();

            var takenChair = Substitute.For<IChair>();
            takenChair.IsTaken.Returns(true);
            takenChair.IsClean.Returns(true);

            var dirtyChair = Substitute.For<IChair>();
            dirtyChair.IsTaken.Returns(false);
            dirtyChair.IsClean.Returns(false);

            var freeChair = Substitute.For<IChair>();
            freeChair.IsTaken.Returns(false);
            freeChair.IsClean.Returns(true);

            // all seats are either dirty or taken
            chairManager.Chairs.Returns(new List<IChair> { takenChair, dirtyChair, takenChair, dirtyChair });
            // we don't care about destination reached check in this OnTick
            navigationComponent.HasReachedDestination().Returns(false, false);
            var goSitState1 = Container.Resolve<GoSitState>();
            
            // checking once is enough as, ideally, OnTick would try to find free chair only once
            goSitState1.OnTick();
            aiComponent.DidNotReceiveWithAnyArgs().TakeChairByMonster(default);
            navigationComponent.DidNotReceiveWithAnyArgs().ProcessStationMovement(default);
            
            // cleanup before next subcase
            aiComponent.ClearReceivedCalls();
            navigationComponent.ClearReceivedCalls();
            
            // there is a clean chair
            chairManager.Chairs.Returns(new List<IChair> { takenChair, dirtyChair, freeChair, freeChair });
            // we need to have a different GoSitState, as the one we used already raised private flag
            var goSitState2 = Container.Resolve<GoSitState>();
            goSitState2.OnTick();
            aiComponent.Received(1).TakeChairByMonster(freeChair);
            navigationComponent.Received(1).ProcessStationMovement(freeChair);
        }

        [Test]
        public void GoSitState_OnTick_Leave_If_No_Chairs()
        {
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var navigationComponent = Substitute.For<INavigationComponent>();
            Container.Rebind<INavigationComponent>()
                .FromInstance(navigationComponent)
                .AsCached();
            
            var chairManager = Substitute.For<IChairManager>();
            Container.Rebind<IChairManager>()
                .FromInstance(chairManager)
                .AsCached();

            var takenChair = Substitute.For<IChair>();
            takenChair.IsTaken.Returns(true);
            takenChair.IsClean.Returns(true);

            var dirtyChair = Substitute.For<IChair>();
            dirtyChair.IsTaken.Returns(false);
            dirtyChair.IsClean.Returns(false);

            var freeChair = Substitute.For<IChair>();
            freeChair.IsTaken.Returns(false);
            freeChair.IsClean.Returns(true);

            // all seats are either dirty or taken
            chairManager.Chairs.Returns(new List<IChair> { takenChair, dirtyChair, takenChair, dirtyChair }, new List<IChair> { takenChair, dirtyChair, freeChair, freeChair });
            // we don't care about destination reached check in this OnTick
            navigationComponent.HasReachedDestination().Returns(false, false);
            var goSitState1 = Container.Resolve<GoSitState>();
            
            // monster should leave if no chairs available
            goSitState1.OnTick();
            aiComponent.Received(1).ChangeState(MonsterState.Leave);
            
            // cleanup
            aiComponent.ClearReceivedCalls();
            
            // there is a clean chair
            chairManager.Chairs.Returns(new List<IChair> { freeChair, takenChair, dirtyChair, freeChair, freeChair });
            // we need to have a different GoSitState, as the one we used already raised private flag
            var goSitState2 = Container.Resolve<GoSitState>();
            goSitState2.OnTick();
            aiComponent.DidNotReceive().ChangeState(MonsterState.Leave);
            
        }
        
        [Test]
        public void GoSitState_OnTick_Sit_If_Reached_Chair()
        {
            var aiComponent = Substitute.For<IAiComponent>();
            Container.Rebind<IAiComponent>()
                .FromInstance(aiComponent)
                .AsCached();
            
            var navigationComponent = Substitute.For<INavigationComponent>();
            Container.Rebind<INavigationComponent>()
                .FromInstance(navigationComponent)
                .AsCached();
            
            var chairManager = Substitute.For<IChairManager>();
            Container.Rebind<IChairManager>()
                .FromInstance(chairManager)
                .AsCached();
            
            // we are not completely indifferent towards chairs
            // we need to have destination set before we check if it is reached
            var freeChair = Substitute.For<IChair>();
            freeChair.IsTaken.Returns(false);
            freeChair.IsClean.Returns(true);
            
            // there is a clean chair in both checks
            chairManager.Chairs.Returns(new List<IChair> { freeChair, freeChair });

            var goSitState3 = Container.Resolve<GoSitState>();
            
            // monster doesn't sit before reaching a chair
            navigationComponent.HasReachedDestination().Returns(false);
            goSitState3.OnTick();
            aiComponent.DidNotReceive().ChangeState(MonsterState.Sit);
            
            // cleanup
            aiComponent.ClearReceivedCalls();
            
            // we can use the same instance of a state
            navigationComponent.HasReachedDestination().Returns(true);
            goSitState3.OnTick();
            aiComponent.Received(1).ChangeState(MonsterState.Sit);
        }   
    }
}