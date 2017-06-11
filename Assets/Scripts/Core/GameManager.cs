using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace TicTacToe3D
{
    public class GameManager : IInitializable
    {
        private readonly GameInfo _info;
        private readonly History _history;
        private readonly BadgeFacade.Factory _badgeFactory;
        private readonly BadgeFacade.Settings _badgeSettings;
        private readonly BadgeSpawnPoint.Registry _spawnRegistry;
        private readonly IFetchService<History> _historyFetchService;
        private readonly ZenjectSceneLoader _sceneLoader;
        private readonly GameSettings _gameSettings;

        public GameManager(GameInfo info,
            [InjectOptional] History history,
            [InjectOptional] BadgeFacade.Factory badgeFactory,
            [InjectOptional] BadgeFacade.Settings badgeSettings,
            [InjectOptional] BadgeSpawnPoint.Registry spawnRegistry,
            IFetchService<History> historyFetchService,
            ZenjectSceneLoader sceneLoader,
            GameSettings gameSettings)
        {
            _info = info;
            _history = history;
            _badgeFactory = badgeFactory;
            _badgeSettings = badgeSettings;
            _spawnRegistry = spawnRegistry;
            _historyFetchService = historyFetchService;
            _sceneLoader = sceneLoader;
            _gameSettings = gameSettings;
        }

        public void Initialize()
        {
            if (_info.HistoryItems != null)
            {
                MakeAllSteps();
            }
        }

        public void SaveGame(string fileName)
        {
            _historyFetchService.Save(_history, fileName);
        }

        public void LoadGame(string fileName)
        {
            var history = _historyFetchService.Load(fileName);
            var info = new GameInfo(_gameSettings)
            {
                Dimension = history.Info.Dimension,
                StepSize = history.Info.StepSize,
                BadgesToWin = history.Info.BadgesToWin,
                GlobalStep = history.Info.GlobalStep,
                GameState = history.Info.GameState,
                ActivePlayerMadeSteps = history.Info.ActivePlayerMadeSteps,
                TimerTime = history.Info.TimerTime,
                GameSettings =
                {
                    GameOverAfterFirstWinner = history.Info.GameSettings.GameOverAfterFirstWinner,
                    TimerType = history.Info.GameSettings.TimerType
                },
                PlayerCanWin = history.Info.PlayerCanWin,
                HistoryItems = history.HistoryItems,
                Players = history.Info.Players,
                ActivePlayer = history.Info.Players.First(x => x == history.Info.ActivePlayer)
            };

            _sceneLoader.LoadScene("GameBoard", LoadSceneMode.Single, container =>
            {
                container.BindInstance(info).WhenInjectedInto<GameBoardInstaller>();
            });
        }

        private void MakeAllSteps()
        {
            foreach (var saveItem in _info.HistoryItems)
            {
                var badge = _badgeFactory.Create();
                badge.transform.localScale = new Vector3(_badgeSettings.Diameter, _badgeSettings.Diameter, _badgeSettings.Diameter);
                badge.transform.SetParent(_spawnRegistry.Spawns.First(x => x.Coordinates == saveItem.BadgeCoordinates).transform, true);
                badge.transform.localPosition = Vector3.zero;

                badge.Owner = _info.Players.First(player => player.Name == saveItem.PlayerName);
                badge.Coordinates = saveItem.BadgeCoordinates;
                badge.Color = badge.Owner.Color;
                badge.IsConfirmed = saveItem.IsBadgeConfirmed;
                _spawnRegistry.Spawns.First(spawn => spawn.Coordinates == saveItem.BadgeCoordinates).Badge = badge.Model;
                _history.Push(saveItem);
            }
        }
    }
}