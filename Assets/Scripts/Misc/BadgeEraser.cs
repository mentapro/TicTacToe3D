using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class BadgeEraser : IInitializable, IDisposable
    {
        private History History { get; set; }
        private BadgeModel.Registry BadgeRegistry { get; set; }
        private GameEvents GameEvents { get; set; }

        public BadgeEraser(History history,
            BadgeModel.Registry badgeRegistry,
            GameEvents gameEvents)
        {
            History = history;
            GameEvents = gameEvents;
            BadgeRegistry = badgeRegistry;
        }

        public void Initialize()
        {
            GameEvents.TimePassed += OnTimePassed;
        }

        public void Dispose()
        {
            GameEvents.TimePassed -= OnTimePassed;
        }

        public bool UndoUnconfirmedBadges()
        {
            var unconfirmedBadges = BadgeRegistry.Badges.Where(x => x.IsConfirmed == false).ToList();
            if (unconfirmedBadges.Count == 0)
            {
                return false;
            }

            var canceledSteps = new List<HistoryItem>();
            for (var i = unconfirmedBadges.Count - 1; i >= 0; i--)
            {
                canceledSteps.Add(History.Pop());
                unconfirmedBadges[i].Destroy();
            }
            GameEvents.UndoSignal(canceledSteps);
            return true;
        }

        private void OnTimePassed()
        {
            UndoUnconfirmedBadges();
        }
    }
}