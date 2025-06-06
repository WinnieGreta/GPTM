using Interfaces;
using NSubstitute;
using NUnit.Framework;
using Signals;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterScoringComponentTests : ZenjectUnitTestFixture
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            SignalBusInstaller.Install(Container);
            Container.Bind<MonsterScoringComponent>().AsSingle();
            Container.DeclareSignal<OnMonsterScoredSignal>();
            
            Container.Bind<MonsterStatusComponent>().AsSingle();
            Container.Bind<IRecipeBook>().FromInstance(Substitute.For<IRecipeBook>()).AsSingle();
            
            var monsterServiceSettings = new MonsterServiceSettings();
            monsterServiceSettings.SetSerializedProperty("EatingDowntime", 2f); // default value for testing
            Container.Bind<MonsterServiceSettings>().FromInstance(monsterServiceSettings).AsSingle();
        }
        
        [Test]
        public void MonsterScoringComponent_Initialization()
        {
            var monsterScoringComponent = Container.Resolve<MonsterScoringComponent>();
            Assert.IsNotNull(monsterScoringComponent);
            Assert.IsInstanceOf<MonsterScoringComponent>(monsterScoringComponent);
        }
        
        [Test]
        public void MonsterScoringComponent_ScoreFood()
        {
            var monsterScoringComponent = Container.Resolve<MonsterScoringComponent>();
            var monsterStatusComponent = Container.Resolve<MonsterStatusComponent>();
            var recipeBook = Container.Resolve<IRecipeBook>();
            var signalBus = Container.Resolve<SignalBus>();
            // subscribe to the signal to verify it was fired
            var score = 0f;
            signalBus.Subscribe<OnMonsterScoredSignal>(signal =>
            {
                score = signal.Score; 
            });

            // set up the mock data
            monsterStatusComponent.FullOrder.AddRange(new[] { DishType.ChocolateCake, DishType.Burger });
            recipeBook[DishType.ChocolateCake].CookingTime.Returns(5f);
            recipeBook[DishType.Burger].CookingTime.Returns(3f);
            monsterStatusComponent.Patience = 2f;

            // call the method to test
            monsterScoringComponent.ScoreFood();

            // assert that the signal was fired with the correct score
            // (5 + 3) * 2 * 2 = 32
            Assert.AreEqual(32f, score, 0.01f, "The score should be calculated correctly based on the cooking times and patience.");
        }
        
        [Test]
        public void MonsterScoringComponent_ScoreFood_EmptyOrder()
        {
            var monsterScoringComponent = Container.Resolve<MonsterScoringComponent>();
            var monsterStatusComponent = Container.Resolve<MonsterStatusComponent>();
            var signalBus = Container.Resolve<SignalBus>();
            // subscribe to the signal to verify it was fired
            var score = 0f;
            signalBus.Subscribe<OnMonsterScoredSignal>(signal =>
            {
                score = signal.Score; // should be 0 since order is empty
            });

            // set up the mock data
            monsterStatusComponent.FullOrder.Clear();
            monsterStatusComponent.Patience = 2f;

            // Call the method to test
            monsterScoringComponent.ScoreFood();

            // Assert that the score is zero
            Assert.AreEqual(0f, score, "The score should be zero when the order is empty.");
        }
        
        [Test]
        public void MonsterScoringComponent_ScoreFood_OrderWithNone()
        {
            var monsterScoringComponent = Container.Resolve<MonsterScoringComponent>();
            var monsterStatusComponent = Container.Resolve<MonsterStatusComponent>();
            var recipeBook = Container.Resolve<IRecipeBook>();
            var signalBus = Container.Resolve<SignalBus>();
            //subscribe to the signal to verify it was fired
            var score = 0f;
            signalBus.Subscribe<OnMonsterScoredSignal>(signal =>
            {
                score = signal.Score; 
            });

            // set up the mock data
            monsterStatusComponent.FullOrder.AddRange(new[] { DishType.ChocolateCake, DishType.None });
            recipeBook[DishType.ChocolateCake].CookingTime.Returns(5f);
            monsterStatusComponent.Patience = 2f;

            // call the method
            monsterScoringComponent.ScoreFood();

            // assert that the score is calculated correctly
            // should be 5 * 2 * 2 = 20
            Assert.AreEqual(20f, score, 0.01f, "The score should be calculated correctly, ignoring DishType.None.");
        }
    }
}