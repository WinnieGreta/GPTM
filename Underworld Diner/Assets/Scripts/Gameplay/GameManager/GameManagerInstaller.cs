using Interfaces;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.GameManager
{
    public class GameManagerInstaller : MonoInstaller
    {
        [SerializeField] private Transform _monsterSpawnAnchor;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameSpawnManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceManager.ResourceManager>().AsSingle();
            Container.BindInstance(_monsterSpawnAnchor).AsSingle();
        }
    }
}