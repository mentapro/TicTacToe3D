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
        private MainMenuPresenter MainMenu { get; set; }
        private GameInformationPresenter GameInfoPanel { get; set; }
        private AdvancedSettingsPresenter AdvancedSettingsPanel { get; set; }

        public NewGameMenuPresenter(MenuManager menuManager, Settings settings, PlayerRowFacade.Factory playerRowFactory)
        {
            MenuManager = menuManager;
            _Settings = settings;
            PlayerRowFactory = playerRowFactory;
        }

        [Inject]
        private void InjectMenus(MainMenuPresenter mainMenu,
            GameInformationPresenter gameInformationPanel,
            AdvancedSettingsPresenter advancedSettingsPanel)
        {
            MainMenu = mainMenu;
            GameInfoPanel = gameInformationPanel;
            AdvancedSettingsPanel = advancedSettingsPanel;
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

            View.CancelButton.onClick.AddListener(OnCancelButtonClicked);
            GameInfoPanel.Open();
        }

        public void Dispose()
        {
            View.CancelButton.onClick.RemoveAllListeners();
        }

        public void SwitchPanel()
        {
            if (GameInfoPanel.IsOpen())
            {
                GameInfoPanel.Close();
                AdvancedSettingsPanel.Open();
            }
            else
            {
                AdvancedSettingsPanel.Close();
                GameInfoPanel.Open();
            }
        }

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
        }

        private void OnCancelButtonClicked()
        {
            MenuManager.OpenMenu(MainMenu);
        }

        [Serializable]
        public class Settings
        {
            public int PlayersMax;
        }
    }
}