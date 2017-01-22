using System;
using Zenject;

namespace TicTacToe3D
{
    public class GameInformationPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private GameInformationView View { get; set; }
        private NewGameMenuPresenter NewGameMenu { get; set; }
        private AdvancedSettingsPresenter AdvancedSettingsPanel { get; set; }

        public GameInformationPresenter(NewGameMenuPresenter newGameMenu)
        {
            NewGameMenu = newGameMenu;
        }

        [Inject]
        private void InjectPanels(AdvancedSettingsPresenter advancedSettingsPanel)
        {
            AdvancedSettingsPanel = advancedSettingsPanel;
        }

        public void SetView(GameInformationView view)
        {
            View = view;
        }

        public void Initialize()
        {
            View.AdvancedSettingsButton.onClick.AddListener(OnAdvancedSettingsButtonClicked);

            NewGameMenu.OpenPanel(this);
        }

        public void Dispose()
        {
            View.AdvancedSettingsButton.onClick.RemoveAllListeners();
        }

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
        }

        private void OnAdvancedSettingsButtonClicked()
        {
            NewGameMenu.OpenPanel(AdvancedSettingsPanel);
        }
    }
}