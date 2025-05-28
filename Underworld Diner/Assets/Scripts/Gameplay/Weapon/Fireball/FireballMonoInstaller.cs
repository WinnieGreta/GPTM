using UnityEngine;
using Zenject;

namespace Gameplay.Weapon.Fireball
{
    public class FireballMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FireballFacade>()
                .FromComponentOnRoot()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<Rigidbody2D>().FromComponentOnRoot().AsSingle();
        }
        
    }
}