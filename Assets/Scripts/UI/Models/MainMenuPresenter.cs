using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Zenject;

namespace TicTacToe3D
{
    public interface IMenuPresenter
    {
        void Open();
        void Close();
    }

    public class MainMenuPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private MainMenuView _view;
        private MenuManager _menuManager;
        private NewGameMenuPresenter _newGameMenu;

        public MainMenuPresenter(MenuManager menuManager, NewGameMenuPresenter newGameMenu)
        {
            _menuManager = menuManager;
            _newGameMenu = newGameMenu;
        }

        public void SetView(MainMenuView view)
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
            _view.ExitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnNewGameButtonClicked()
        {
            _menuManager.OpenMenu(_newGameMenu);
        }

        private void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void Dispose()
        {
            _view.NewGameButton.onClick.RemoveAllListeners();
            _view.ExitButton.onClick.RemoveAllListeners();
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