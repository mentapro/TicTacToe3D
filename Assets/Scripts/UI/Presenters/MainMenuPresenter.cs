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
        private MainMenuView View { get; set; }
        private MenuManager MenuManager { get; set; }
        private NewGameMenuPresenter NewGameMenu { get; set; }
        
        public MainMenuPresenter(MenuManager menuManager)
        {
            MenuManager = menuManager;
        }

        [Inject]
        private void InjectMenus(NewGameMenuPresenter newGameMenu)
        {
            NewGameMenu = newGameMenu;
        }

        public void SetView(MainMenuView view)
        {
            View = view;
        }

        public void Initialize()
        {
            View.NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
            View.ExitButton.onClick.AddListener(OnExitButtonClicked);

            MenuManager.OpenMenu(this);
        }

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
        }

        public void Dispose()
        {
            View.NewGameButton.onClick.RemoveAllListeners();
            View.ExitButton.onClick.RemoveAllListeners();
        }

        private void OnNewGameButtonClicked()
        {
            MenuManager.OpenMenu(NewGameMenu);
        }

        private void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}