using System;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class VictoryHandler : IInitializable, IDisposable
    {
        private GameInfo Info { get; set; }
        private GameSettings GameSettings { get; set; }
        private PlayerWon PlayerWon { get; set; }

        public VictoryHandler(GameInfo info,
            GameSettings gameSettings,
            PlayerWon playerWon)
        {
            Info = info;
            GameSettings = gameSettings;
            PlayerWon = playerWon;
        }

        public void Initialize()
        {
            PlayerWon += OnPlayerWon;
        }
        
        public void Dispose()
        {
            PlayerWon -= OnPlayerWon;
        }

        private void OnPlayerWon(Player winner)
        {
            winner.State = PlayerStates.Winner;

            if (GameSettings.GameOverAfterFirstWinner)
            {
                GameOver();
            }
            else
            {
                if (Info.Players.Count(player => player.State == PlayerStates.Plays) == 1)
                {
                    GameOver();
                }
            }
        }

        private void GameOver()
        {
            foreach (var player in Info.Players.Where(x => x.State == PlayerStates.Plays))
            {
                player.State = PlayerStates.Loser;
            }
            Info.CurrentState = GameStates.GameEnded;
        }
    }
}