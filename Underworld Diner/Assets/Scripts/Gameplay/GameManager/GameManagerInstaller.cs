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
            Container.BindInstance(_monsterSpawnAnchor).AsSingle();
        }
    }
}