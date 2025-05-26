using System.Collections.Generic;
using Gameplay.GameManager.LevelTimerManager;
using Interfaces;
using Interfaces.Player;
using UnityEngine;
using Zenject;
using Signals;

namespace Gameplay.GameManager
{
    public class GameManagerInstaller : MonoInstaller
    {
        [SerializeField] private Transform _monsterSpawnAnchor;
        [SerializeField] private GameObjectContext _player;
        [SerializeField] private List<Canvas> _uiCanvases;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameSpawnManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelTimerComponent>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoringManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<StatisticsManager>().AsSingle();
            Container.BindInstance(_monsterSpawnAnchor).AsSingle();
            Container.BindInstance(_uiCanvases).AsSingle();
            Container.Bind<IPlayer>().FromSubContainerResolve().ByInstance(_player.Container).AsSingle();
            
            Container.DeclareSignal<OnMonsterScoredSignal>();
            Container.DeclareSignal<OnGameUnpauseSignal>();
            Container.DeclareSignal<OnGamePauseSignal>();
            Container.DeclareSignal<OnLevelFinishedSignal>();
            
            Container.BindSignal<OnMonsterScoredSignal>()
                .ToMethod<ScoringManager>(x => x.UpdateScore)
                .FromResolve();
        }
    }
}