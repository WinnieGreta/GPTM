using Interfaces;
using UnityEngine;
using Zenject;

namespace Gameplay.Monster
{
    public class MonsterInstaller : Installer<MonsterType, Transform, MonsterInstaller>
    {
        private readonly MonsterType _monsterType;
        private readonly Transform _transform;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MonsterFacade>().FromComponentInNewPrefabResource(_monsterType.ToString())
                .UnderTransformGroup("Monsters")
                .AsSingle()
                .OnInstantiated<MonsterFacade>((_, x) => x.transform.position = _transform.position);
        }

        public MonsterInstaller(MonsterType monsterType, Transform transform)
        {
            _monsterType = monsterType;
            _transform = transform;
        }
    }
}