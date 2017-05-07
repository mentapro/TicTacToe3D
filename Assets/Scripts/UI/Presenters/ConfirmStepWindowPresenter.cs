using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class ConfirmStepWindowPresenter : MenuPresenter<ConfirmStepWindowView>, IInitializable, IDisposable
    {
        private MenuManager MenuManager { get; set; }
        private GameInfo Info { get; set; }
        private BadgeModel.Registry BadgeRegistry { get; set; }
        private GameEvents GameEvents { get; set; }
        private History History { get; set; }

        public ConfirmStepWindowPresenter(MenuManager menuManager,
            GameInfo info,
            BadgeModel.Registry badgeRegistry,
            GameEvents gameEvents,
            History history)
        {
            MenuManager = menuManager;
            Info = info;
            BadgeRegistry = badgeRegistry;
            GameEvents = gameEvents;
            History = history;

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
            GameEvents.TimePassed += OnTimePassed;
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
            UndoUnconfirmedBadges();
            MenuManager.CloseMenu(Menus.ConfirmStepWindow);
        }

        private void UndoUnconfirmedBadges()
        {
            var unconfirmedBadges = BadgeRegistry.Badges.Where(x => x.IsConfirmed == false).ToList();
            if (unconfirmedBadges.Count == 0)
            {
                return;
            }

            var canceledSteps = new List<HistoryItem>();
            for (var i = unconfirmedBadges.Count - 1; i >= 0; i--)
            {
                canceledSteps.Add(History.Pop());
                unconfirmedBadges[i].Destroy();
            }
            GameEvents.UndoSignal(canceledSteps);
        }
    }
}