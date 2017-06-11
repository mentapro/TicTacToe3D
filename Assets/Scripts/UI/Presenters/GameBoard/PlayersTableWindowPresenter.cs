using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ModestTree;
using Zenject;

namespace TicTacToe3D
{
    public class PlayersTableWindowPresenter : MenuPresenter<PlayersTableWindowView>, IInitializable, IDisposable
    {
        private Player _previousPlayer;

        private GameInfo Info { get; set; }
        private PlayerRowGameFacade.Factory PlayerRowFactory { get; set; }
        private PlayerRowGameModel.Registry PlayerRowRegistry { get; set; }
        private ScoreCalculationService ScoreCalculator { get; set; }
        private GameEvents GameEvents { get; set; }

        public PlayersTableWindowPresenter(GameInfo info,
            PlayerRowGameFacade.Factory playerRowFactory,
            PlayerRowGameModel.Registry playerRowRegistry,
            ScoreCalculationService scoreCalculator,
            GameEvents gameEvents, AudioController audioController) : base(audioController)
        {
            Info = info;
            PlayerRowFactory = playerRowFactory;
            PlayerRowRegistry = playerRowRegistry;
            ScoreCalculator = scoreCalculator;
            GameEvents = gameEvents;
        }

        public void Initialize()
        {
            foreach (var player in Info.Players)
            {
                var playerRow = PlayerRowFactory.Create();
                playerRow.transform.SetParent(View.PlayersRows, false);
                playerRow.ColorImage.color = player.Color;
                playerRow.NameText.text = player.Name;
                playerRow.StateText.text = player.State.ToString();
                playerRow.ScoreAmountText.text = player.Score.ToString();
                playerRow.WonAmountText.text = player.WonRounds.ToString();
                playerRow.Owner = player;
                player.PropertyChanged += OnPlayerPropertyChanged;
            }
            UpdateScores();
            PlayerRowRegistry.Rows.First(row => ReferenceEquals(row.Owner, Info.ActivePlayer)).TurnOnBackground();
            _previousPlayer = Info.ActivePlayer;

            Info.PropertyChanged += OnGameInfoPropertyChanged;
            GameEvents.StepConfirmed += OnStepConfirmed;
            GameEvents.UndoSignal += OnUndo;
        }

        public void Dispose()
        {
            Info.Players.ForEach(player => player.PropertyChanged -= OnPlayerPropertyChanged);
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
            GameEvents.StepConfirmed -= OnStepConfirmed;
            GameEvents.UndoSignal -= OnUndo;
        }

        private void OnStepConfirmed()
        {
            UpdateScores();
        }

        private void OnUndo(List<HistoryItem> canceledbadges)
        {
            UpdateScores();
        }

        private void UpdateScores()
        {
            var scores = ScoreCalculator.CalculateScores();
            foreach (var playerScore in scores)
            {
                playerScore.Key.Score = playerScore.Value;
            }
        }

        private void OnPlayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "State":
                    OnPlayerStateChanged(sender as Player);
                    break;
                case "Score":
                    OnPlayerScoreChanged(sender as Player);
                    break;
                case "WonRounds":
                    OnPlayerWonRoundsChanged(sender as Player);
                    break;
            }
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ActivePlayer":
                    OnActivePlayerChanged(Info.ActivePlayer);
                    break;
                case "GameState":
                    OnGameStateChanged(Info.GameState);
                    break;
            }
        }

        private void OnGameStateChanged(GameStates state)
        {
            if (state == GameStates.GameEnded)
            {
                Info.Players.Where(player => player.State == PlayerStates.Winner).ForEach(x => x.WonRounds++);
            }
        }

        private void OnPlayerWonRoundsChanged(Player player)
        {
            PlayerRowRegistry.Rows.First(row => ReferenceEquals(row.Owner, player)).SetWonRounds(player.WonRounds.ToString());
        }

        private void OnPlayerScoreChanged(Player player)
        {
            PlayerRowRegistry.Rows.First(row => ReferenceEquals(row.Owner, player)).SetScore(player.Score.ToString());
        }

        private void OnPlayerStateChanged(Player player)
        {
            PlayerRowRegistry.Rows.First(row => ReferenceEquals(row.Owner, player)).SetState(player.State.ToString());
        }

        private void OnActivePlayerChanged(Player activePlayer)
        {
            PlayerRowRegistry.Rows.First(row => ReferenceEquals(row.Owner, _previousPlayer)).TurnOffBackground();
            PlayerRowRegistry.Rows.First(row => ReferenceEquals(row.Owner, activePlayer)).TurnOnBackground();
            _previousPlayer = activePlayer;
        }
    }
}