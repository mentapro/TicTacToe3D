using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Zenject;

namespace TicTacToe3D
{
    public class PauseWindowPresenter : MenuPresenter<PauseWindowView>, IInitializable, ITickable, IDisposable
    {
        private bool _tick;
        private bool _isPauseOpen;
        private GameStates _previousState;

        private GameInfo Info { get; set; }
        private MenuManager MenuManager { get; set; }
        private AudioController AudioController { get; set; }

        public PauseWindowPresenter(GameInfo info,
            MenuManager menuManager,
            AudioController audioController) : base(audioController)
        {
            Info = info;
            MenuManager = menuManager;
            AudioController = audioController;

            menuManager.SetMenu(this);
        }

        public void Initialize()
        {
            Info.PropertyChanged += OnGameInfoPropertyChanged;

            View.ResumeButton.onClick.AddListener(OnResumeButtonClicked);
            View.SaveGameButton.onClick.AddListener(OnSaveGameButtonClicked);
            View.LoadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
            View.ExitToMenuButton.onClick.AddListener(OnExitToMenuButtonClicked);
            View.ExitGameButton.onClick.AddListener(OnExitGameButtonClicked);
        }

        public void Dispose()
        {
            Info.PropertyChanged -= OnGameInfoPropertyChanged;

            View.ResumeButton.onClick.RemoveAllListeners();
            View.SaveGameButton.onClick.RemoveAllListeners();
            View.LoadGameButton.onClick.RemoveAllListeners();
            View.ExitToMenuButton.onClick.RemoveAllListeners();
            View.ExitGameButton.onClick.RemoveAllListeners();
        }

        public void Tick()
        {
            if (_tick == false)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && _isPauseOpen == false)
            {
                _previousState = Info.GameState;
                _isPauseOpen = true;
                Info.GameState = GameStates.Paused;
                MenuManager.OpenMenu(Menus.PauseWindow);
            }
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                OnGameStateChanged(Info.GameState);
            }
        }

        private void OnGameStateChanged(GameStates state)
        {
            _tick = state == GameStates.Started || state == GameStates.Paused;
        }

        private void OnResumeButtonClicked()
        {
            MenuManager.CloseMenu(Menus.PauseWindow);
            _isPauseOpen = false;
            if (_previousState == GameStates.Started)
            {
                Info.GameState = GameStates.Started;
            }
        }

        private void OnSaveGameButtonClicked()
        {
            MenuManager.OpenMenu(Menus.SaveGameWindow);
        }

        private void OnLoadGameButtonClicked()
        {
            MenuManager.OpenMenu(Menus.LoadGameWindow);
        }

        private void OnSettingsButtonClicked()
        {

        }

        private void OnExitToMenuButtonClicked()
        {
            AudioController.SourceBackground.Stop();
            SceneManager.LoadScene("MainMenu");
        }

        private void OnExitGameButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}