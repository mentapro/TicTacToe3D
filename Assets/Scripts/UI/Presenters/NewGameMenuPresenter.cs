using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace TicTacToe3D
{
    public class NewGameMenuPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private NewGameMenuView View { get; set; }
        private MenuManager MenuManager { get; set; }
        private Settings _Settings { get; set; }
        private PlayerRowFacade.Factory PlayerRowFactory { get; set; }
        private PlayerRowModel.Registry RowRegistry { get; set; }
        private GameInfo Info { get; set; }
        private ZenjectSceneLoader SceneLoader { get; set; }

        public NewGameMenuPresenter(MenuManager menuManager,
            Settings settings,
            PlayerRowFacade.Factory playerRowFactory,
            PlayerRowModel.Registry rowRegistry,
            GameInfo info, ZenjectSceneLoader sceneLoader)
        {
            MenuManager = menuManager;
            _Settings = settings;
            PlayerRowFactory = playerRowFactory;
            RowRegistry = rowRegistry;
            Info = info;
            SceneLoader = sceneLoader;

            MenuManager.SetMenu(this);
        }

        public void SetView(NewGameMenuView view)
        {
            View = view;
        }

        public void Initialize()
        {
            for (var i = 0; i < _Settings.PlayersMax; i++)
            {
                var playerRow = PlayerRowFactory.Create();
                playerRow.transform.SetParent(View.PlayerRows, false);
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

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
        }

        private void OnStartButtonClicked()
        {
            var players = RowRegistry.GetValidatedPlayers();
            if (players.Count < 2)
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