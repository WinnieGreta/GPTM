using Gameplay.GameManager.LevelTimerManager;
using Interfaces;
using UnityEngine;
using Zenject;
using Signals;

namespace Gameplay.GameManager
{
    public class GameManagerInstaller : MonoInstaller
    {
        [SerializeField] private Transform _monsterSpawnAnchor;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameSpawnManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelTimerComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoringManager>().AsSingle();
            Container.BindInstance(_monsterSpawnAnchor).AsSingle();
            Container.DeclareSignal<OnMonsterScoredSignal>();
            Container.BindSignal<OnMonsterScoredSignal>()
                .ToMethod<ScoringManager>(x => x.UpdateScore)
                .FromResolve();
            Container.DeclareSignal<OnGamePauseSignal>();
            Container.DeclareSignal<OnGameUnpauseSignal>();
        }
    }
}