using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class ActivePlayerHandler : IInitializable, IDisposable
    {
        private bool _playerCanWin;

        private GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }
        private History History { get; set; }

        public ActivePlayerHandler(GameInfo info, GameEvents gameEvents, History history)
        {
            Info = info;
            GameEvents = gameEvents;
            History = history;
        }

        public void Initialize()
        {
            Info.GlobalStep = 1;
            NextActivePlayer();

            GameEvents.BadgeSpawned += OnBadgeSpawned;
            GameEvents.StepConfirmed += OnStepConfirmed;
            GameEvents.UndoSignal += OnUndo;
            GameEvents.PlayerLostSignal += OnPlayerLost;
        }

        public void Dispose()
        {
            GameEvents.BadgeSpawned -= OnBadgeSpawned;
            GameEvents.StepConfirmed -= OnStepConfirmed;
            GameEvents.UndoSignal -= OnUndo;
            GameEvents.PlayerLostSignal += OnPlayerLost;
        }

        private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
        {
            _playerCanWin = isVictorious;

            if (Info.GameSettings.ConfirmStep == false && Info.ActivePlayerMadeSteps >= Info.StepSize)
            {
                GameEvents.StepConfirmed();
            }
        }

        private void OnStepConfirmed()
        {
            if (_playerCanWin)
            {
                GameEvents.PlayerWonSignal(Info.ActivePlayer);

                if (Info.GameSettings.GameOverAfterFirstWinner)
                {
                    return;
                }
            }

            if (Info.Players.Count(player => player.State == PlayerStates.Plays) >= 2)
            {
                NextActivePlayer();
            }
        }

        private void OnUndo(List<HistoryItem> canceledSteps)
        {
            if (History.Peek() == null)
            {
                Info.GlobalStep = 1;
                Info.ActivePlayerMadeSteps = 0;
                Info.ActivePlayer = Info.Players.First();
                return;
            }

            var last = canceledSteps[canceledSteps.Count - 1];
            Info.GlobalStep = last.GlobalStep;
            Info.ActivePlayerMadeSteps = last.PlayerMadeSteps - 1;
            Info.ActivePlayer = Info.Players.First(x => x.Name == last.PlayerName);
        }
        
        private void OnPlayerLost(Player loser)
        {
            if (Info.Players.Count(player => player.State == PlayerStates.Plays) >= 2)
            {
                NextActivePlayer();
            }
        }

        private void NextActivePlayer()
        {
            if (Info.ActivePlayer == null)
            {
                Info.ActivePlayer = Info.Players.First();
                return;
            }

            Info.ActivePlayerMadeSteps = 0;
            var playerIndex = Info.Players.IndexOf(Info.ActivePlayer);
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
                    Info.ActivePlayer = Info.Players[nextPlayerIndex];
                    return;
                }

                nextPlayerIndex++;
            }
        }
    }
}