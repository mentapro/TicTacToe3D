using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TicTacToe3D
{
    public enum GameStates
    {
        Preload,
        Started,
        Paused,
        GameEnded,
    }
    
    public class GameInfo : INotifyPropertyChanged
    {
        private int _dimension;
        private int _badgesToWin;
        private int _stepSize;
        private int _activePlayerMadeSteps;
        private int _globalStep;
        private Player _activePlayer;
        private GameStates _gameState;
        private float _timerTime;

        public GameInfo(GameSettings gameSettings)
        {
            GameSettings = gameSettings;
            GameGeometry = new GameGeometryService();
            SetDefaults();
            Reset();
        }

        [JsonIgnore]
        public GameGeometryService GameGeometry { get; private set; }
        public GameSettings GameSettings { get; private set; }
        public List<Player> Players { get; set; }

        public float TimerTime
        {
            get { return _timerTime; }
            set
            {
                if (value.Equals(_timerTime)) return;
                _timerTime = value;
                NotifyPropertyChanged("TimerTime");
            }
        }

        public int ActivePlayerMadeSteps
        {
            get { return _activePlayerMadeSteps; }
            set
            {
                if (value == _activePlayerMadeSteps) return;
                _activePlayerMadeSteps = value;
                NotifyPropertyChanged("ActivePlayerMadeSteps");
            }
        }

        public GameStates GameState
        {
            get { return _gameState; }
            set
            {
                if (value == _gameState) return;
                _gameState = value;
                NotifyPropertyChanged("GameState");
            }
        }

        public Player ActivePlayer
        {
            get { return _activePlayer; }
            set
            {
                if (value == _activePlayer) return;
                _activePlayer = value;
                NotifyPropertyChanged("ActivePlayer");
            }
        }

        public int Dimension
        {
            get { return _dimension; }
            set
            {
                if (value == _dimension) return;
                _dimension = value;
                NotifyPropertyChanged("Dimension");
            }
        }

        public int BadgesToWin
        {
            get { return _badgesToWin; }
            set
            {
                if (value == _badgesToWin) return;
                _badgesToWin = value;
                NotifyPropertyChanged("BadgesToWin");
            }
        }

        public int StepSize
        {
            get { return _stepSize; }
            set
            {
                if (value == _stepSize) return;
                _stepSize = value;
                NotifyPropertyChanged("StepSize");
            }
        }

        public int GlobalStep
        {
            get { return _globalStep; }
            set
            {
                if (value == _globalStep) return;
                _globalStep = value;
                NotifyPropertyChanged("GlobalStep");
            }
        }

        public bool PlayerCanWin { get; set; }

        [JsonIgnore]
        public List<HistoryItem> HistoryItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetDefaults()
        {
            Dimension = 4;
            BadgesToWin = 4;
            StepSize = 2;
            TimerTime = 10;
            Players = new List<Player>
            {
                new Player(PlayerTypes.Human, "Player 1", UnityEngine.Color.red),
                new Player(PlayerTypes.AI, "Player 2", UnityEngine.Color.blue),
                new Player(PlayerTypes.Human, "Player 3", UnityEngine.Color.green)
            };
        }

        public void Reset()
        {
            GameState = GameStates.Preload;
            if (HistoryItems == null) // if game is not loading
            {
                ActivePlayer = null;
            }
            GameGeometry.Update(_dimension, _badgesToWin);
        }
    }
}