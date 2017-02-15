using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public partial class BadgeModel
    {
        public class Registry : IInitializable, IDisposable
        {
            private readonly List<BadgeModel> _badges = new List<BadgeModel>();
            private GameEvents GameEvents { get; set; }

            public Registry(GameEvents gameEvents)
            {
                GameEvents = gameEvents;
            }

            public IEnumerable<BadgeModel> Badges
            {
                get { return _badges; }
            }

            public int BadgesCount
            {
                get { return _badges.Count; }
            }

            public void AddBadge(BadgeModel badge)
            {
                _badges.Add(badge);
            }

            public void RemoveBadge(BadgeModel badge)
            {
                _badges.Remove(badge);
            }

            public void Initialize()
            {
                GameEvents.BadgeSpawned += OnBadgeSpawned;
                GameEvents.UndoSignal += OnUndo;
                GameEvents.StepConfirmed += OnStepConfirmed;
            }

            public void Dispose()
            {
                GameEvents.BadgeSpawned -= OnBadgeSpawned;
                GameEvents.UndoSignal -= OnUndo;
                GameEvents.StepConfirmed -= OnStepConfirmed;
            }

            private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
            {
                if (_badges.Count - 2 >= 0)
                {
                    _badges[_badges.Count - 2].Glowing.Stop();
                }
                badge.Glowing.Play();
            }

            private void OnStepConfirmed()
            {
                foreach (var badge in _badges.Where(x => x.IsConfirmed == false))
                {
                    badge.IsConfirmed = true;
                }
            }

            private void OnUndo(List<HistoryItem> canceledSteps)
            {
                if (_badges.Count - 1 >= 0)
                {
                    _badges[_badges.Count - 1].Glowing.Play();
                }
            }
        }
    }
}