using System;
using System.Collections.Generic;
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
                InitializeGameInfo();

            Container.BindInstance(_info).AsSingle();

            Container.Bind<BadgeModel>().AsTransient();
            Container.Bind<BadgeSpawner>().AsSingle();

            Container.Bind<BadgeModel.Registry>().AsSingle();
            Container.BindAllInterfacesAndSelf<BadgeSpawnPoint.Registry>().To<BadgeSpawnPoint.Registry>().AsSingle();

            Container.BindAllInterfaces<CameraHandler>().To<CameraHandler>().AsSingle();
            Container.BindAllInterfaces<PlayerInputHandler>().To<PlayerInputHandler>().AsSingle();
            Container.BindAllInterfaces<ActivePlayerHandler>().To<ActivePlayerHandler>().AsSingle();
            Container.BindAllInterfaces<GameControlHandler>().To<GameControlHandler>().AsSingle();
            Container.BindAllInterfaces<VictoryHandler>().To<VictoryHandler>().AsSingle();
            Container.BindAllInterfaces<GameBoardSpawner>().To<GameBoardSpawner>().AsSingle();
            Container.BindAllInterfaces<WinDetector>().To<WinDetector>().AsSingle();
            
            Container.BindInitializableExecutionOrder<GameControlHandler>(1000);

            InstallSignals();
            InstallFactories();
        }

        private void InstallSignals()
        {
            Container.BindSignal<BadgeSpawned>();
            Container.BindSignal<ActivePlayerChanged>(); // subscribe on this before Initialize (Start) because it will be fired during Initialize
            Container.BindSignal<PlayerWon>();
        }

        private void InstallFactories()
        {
            Container.BindFactory<GameObject, GameBoardSpawner.BoardFactory>()
                .FromPrefab(_settings.BoardPrefab);
            Container.BindFactory<GameObject, GameBoardSpawner.StickFactory>()
                .FromPrefab(_settings.StickPrefab)
                .UnderTransformGroup("Sticks");
            Container.BindFactory<BadgeSpawnPoint, GameBoardSpawner.StickPartitionFactory>()
                .FromPrefab(_settings.StickPartitionPrefab);
            Container.BindFactory<BadgeFacade, BadgeFacade.Factory>()
                .FromPrefab(_settings.BadgePrefab);
        }

        private void InitializeGameInfo()
        {
            _info = new GameInfo
            {
                Dimension = 4,
                BadgesToWin = 4,
                StepSize = 1,
                Players = new List<Player>
                {
                    new Player(PlayerTypes.Human, "Player 1", Color.red),
                    new Player(PlayerTypes.Human, "Player 2", Color.blue),
                    new Player(PlayerTypes.Human, "Player 3", Color.green)
                }
            };
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