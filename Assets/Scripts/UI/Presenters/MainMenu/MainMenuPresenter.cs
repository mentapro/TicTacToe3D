using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Zenject;

namespace TicTacToe3D
{
    public class MainMenuPresenter : MenuPresenter<MainMenuView>, IInitializable, IDisposable
    {
        private MenuManager MenuManager { get; set; }

        public MainMenuPresenter(MenuManager menuManager, AudioController audioController) : base(audioController)
        {
            MenuManager = menuManager;

            MenuManager.SetMenu(this);
        }

        public void Initialize()
        {
            View.NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
            View.LoadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
            View.HighscoresButton.onClick.AddListener(OnHighscoresButtonClicked);
            View.ExitButton.onClick.AddListener(OnExitButtonClicked);

            MenuManager.OpenMenu(Menus.MainMenu);
        }

        public void Dispose()
        {
            View.NewGameButton.onClick.RemoveAllListeners();
            View.LoadGameButton.onClick.RemoveAllListeners();
            View.HighscoresButton.onClick.RemoveAllListeners();
            View.ExitButton.onClick.RemoveAllListeners();
        }

        private void OnNewGameButtonClicked()
        {
            MenuManager.OpenMenu(Menus.NewGameMenu);
        }

        private void OnLoadGameButtonClicked()
        {
            MenuManager.OpenMenu(Menus.LoadGameMenu);
        }

        private void OnHighscoresButtonClicked()
        {
            MenuManager.OpenMenu(Menus.HighscoresMenu);
        }

        private void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}