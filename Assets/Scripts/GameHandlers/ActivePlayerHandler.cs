using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class ActivePlayerHandler : IInitializable, IDisposable
    {
        private GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }
        private History History { get; set; }
        private AudioController AudioController { get; set; }

        public ActivePlayerHandler(GameInfo info, GameEvents gameEvents, History history, AudioController audioController)
        {
            Info = info;
            GameEvents = gameEvents;
            History = history;
            AudioController = audioController;
        }

        public void Initialize()
        {
            if (Info.HistoryItems == null) // if game is not loading
            {
                Info.GlobalStep = 1;
                NextActivePlayer();
            }

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
            GameEvents.PlayerLostSignal -= OnPlayerLost;
        }

        private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
        {
            Info.PlayerCanWin = isVictorious;

            if (Info.GameSettings.ConfirmStep == false || Info.ActivePlayer.Type == PlayerTypes.AI)
            {
                GameEvents.StepConfirmed();
            }
        }

        private void OnStepConfirmed()
        {
            if (Info.PlayerCanWin)
            {
                GameEvents.PlayerWonSignal();

                if (Info.GameSettings.GameOverAfterFirstWinner)
                {
                    return;
                }
            }

            if (Info.Players.Count(x => x.State == PlayerStates.Plays) >= 2 && (Info.ActivePlayerMadeSteps >= Info.StepSize || Info.PlayerCanWin))
            {
                NextActivePlayer();
                ShowNotification(Info);
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
        
        private void OnPlayerLost()
        {
            if (Info.Players.Count(player => player.State == PlayerStates.Plays) >= 2)
            {
                NextActivePlayer();
                ShowNotification(Info);
            }
        }

        private void NextActivePlayer()
        {
            Info.ActivePlayerMadeSteps = 0;
            if (Info.ActivePlayer == null)
            {
                Info.ActivePlayer = Info.Players.First();
                return;
            }

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
        
        public static void ShowNotification(GameInfo info)
        {
            info.GameState = GameStates.Paused;
            var details = new ModalDialogDetails
            {
                DialogMessage = "Next player: " + info.ActivePlayer.Name,
                Button1 = new ModalDialogButtonDetails
                {
                    Title = "OK",
                    Handler = () => info.GameState = GameStates.Started
                }
            };
            ModalDialog.Show(details);
        }
    }
}