using Gameplay.Monster.Abstract;
using Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Monster.Tests
{
    public class MonsterAnimatorComponentTests : ZenjectUnitTestFixture
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
            Container.BindInterfacesAndSelfTo<MonsterAnimatorComponent>().AsSingle();
            Container.Bind<MonsterStatusComponent>().AsSingle();
            var gameObject = new GameObject("MonsterAnimatorComponent");
            Container.Bind<Transform>().FromNewComponentOn(gameObject).AsSingle();
            Container.Bind<MonsterFacade>().FromNewComponentOn(gameObject).AsSingle();
            Container.Bind<SpriteRenderer>().FromNewComponentOn(gameObject).AsSingle();
            Container.Bind<NavMeshAgent>().FromNewComponentOn(gameObject).AsSingle();

            var animator = gameObject.AddComponent<Animator>();

            var animatorController = AnimatorController.CreateAnimatorControllerAtPath("Assets/AnimatorController.controller");
            animatorController.AddParameter("isSitting", AnimatorControllerParameterType.Bool);
            animatorController.AddParameter("isMoving", AnimatorControllerParameterType.Bool);
            animatorController.AddParameter("isDead", AnimatorControllerParameterType.Bool);

            animator.runtimeAnimatorController = animatorController;
            Container.BindInstance(animator).AsSingle();
        }

        //teardown delete controller
        [TearDown]
        public void TearDown()
        {
            var animatorController = Container.Resolve<Animator>().runtimeAnimatorController as AnimatorController;
            if (animatorController != null)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(animatorController));
            }
        }

        [Test]
        public void MonsterAnimatorComponent_Initialization()
        {
            var animatorComponent = Container.Resolve<MonsterAnimatorComponent>();
            animatorComponent.Initialize();
            Assert.IsNotNull(animatorComponent);
            Assert.IsInstanceOf<MonsterAnimatorComponent>(animatorComponent);
        }

        [Test]
        public void MonsterAnimatorComponent_StartSit()
        {
            var animatorComponent = Container.Resolve<MonsterAnimatorComponent>();
            var animator = Container.Resolve<Animator>();
            var navMeshAgent = Container.Resolve<NavMeshAgent>();
            var spriteRenderer = Container.Resolve<SpriteRenderer>();

            animatorComponent.StartSit(true);
            animatorComponent.LateTick();

            Assert.IsTrue(animator.GetBool("isSitting"));
            Assert.IsFalse(spriteRenderer.flipX);
            Assert.AreEqual(spriteRenderer.sortingOrder, 1);
            Assert.IsFalse(navMeshAgent.enabled);
        }

        [Test]
        public void MonsterAnimatorComponent_FixedTick_MovementCheck()
        {
            var animatorComponent = Container.Resolve<MonsterAnimatorComponent>();
            var transform = Container.Resolve<Transform>();
            var animator = Container.Resolve<Animator>();
            var spriteRenderer = Container.Resolve<SpriteRenderer>();

            animatorComponent.Initialize();
            // Initial position
            transform.position = Vector3.zero;
            animatorComponent.FixedTick();

            // Move to a new position
            transform.position = Vector3.right;
            animatorComponent.FixedTick();
            animatorComponent.LateTick();

            // Check if moving is detected
            Assert.IsTrue(animator.GetBool("isMoving"));
            Assert.IsFalse(spriteRenderer.flipX);

            // Move back to the original position
            transform.position = Vector3.zero;
            animatorComponent.FixedTick();
            animatorComponent.LateTick();

            Assert.IsTrue(animator.GetBool("isMoving"));
            Assert.IsTrue(spriteRenderer.flipX);

            // Check if moving is reset
            animatorComponent.FixedTick();
            animatorComponent.LateTick();

            Assert.IsFalse(animator.GetBool("isMoving"));
        }

        [Test]
        public void MonsterAnimatorComponent_LateTick_FlipSprite()
        {
            var animatorComponent = Container.Resolve<MonsterAnimatorComponent>();
            var transform = Container.Resolve<Transform>();
            var spriteRenderer = Container.Resolve<SpriteRenderer>();

            animatorComponent.Initialize();
            // Initial position
            transform.position = Vector3.zero;
            animatorComponent.FixedTick();
            animatorComponent.LateTick();

            // Move to the right
            transform.position = Vector3.right;
            animatorComponent.FixedTick();
            animatorComponent.LateTick();

            Assert.IsFalse(spriteRenderer.flipX);

            // Move to the left
            transform.position = Vector3.left;
            animatorComponent.FixedTick();
            animatorComponent.LateTick();

            Assert.IsTrue(spriteRenderer.flipX);
           
            // Sitting with facing right
            animatorComponent.FixedTick();
            animatorComponent.StartSit(true);
            animatorComponent.LateTick();
            Assert.IsFalse(spriteRenderer.flipX); 
            
            // Sitting with facing left
            animatorComponent.FixedTick();
            animatorComponent.StartSit(false);
            animatorComponent.LateTick();
            Assert.IsTrue(spriteRenderer.flipX);
        }

        [Test]
        public void MonsterAnimatorComponent_LateTick_SitState()
        {
            var animatorComponent = Container.Resolve<MonsterAnimatorComponent>();
            var animator = Container.Resolve<Animator>();

            animatorComponent.StartSit(true);
            animatorComponent.LateTick();

            Assert.IsTrue(animator.GetBool("isSitting"));
            Assert.IsFalse(animator.GetBool("isMoving"));

            animatorComponent.StopSit();
            animatorComponent.LateTick();

            Assert.IsFalse(animator.GetBool("isSitting"));
        }
        
        [Test]
        public void MonsterAnimatorComponent_DeathAnimation()
        {
            var animatorComponent = Container.Resolve<MonsterAnimatorComponent>();
            var animator = Container.Resolve<Animator>();

            animatorComponent.DeathAnimation();

            Assert.IsTrue(animator.GetBool("isDead"));
        }
        
        [Test]
        public void MonsterAnimatorComponent_Restart()
        {
            var animatorComponent = Container.Resolve<MonsterAnimatorComponent>();
            var animator = Container.Resolve<Animator>();
            var transform = Container.Resolve<Transform>();
            
            //move then sit and die to trigger internal state changes
            animatorComponent.Initialize();
            
            transform.position = Vector3.right;
            animatorComponent.FixedTick();
            animatorComponent.LateTick();
            
            animatorComponent.StartSit(true);
            animatorComponent.LateTick();
            Assert.IsTrue(animator.GetBool("isSitting"));
            
            animatorComponent.DeathAnimation();
            Assert.IsTrue(animator.GetBool("isDead"));
            

            animatorComponent.Restart();
            
            // Assert that all states are reset
            Assert.IsFalse(animator.GetBool("isSitting"));
            Assert.IsFalse(animator.GetBool("isMoving"));
            Assert.IsFalse(animator.GetBool("isDead"));
        }
    }
}