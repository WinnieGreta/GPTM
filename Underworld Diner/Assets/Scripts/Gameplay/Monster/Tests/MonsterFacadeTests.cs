using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterFacadeTests : ZenjectUnitTestFixture
    {
        /*[Inject] private MonsterAIComponent _aiComponent;
        [Inject] private SignalBus _signalBus;
        [Inject] private MonsterStatusComponent _status;
        [Inject] private MonsterPatienceComponent _patienceComponent;
        [Inject] private MonsterType _monsterType;*/
        
        //public DishType ExpectedDish => _status.ExpectedDish;

        [SetUp]
        public void Setup()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<MonsterStatusComponent>().AsSingle();
        }
        
        [Test]
        public void TestExpectedDish()
        {
            Container.Unbind<MonsterStatusComponent>();
            //var status = 
        }
    }
}