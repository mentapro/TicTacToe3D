using System;
using System.Linq;
using ModestTree;
using Zenject;

namespace TicTacToe3D
{
    public class ActivePlayerHandler : IInitializable, IDisposable
    {
        private int MadeSteps { get; set; }

        private GameInfo Info { get; set; }
        private BadgeSpawned BadgeSpawned { get; set; }
        private ActivePlayerChanged ActivePlayerChanged { get; set; }

        public ActivePlayerHandler(GameInfo info,
            BadgeSpawned badgeSpawned,
            ActivePlayerChanged activePlayerChanged)
        {
            Info = info;
            BadgeSpawned = badgeSpawned;
            ActivePlayerChanged = activePlayerChanged;

            BadgeSpawned += OnBadgeSpawned;
        }

        public void Initialize()
        {
            MadeSteps = 0;
            UpdateActivePlayer();
        }

        public void Dispose()
        {
            BadgeSpawned -= OnBadgeSpawned;
        }

        private void OnBadgeSpawned(BadgeModel badge)
        {
            MadeSteps++;
            if (Info.Players.Count(player => player.State == PlayerStates.Plays) >= 2)
            {
                UpdateActivePlayer();
            }
        }

        private void UpdateActivePlayer()
        {
            Assert.That(Info.Players.Count(player => player.State == PlayerStates.Plays) >= 2);
            Assert.That(Info.Players.Count(player => player.IsActive) <= 1);

            var activePlayer = Info.Players.FirstOrDefault(x => x.IsActive);

            if (activePlayer == null)
            {
                activePlayer = Info.Players.First();
                activePlayer.IsActive = true;
                ActivePlayerChanged.Fire(activePlayer);
                return;
            }

            if (MadeSteps < Info.StepSize)
            {
                return;
            }

            activePlayer.IsActive = false;
            var playerIndex = Info.Players.IndexOf(activePlayer);
            var nextPlayerIndex = playerIndex + 1;
            while (true)
            {
                if (nextPlayerIndex == Info.Players.Count)
                {
                    nextPlayerIndex = 0;
                    Info.GlobalStep++;
                }

                if (Info.Players[nextPlayerIndex].State == PlayerStates.Plays)
                {
                    activePlayer = Info.Players[nextPlayerIndex];
                    activePlayer.IsActive = true;
                    ActivePlayerChanged.Fire(activePlayer);
                    return;
                }

                nextPlayerIndex++;
            }
        }
    }
}