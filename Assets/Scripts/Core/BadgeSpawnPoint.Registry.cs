using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public partial class BadgeSpawnPoint
    {
        public class Registry : IInitializable, IDisposable
        {
            private readonly List<BadgeSpawnPoint> _spawnPoints = new List<BadgeSpawnPoint>();
            private BadgeSpawned BadgeSpawned { get; set; }

            public Registry(BadgeSpawned badgeSpawned)
            {
                BadgeSpawned = badgeSpawned;
            }

            public IEnumerable<BadgeSpawnPoint> Spawns
            {
                get { return _spawnPoints; }
            }

            public int SpawnsCount
            {
                get { return _spawnPoints.Count; }
            }
            
            public void AddSpawn(BadgeSpawnPoint row)
            {
                _spawnPoints.Add(row);
            }

            public void Initialize()
            {
                BadgeSpawned += OnBadgeSpawned;
            }

            public void Dispose()
            {
                BadgeSpawned -= OnBadgeSpawned;
            }

            private void OnBadgeSpawned(BadgeModel badge)
            {
                _spawnPoints.First(x => x.Coordinates == badge.Coordinates).Badge = badge;
            }
        }
    }
}