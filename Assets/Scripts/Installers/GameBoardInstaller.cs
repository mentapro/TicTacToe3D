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

            Container.Bind<IInitializable>().To<GameBoardSpawner>().AsSingle();
            Container.BindFactory<GameObject, GameBoardSpawner.BoardFactory>()
                .FromPrefab(_settings.BoardPrefab);
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
                    new Player(PlayerType.Human, "Player 1", Color.red),
                    new Player(PlayerType.Human, "Player 2", Color.blue)
                }
            };
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject BoardPrefab;
            public GameObject StickPrefab;
            public GameObject StickPartitionPrefab;
        }
    }
}