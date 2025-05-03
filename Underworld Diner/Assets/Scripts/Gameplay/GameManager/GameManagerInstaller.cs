using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.GameManager
{
    public class GameManagerInstaller : MonoInstaller
    {
        //[SerializeField] private NavMeshSurface _navMeshSurface;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }
    }
}