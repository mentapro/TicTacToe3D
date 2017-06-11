using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class VictoryHandler : IInitializable, IDisposable
    {
        private readonly Dictionary<Player, List<BadgeModel>> _winnersCatalog = new Dictionary<Player, List<BadgeModel>>();
        private GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }
        private BadgeModel.Registry BadgeRegistry { get; set; }
        private AudioController AudioController { get; set; }

        public VictoryHandler(GameInfo info,
            GameEvents gameEvents,
            BadgeModel.Registry badgeRegistry,
            AudioController audioController)
        {
            Info = info;
            GameEvents = gameEvents;
            BadgeRegistry = badgeRegistry;
            AudioController = audioController;
        }

        public void Initialize()
        {
            if (Info.HistoryItems == null) // if game is not loading
            {
                Info.Players.ForEach(player => player.State = PlayerStates.Plays);
            }
            else
            {
                FillWinnersCatalogAfterLoading();
            }
            StartGame();

            GameEvents.PlayerWonSignal += OnPlayerWon;
            GameEvents.PlayerLostSignal += OnPlayerLost;
            GameEvents.TimePassed += OnTimePassed;
            GameEvents.StepConfirmed += OnStepConfirmed;
        }

        public void Dispose()
        {
            GameEvents.PlayerWonSignal -= OnPlayerWon;
            GameEvents.PlayerLostSignal -= OnPlayerLost;
            GameEvents.TimePassed -= OnTimePassed;
            GameEvents.StepConfirmed -= OnStepConfirmed;
        }

        private void OnStepConfirmed()
        {
            if (BadgeRegistry.BadgesCount >= Info.Dimension * Info.Dimension * Info.Dimension)
            {
                GameOver();
            }
        }

        private void OnTimePassed()
        {
            Info.ActivePlayer.State = PlayerStates.Loser;
            GameEvents.PlayerLostSignal();
        }

        private void OnPlayerLost()
        {
            if (Info.Players.Count(player => player.State == PlayerStates.Plays) == 1)
            {
                var winner = Info.Players.First(player => player.State == PlayerStates.Plays);
                winner.State = PlayerStates.Winner;
                GameOver();
            }
        }

        private IEnumerator Coroutine(Action action, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            action.Invoke();
        }

        private void OnPlayerWon()
        {
            Info.ActivePlayer.State = PlayerStates.Winner;
            AudioController.StartCoroutine(Coroutine(() =>
            {
                if (Info.GameState == GameStates.GameEnded) return;
                AudioController.Source.PlayOneShot(AudioController.AudioSettings.OnPlayerWon);
            }, 0.2f));

            var victoryLine = FindVictoryLine();
            _winnersCatalog.Add(Info.ActivePlayer, victoryLine);

            if (Info.GameSettings.GameOverAfterFirstWinner)
            {
                GameOver();
            }
            else
            {
                if (Info.Players.Count(player => player.State == PlayerStates.Plays) == 1)
                {
                    AudioController.StartCoroutine(Coroutine(GameOver, 0.1f)); 
                }
                else
                {
                    GlowBadgesOnce(victoryLine);
                }
            }
        }

        private List<BadgeModel> FindVictoryLine()
        {
            foreach (var line in Info.GameGeometry.Lines)
            {
                if (line.All(point => BadgeRegistry.Badges.Any(x => x.Coordinates == point && x.Owner == Info.ActivePlayer)))
                {
                    return BadgeRegistry.Badges.Where(badge => line.Contains(badge.Coordinates)).ToList();
                }
            }
            return null;
        }

        private void FillWinnersCatalogAfterLoading()
        {
            foreach (var player in Info.Players)
            {
                foreach (var line in Info.GameGeometry.Lines)
                {
                    if (line.All(point => BadgeRegistry.Badges.Any(x => x.Coordinates == point && x.Owner == player && x.IsConfirmed)))
                    {
                        _winnersCatalog.Add(player, BadgeRegistry.Badges.Where(badge => line.Contains(badge.Coordinates)).ToList());
                    }
                }
            }
        }

        private void GameOver()
        {
            foreach (var player in Info.Players.Where(x => x.State == PlayerStates.Plays))
            {
                player.State = PlayerStates.Loser;
            }
            Info.GameState = GameStates.GameEnded;
            AudioController.Source.PlayOneShot(AudioController.AudioSettings.Victory);

            GlowWinnersBadges();
        }

        private void GlowWinnersBadges()
        {
            foreach (var entry in _winnersCatalog)
            {
                foreach (var badge in entry.Value)
                {
                    badge.Glowing.Stop();
                    badge.Glowing.Play();
                }
            }
        }

        private void GlowBadgesOnce(List<BadgeModel> targetBadges)
        {
            foreach (var badge in targetBadges)
            {
                badge.Glowing.Emit(7);
            }
        }

        private void StartGame()
        {
            var details = new ModalDialogDetails
            {
                DialogMessage = "Board is ready. Press \"OK\" to continue.",
                Button1 = new ModalDialogButtonDetails
                {
                    Title = "OK",
                    Handler = () =>
                    {
                        Info.HistoryItems = null;
                        Info.GameState = GameStates.Started;
                        ActivePlayerHandler.ShowNotification(Info);
                    }
                }
            };
            ModalDialog.Show(details);
        }
    }
}