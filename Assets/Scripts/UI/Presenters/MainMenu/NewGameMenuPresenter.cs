using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace TicTacToe3D
{
    public class NewGameMenuPresenter : MenuPresenter<NewGameMenuView>, IInitializable, IDisposable
    {
        private MenuManager MenuManager { get; set; }
        private Settings _Settings { get; set; }
        private PlayerRowMenuFacade.Factory PlayerRowFactory { get; set; }
        private PlayerRowMenuModel.Registry RowRegistry { get; set; }
        private GameInfo Info { get; set; }
        private ZenjectSceneLoader SceneLoader { get; set; }

        public NewGameMenuPresenter(MenuManager menuManager,
            Settings settings,
            PlayerRowMenuFacade.Factory playerRowFactory,
            PlayerRowMenuModel.Registry rowRegistry,
            GameInfo info, ZenjectSceneLoader sceneLoader, AudioController audioController) : base(audioController)
        {
            MenuManager = menuManager;
            _Settings = settings;
            PlayerRowFactory = playerRowFactory;
            RowRegistry = rowRegistry;
            Info = info;
            SceneLoader = sceneLoader;

            MenuManager.SetMenu(this);
        }

        public void Initialize()
        {
            for (var i = 0; i < _Settings.PlayersMax; i++)
            {
                var playerRow = PlayerRowFactory.Create();
                playerRow.transform.SetParent(View.PlayersRows, false);
            }

            View.StartButton.onClick.AddListener(OnStartButtonClicked);
            View.CancelButton.onClick.AddListener(OnCancelButtonClicked);
            MenuManager.OpenMenu(Menus.GameInfoMenu);
        }

        public void Dispose()
        {
            View.StartButton.onClick.RemoveAllListeners();
            View.CancelButton.onClick.RemoveAllListeners();
        }

        private void OnStartButtonClicked()
        {
            var players = RowRegistry.GetValidatedPlayers();
            if (players == null || players.Count < 2)
            {
                ModalDialog.Show("<color=red>Error!</color>\nNot all values are filled.");
                return;
            }
            Info.Players = players;
            SceneLoader.LoadScene("GameBoard", LoadSceneMode.Single, container =>
            {
                container.BindInstance(Info).WhenInjectedInto<GameBoardInstaller>();
            });
        }

        private void OnCancelButtonClicked()
        {
            MenuManager.OpenMenu(Menus.MainMenu);
        }

        [Serializable]
        public class Settings
        {
            public int PlayersMax;
        }
    }
}