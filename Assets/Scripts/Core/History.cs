using System;
using System.Collections.Generic;
using Zenject;

namespace TicTacToe3D
{
    public class HistoryItem
    {
        public string PlayerName { get; set; }
        public Point BadgeCoordinates { get; set; }
        public int PlayerMadeSteps { get; set; }
        public int GlobalStep { get; set; }
    }

    public class History : IInitializable, IDisposable
    {
        private readonly List<HistoryItem> _historyItems  = new List<HistoryItem>();
        private GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }
        
        public History(GameInfo info,
            GameEvents gameEvents)
        {
            Info = info;
            GameEvents = gameEvents;
        }

        public void Initialize()
        {
            GameEvents.BadgeSpawned += OnBadgeSpawned;
        }

        public void Dispose()
        {
            GameEvents.BadgeSpawned -= OnBadgeSpawned;
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
                PlayerMadeSteps = Info.ActivePlayerMadeSteps,
                GlobalStep = Info.GlobalStep
            });
        }
    }
}