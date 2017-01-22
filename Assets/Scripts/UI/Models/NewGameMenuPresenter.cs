using System;
using Zenject;

namespace TicTacToe3D
{
    public class NewGameMenuPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private NewGameMenuView _view;
        private MenuManager _menuManager;
        private MainMenuPresenter _mainMenu;

        public NewGameMenuPresenter(MenuManager menuManager, MainMenuPresenter mainMenu)
        {
            _menuManager = menuManager;
            _mainMenu = mainMenu;
        }

        public void SetView(NewGameMenuView view)
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.CancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        private void OnCancelButtonClicked()
        {
            _menuManager.OpenMenu(_mainMenu);
        }

        public void Dispose()
        {
            _view.CancelButton.onClick.RemoveAllListeners();
        }

        public void Open()
        {
            _view.IsOpen = true;
        }

        public void Close()
        {
            _view.IsOpen = false;
        }
    }
}