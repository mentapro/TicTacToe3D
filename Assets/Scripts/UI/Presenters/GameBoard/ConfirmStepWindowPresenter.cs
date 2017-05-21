using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class ConfirmStepWindowPresenter : MenuPresenter<ConfirmStepWindowView>, IInitializable, ITickable, IDisposable
    {
        private bool _tick;
        private bool _isOpen;

        private MenuManager MenuManager { get; set; }
        private GameInfo Info { get; set; }
        private BadgeEraser BadgeEraser { get; set; }
        private GameEvents GameEvents { get; set; }

        public ConfirmStepWindowPresenter(MenuManager menuManager,
            GameInfo info,
            BadgeEraser badgeEraser,
            GameEvents gameEvents)
        {
            MenuManager = menuManager;
            Info = info;
            BadgeEraser = badgeEraser;
            GameEvents = gameEvents;

            menuManager.SetMenu(this);
        }

        public void Initialize()
        {
            View.ConfirmStepButton.onClick.AddListener(OnConfirmStepClicked);
            View.UndoStepButton.onClick.AddListener(OnUndoClicked);

            Info.PropertyChanged += OnGameInfoPropertyChanged;
            GameEvents.BadgeSpawned += OnBadgeSpawned;
            GameEvents.TimePassed += OnTimePassed;
        }

        public void Dispose()
        {
            View.ConfirmStepButton.onClick.RemoveAllListeners();
            View.UndoStepButton.onClick.RemoveAllListeners();

            Info.PropertyChanged -= OnGameInfoPropertyChanged;
            GameEvents.BadgeSpawned -= OnBadgeSpawned;
            GameEvents.TimePassed -= OnTimePassed;
        }

        public void Tick()
        {
            if (_tick == false)
            {
                return;
            }

            if (_isOpen)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    OnConfirmStepClicked();
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
            _tick = state == GameStates.Started;
        }

        private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
        {
            if (Info.GameSettings.ConfirmStep && badge.Owner.Type != PlayerTypes.AI)
            {
                MenuManager.OpenMenu(Menus.ConfirmStepWindow);
                _isOpen = true;
            }
        }

        private void OnConfirmStepClicked()
        {
            GameEvents.StepConfirmed();
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
            _isOpen = false;
        }

        private void OnTimePassed()
        {
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
            _isOpen = false;
        }

        private void OnUndoClicked()
        {
            BadgeEraser.UndoUnconfirmedBadges();
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
            _isOpen = false;
        }
    }
}