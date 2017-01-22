using System;
using Zenject;

namespace TicTacToe3D
{
    public class NewGameMenuPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private NewGameMenuView View { get; set; }
        private MenuManager MenuManager { get; set; }
        private MainMenuPresenter MainMenu { get; set; }
        private IMenuPresenter CurrentPanel { get; set; }

        public NewGameMenuPresenter(MenuManager menuManager)
        {
            MenuManager = menuManager;
        }

        [Inject]
        private void InjectMenus(MainMenuPresenter mainMenu)
        {
            MainMenu = mainMenu;
        }

        public void SetView(NewGameMenuView view)
        {
            View = view;
        }

        public void Initialize()
        {
            View.CancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        public void Dispose()
        {
            View.CancelButton.onClick.RemoveAllListeners();
        }

        public void OpenPanel(IMenuPresenter panel)
        {
            if (CurrentPanel != null)
            {
                CurrentPanel.Close();
            }
            panel.Open();
            CurrentPanel = panel;
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
    }
}