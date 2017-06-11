using System;
using System.ComponentModel;
using System.Linq;
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
        private BadgeModel.Registry BadgeRegistry { get; set; }
        private AudioController AudioController { get; set; }

        public ConfirmStepWindowPresenter(MenuManager menuManager,
            GameInfo info,
            BadgeEraser badgeEraser,
            GameEvents gameEvents,
            BadgeModel.Registry badgeRegistry,
            AudioController audioController) : base(audioController)
        {
            MenuManager = menuManager;
            Info = info;
            BadgeEraser = badgeEraser;
            GameEvents = gameEvents;
            BadgeRegistry = badgeRegistry;
            AudioController = audioController;

            menuManager.SetMenu(this);
        }

        public void Initialize()
        {
            if (Info.HistoryItems != null && BadgeRegistry.Badges.Count(badge => badge.IsConfirmed == false) > 0)
            {
                MenuManager.OpenMenu(Menus.ConfirmStepWindow);
                _isOpen = true;
            }
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
            AudioController.Source.PlayOneShot(AudioController.AudioSettings.ConfirmStepClip);
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
            AudioController.Source.PlayOneShot(AudioController.AudioSettings.UndoBadgesClip);
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
            _isOpen = false;
        }
    }
}