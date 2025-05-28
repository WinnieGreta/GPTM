using Interfaces.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapon.Fireball
{
    public class FireballFactoryInstaller : MonoInstaller
    {
        [SerializeField] private FireballFacade _fireballFacade;

        private MonoPoolableMemoryPool<Vector3, FireballFacade> _memoryPool;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<FireballFacade, MonoPoolableMemoryPool<Vector3, FireballFacade>>()
                .WithInitialSize(3)
                .FromComponentInNewPrefab(_fireballFacade)
                .UnderTransformGroup("Projectiles");
            Container.BindFactory<Vector3, IProjectile, IProjectile.Factory>()
                .WithId("FireballFactory").FromMethod(SpawnFireball);
        }

        private IProjectile SpawnFireball(DiContainer _, Vector3 root)
        {
            if (_memoryPool == null)
            {
                _memoryPool = Container.Resolve<MonoPoolableMemoryPool<Vector3, FireballFacade>>();
            }

            return _memoryPool.Spawn(root);
        }
    }
}