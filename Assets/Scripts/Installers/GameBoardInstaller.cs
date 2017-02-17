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
                Container.BindInstance(_info).AsSingle();

            Container.Bind<BadgeModel>().AsTransient();
            Container.Bind<BadgeSpawner>().AsSingle();
            Container.Bind<MenuManager>().AsSingle();
            Container.Bind<GameEvents>().AsSingle();

            Container.BindInterfacesAndSelfTo<BadgeModel.Registry>().AsSingle();
            Container.BindInterfacesAndSelfTo<BadgeSpawnPoint.Registry>().AsSingle();
            Container.BindInterfacesAndSelfTo<History>().AsSingle();

            Container.BindInterfacesTo<CameraHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();
            Container.BindInterfacesTo<ActivePlayerHandler>().AsSingle();
            Container.BindInterfacesTo<GameRestartHandler>().AsSingle();
            Container.BindInterfacesTo<VictoryHandler>().AsSingle();
            Container.BindInterfacesTo<TimerHandler>().AsSingle();
            Container.BindInterfacesTo<GameBoardSpawner>().AsSingle();
            
            Container.BindInitializableExecutionOrder<GameRestartHandler>(1000);

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
        }

        private void InstallPresenters()
        {
            Container.BindInterfacesAndSelfTo<ConfirmStepWindowPresenter>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GameObject BoardPrefab;
            public GameObject StickPrefab;
            public GameObject StickPartitionPrefab;
            public GameObject BadgePrefab;
        }
    }
}