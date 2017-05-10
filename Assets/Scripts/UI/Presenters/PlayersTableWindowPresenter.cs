using System;
using System.ComponentModel;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class PlayersTableWindowPresenter : MenuPresenter<PlayersTableWindowView>, IInitializable, IDisposable
    {
        private Player _previousPlayer;

        private GameInfo Info { get; set; }
        private PlayerRowGameFacade.Factory PlayerRowFactory { get; set; }
        private PlayerRowGameModel.Registry PlayerRowRegistry { get; set; }

        public PlayersTableWindowPresenter(GameInfo info,
            PlayerRowGameFacade.Factory playerRowFactory,
            PlayerRowGameModel.Registry playerRowRegistry)
        {
            Info = info;
            PlayerRowFactory = playerRowFactory;
            PlayerRowRegistry = playerRowRegistry;
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
                playerRow.Owner = player;
                player.PropertyChanged += OnPlayerPropertyChanged;
            }
            PlayerRowRegistry.Rows.First(row => ReferenceEquals(row.Owner, Info.ActivePlayer)).TurnOnBackground();
            _previousPlayer = Info.ActivePlayer;

            Info.PropertyChanged += OnGameInfoPropertyChanged;
        }

        public void Dispose()
        {
            Info.Players.ForEach(player => player.PropertyChanged -= OnPlayerPropertyChanged);
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        private void OnPlayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "State":
                    OnPlayerStateChanged(sender as Player);
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
            }
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