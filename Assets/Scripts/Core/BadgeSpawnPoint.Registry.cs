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
            private GameEvents GameEvents { get; set; }

            public Registry(GameEvents gameEvents)
            {
                GameEvents = gameEvents;
            }

            public IEnumerable<BadgeSpawnPoint> Spawns
            {
                get { return _spawnPoints; }
            }

            public int SpawnsCount
            {
                get { return _spawnPoints.Count; }
            }
            
            public void AddSpawn(BadgeSpawnPoint spawn)
            {
                _spawnPoints.Add(spawn);
            }

            public void Initialize()
            {
                GameEvents.BadgeSpawned += OnBadgeSpawned;
                GameEvents.UndoSignal += OnUndo;
            }

            public void Dispose()
            {
                GameEvents.BadgeSpawned -= OnBadgeSpawned;
                GameEvents.UndoSignal -= OnUndo;
            }

            private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
            {
                _spawnPoints.First(x => x.Coordinates == badge.Coordinates).Badge = badge;
            }

            private void OnUndo(List<HistoryItem> canceledSteps)
            {
                foreach (var canceledStep in canceledSteps)
                {
                    _spawnPoints.First(x => x.Coordinates == canceledStep.BadgeCoordinates).Badge = null;
                }
            }
        }
    }
}