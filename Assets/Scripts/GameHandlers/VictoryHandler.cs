using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class VictoryHandler : IInitializable, IDisposable
    {
        private readonly Dictionary<Player, List<BadgeModel>> _winnersCatalog = new Dictionary<Player, List<BadgeModel>>();
        private GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }
        private BadgeModel.Registry BadgeRegistry { get; set; }

        public VictoryHandler(GameInfo info,
            GameEvents gameEvents,
            BadgeModel.Registry badgeRegistry)
        {
            Info = info;
            GameEvents = gameEvents;
            BadgeRegistry = badgeRegistry;
        }

        public void Initialize()
        {
            GameEvents.PlayerWonSignal += OnPlayerWon;
        }
        
        public void Dispose()
        {
            GameEvents.PlayerWonSignal -= OnPlayerWon;
        }
        
        private void OnPlayerWon()
        {
            Info.ActivePlayer.State = PlayerStates.Winner;

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
                    GameOver();
                }
                else
                {
                    GlowBadgesOnce(victoryLine);
                }
            }
        }

        private List<BadgeModel> FindVictoryLine()
        {
            foreach (var line in Info.Lines)
            {
                if (line.All(point => BadgeRegistry.Badges.Any(x => x.Coordinates == point && x.Owner == Info.ActivePlayer)))
                {
                    return BadgeRegistry.Badges.Where(badge => line.Contains(badge.Coordinates)).ToList();
                }
            }

            throw new InvalidOperationException("Player \"" + Info.ActivePlayer.Name + "\" does not have victory line.");
        }

        private void GameOver()
        {
            foreach (var player in Info.Players.Where(x => x.State == PlayerStates.Plays))
            {
                player.State = PlayerStates.Loser;
            }
            Info.GameState = GameStates.GameEnded;

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
    }
}