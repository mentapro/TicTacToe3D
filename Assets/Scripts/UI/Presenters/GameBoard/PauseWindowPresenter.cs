using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class PauseWindowPresenter : MenuPresenter<PauseWindowView>, IInitializable, ITickable, IDisposable
    {
        private bool _tick;
        private bool _isOpen;

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
        }

        public void Dispose()
        {
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        public void Tick()
        {
            if (_tick == false)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isOpen)
                {
                    MenuManager.CloseMenu(Menus.PauseWindow);
                    _isOpen = false;
                }
                else
                {
                    MenuManager.OpenMenu(Menus.PauseWindow);
                    _isOpen = true;
                }
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
    }
}