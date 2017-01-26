using System;
using Zenject;

namespace TicTacToe3D
{
    public class NewGameMenuPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private NewGameMenuView View { get; set; }
        private MenuManager MenuManager { get; set; }
        private Settings _Settings { get; set; }
        private PlayerRowFacade.Factory PlayerRowFactory { get; set; }

        public NewGameMenuPresenter(MenuManager menuManager, Settings settings, PlayerRowFacade.Factory playerRowFactory)
        {
            MenuManager = menuManager;
            _Settings = settings;
            PlayerRowFactory = playerRowFactory;

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