using UnityEngine;
using Zenject;

namespace Gameplay.Weapon.Fireball
{
    public class FireballMonoInstaller : MonoInstaller
    {
        [SerializeField] private Animator _animator;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_animator).AsSingle();
            
            Container.BindInterfacesAndSelfTo<FireballFacade>()
                .FromComponentOnRoot()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<Rigidbody2D>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesAndSelfTo<FireballAnimatorComponent>().AsSingle();
        }
        
    }
}