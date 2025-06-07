using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster.States.Tests
{
    public class LeaveStateTests : ZenjectUnitTestFixture
    {
        private INavigationComponent _navigation;
        private IAiComponent _aiComponent;
        private IDespawnable _despawnable;
        
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _aiComponent = Substitute.For<IAiComponent>();
            _navigation = Substitute.For<INavigationComponent>();
            _despawnable = Substitute.For<IDespawnable>();
            
            var gameObject = new GameObject("Exit");
            Container.Bind<Transform>().FromNewComponentOn(gameObject).AsSingle();
            
            Container.BindInstance(_aiComponent);
            Container.BindInstance(_navigation);
            Container.BindInstance(_despawnable);
            Container.Bind<LeaveState>().AsTransient();
        }

        [Test]
        public void LeaveState_Enter()
        {
            var transform = Container.Resolve<Transform>();
            var state = Container.Resolve<LeaveState>();
            state.Enter();
            _navigation.Received(1).ProcessMovement(Arg.Any<Vector2>());
        }

        [Test]
        public void LeaveState_OnTick()
        {
            _navigation.HasReachedDestination(1f).Returns(true);
            var state = Container.Resolve<LeaveState>();
            state.OnTick();
            _aiComponent.Received(1).ChangeState(MonsterState.Null);
            _despawnable.Received(1).Despawn();
        }
        
    }
}