using System;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class WinDetector : IInitializable, IDisposable
    {
        private GameInfo Info { get; set; }
        private BadgeModel.Registry BadgeRegistry { get; set; }
        private BadgeSpawned BadgeSpawned { get; set; }
        private PlayerWon PlayerWon { get; set; }

        public WinDetector(GameInfo info,
            BadgeModel.Registry badgeRegistry,
            BadgeSpawned badgeSpawned,
            PlayerWon playerWon)
        {
            Info = info;
            BadgeRegistry = badgeRegistry;
            BadgeSpawned = badgeSpawned;
            PlayerWon = playerWon;
        }

        public void Initialize()
        {
            BadgeSpawned += OnBadgeSpawned;
        }

        public void Dispose()
        {
            BadgeSpawned -= OnBadgeSpawned;
        }

        private void OnBadgeSpawned(BadgeModel spawnedBadge)
        {
            var lines = Info.Lines.Where(line => line.Contains(spawnedBadge.Coordinates));
            if (lines.Any(line => line.All(point => BadgeRegistry.Badges.Any(badge => badge.Coordinates == point && badge.Owner == spawnedBadge.Owner))))
            {
                PlayerWon.Fire(spawnedBadge.Owner);
            }
        }
    }
}