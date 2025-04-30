using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.GameplayManager
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        public override void InstallBindings()
        {
            Container.BindInstance(_navMeshSurface).AsSingle();
        }
    }
}