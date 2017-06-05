using System;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class GameBoardInstaller : MonoInstaller<GameBoardInstaller>
    {
        [InjectOptional]
        private GameInfo _info = null;
        [Inject]
        private Settings _settings = null;

        public override void InstallBindings()
        {
            if (_info == null)
                Container.Bind<GameInfo>().AsSingle();
            else
            {
                _info.Reset();
                Container.BindInstance(_info).AsSingle();
            }

            Container.Bind<BadgeModel>().AsTransient();
            Container.Bind<BadgeSpawner>().AsSingle();
            Container.Bind<MenuManager>().AsSingle();
            Container.Bind<GameEvents>().AsSingle();
            Container.Bind<PlayerRowGameModel>().AsTransient();
            Container.Bind<SaveItemModel>().AsTransient();
            Container.Bind<IArtificialIntelligence>().To<RulesArtificialIntelligence>().AsSingle();
            Container.Bind<IFetchService<History>>().To<HistoryFetchService>().AsSingle();
            Container.Bind<IFetchService<Stats>>().To<StatsFetchService>().AsSingle();
            Container.Bind<ScoreCalculationService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerRowGameModel.Registry>().AsSingle();
            Container.BindInterfacesAndSelfTo<BadgeModel.Registry>().AsSingle();
            Container.BindInterfacesAndSelfTo<BadgeSpawnPoint.Registry>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveItemModel.Registry>().AsSingle();
            Container.BindInterfacesAndSelfTo<History>().AsSingle();
            Container.BindInterfacesAndSelfTo<Stats>().AsSingle();
            Container.BindInterfacesAndSelfTo<BadgeEraser>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

            Container.BindInitializableExecutionOrder<BadgeModel.Registry>(2);
            Container.BindInitializableExecutionOrder<Stats>(9);
            Container.BindInitializableExecutionOrder<GameManager>(10);
            Container.BindInitializableExecutionOrder<ConfirmStepWindowPresenter>(11);
            Container.BindInitializableExecutionOrder<VictoryHandler>(12);

            Container.BindInterfacesTo<CameraHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesTo<ActivePlayerHandler>().AsSingle();
            Container.BindInterfacesTo<VictoryHandler>().AsSingle();
            Container.BindInterfacesTo<TimerHandler>().AsSingle();
            Container.BindInterfacesTo<GameBoardSpawner>().AsSingle();
            
            InstallFactories();
            InstallPresenters();
        }

        private void InstallFactories()
        {
            Container.Bind<GameBoardSpawner.BoardFactory>().AsSingle().WithArguments(_settings.BoardPrefab);
            Container.Bind<GameBoardSpawner.StickFactory>().AsSingle().WithArguments(_settings.StickPrefab);
            Container.BindFactory<BadgeSpawnPoint, GameBoardSpawner.StickPartitionFactory>()
                .FromComponentInNewPrefab(_settings.StickPartitionPrefab);
            Container.BindFactory<BadgeFacade, BadgeFacade.Factory>()
                .FromComponentInNewPrefab(_settings.BadgePrefab);
            Container.BindFactory<PlayerRowGameFacade, PlayerRowGameFacade.Factory>()
                .FromComponentInNewPrefab(_settings.PlayerRowGameFacadePrefab);
            Container.BindFactory<SaveItemFacade, SaveItemFacade.Factory>()
                .FromComponentInNewPrefab(_settings.SaveItemTogglePrefab);
        }

        private void InstallPresenters()
        {
            Container.BindInterfacesAndSelfTo<ConfirmStepWindowPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayAgainWindowPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayersTableWindowPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<InformationPanelPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseWindowPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveGameWindowPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadGameWindowPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<ModalDialog>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GameObject BoardPrefab;
            public GameObject StickPrefab;
            public GameObject StickPartitionPrefab;
            public GameObject BadgePrefab;
            public GameObject PlayerRowGameFacadePrefab;
            public GameObject SaveItemTogglePrefab;
        }
    }
}