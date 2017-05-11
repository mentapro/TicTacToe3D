using System;
using Zenject;

namespace TicTacToe3D
{
    public class ConfirmStepWindowPresenter : MenuPresenter<ConfirmStepWindowView>, IInitializable, IDisposable
    {
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

            GameEvents.BadgeSpawned += OnBadgeSpawned;
            GameEvents.TimePassed += OnTimePassed;
        }

        public void Dispose()
        {
            View.ConfirmStepButton.onClick.RemoveAllListeners();
            View.UndoStepButton.onClick.RemoveAllListeners();

            GameEvents.BadgeSpawned -= OnBadgeSpawned;
            GameEvents.TimePassed -= OnTimePassed;
        }

        private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
        {
            if (Info.GameSettings.ConfirmStep)
            {
                MenuManager.OpenMenu(Menus.ConfirmStepWindow);
            }
        }

        private void OnConfirmStepClicked()
        {
            GameEvents.StepConfirmed();
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
        }

        private void OnTimePassed()
        {
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
        }

        private void OnUndoClicked()
        {
            BadgeEraser.UndoUnconfirmedBadges();
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
        }
    }
}