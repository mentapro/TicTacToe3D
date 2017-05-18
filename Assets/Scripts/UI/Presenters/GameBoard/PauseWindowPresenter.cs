using System;
using System.ComponentModel;
using UnityEngine;
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

        public PauseWindowPresenter(GameInfo info,
            MenuManager menuManager)
        {
            Info = info;
            MenuManager = menuManager;

            menuManager.SetMenu(this);
        }

        public void Initialize()
        {
            Info.PropertyChanged += OnGameInfoPropertyChanged;

            View.ResumeButton.onClick.AddListener(OnResumeButtonClicked);
            View.SaveGameButton.onClick.AddListener(OnSaveGameButtonClicked);
        }

        public void Dispose()
        {
            Info.PropertyChanged -= OnGameInfoPropertyChanged;

            View.ResumeButton.onClick.RemoveAllListeners();
            View.SaveGameButton.onClick.RemoveAllListeners();
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

        }

        private void OnSettingsButtonClicked()
        {

        }

        private void OnExitToMenuButtonClicked()
        {

        }

        private void OnExitGameButtonClicked()
        {

        }
    }
}