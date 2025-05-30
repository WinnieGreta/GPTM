using Gameplay.Monster.Abstract;
using NSubstitute;
using NUnit.Framework;
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
        }
        
    }
}