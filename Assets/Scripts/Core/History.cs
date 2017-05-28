using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject;

namespace TicTacToe3D
{
    public class HistoryItem
    {
        public string PlayerName { get; set; }
        public Point BadgeCoordinates { get; set; }
        public bool IsBadgeConfirmed { get; set; }
        public int PlayerMadeSteps { get; set; }
        public int GlobalStep { get; set; }
    }

    public class History : IInitializable, IDisposable
    {
        private readonly List<HistoryItem> _historyItems = new List<HistoryItem>();
        public GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }

        public List<HistoryItem> HistoryItems
        {
            get { return _historyItems; }
        }

        public History(GameInfo info,
            GameEvents gameEvents)
        {
            Info = info;
            GameEvents = gameEvents;
        }

        public void Initialize()
        {
            GameEvents.BadgeSpawned += OnBadgeSpawned;
            GameEvents.StepConfirmed += OnStepConfirmed;
        }

        public void Dispose()
        {
            GameEvents.BadgeSpawned -= OnBadgeSpawned;
            GameEvents.StepConfirmed -= OnStepConfirmed;
        }

        public void Push(HistoryItem item)
        {
            _historyItems.Add(item);
        }

        public HistoryItem Pop()
        {
            if (_historyItems.Count == 0)
            {
                return null;
            }

            var lastItem = _historyItems[_historyItems.Count - 1];
            _historyItems.Remove(lastItem);
            return lastItem;
        }

        public HistoryItem Peek()
        {
            return _historyItems.Count == 0 ? null : _historyItems[_historyItems.Count - 1];
        }

        private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
        {
            _historyItems.Add(new HistoryItem
            {
                PlayerName = badge.Owner.Name,
                BadgeCoordinates = badge.Coordinates,
                IsBadgeConfirmed = badge.IsConfirmed,
                PlayerMadeSteps = Info.ActivePlayerMadeSteps,
                GlobalStep = Info.GlobalStep
            });
        }

        private void OnStepConfirmed()
        {
            _historyItems.Where(item => item.IsBadgeConfirmed == false).ForEach(x => x.IsBadgeConfirmed = true);
        }
    }
}