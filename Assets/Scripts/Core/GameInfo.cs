using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    [Serializable]
    public class GameSettings
    {
        public bool GameOverAfterFirstWinner;
        public bool ConfirmStep;
    }

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
        private Player _activePlayer;
        private GameStates _gameState;
        
        public GameInfo(GameSettings gameSettings)
        {
            GameSettings = gameSettings;

            Dimension = 4;
            BadgesToWin = 4;
            StepSize = 2;
            Players = new List<Player>
            {
                new Player(PlayerTypes.Human, "Player 1", UnityEngine.Color.red),
                new Player(PlayerTypes.Human, "Player 2", UnityEngine.Color.blue),
                new Player(PlayerTypes.Human, "Player 3", UnityEngine.Color.green)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public GameSettings GameSettings { get; private set; }
        public List<Player> Players { get; set; }
        public int GlobalStep { get; set; }

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
                Points = GetAllPoints(_dimension).ToList();
                Lines = GetAllLines(_dimension, _badgesToWin);
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
                Lines = GetAllLines(_dimension, _badgesToWin);
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
        
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public List<Point> Points { get; private set; }
        public List<List<Point>> Lines { get; private set; }

        private List<List<Point>> GetAllLines(int dimension, int badgesToWin)
        {
            Point[] directions =
            {
                new Point(1, 0, 0),
                new Point(0, 1, 0),
                new Point(0, 0, 1),
                new Point(1, 0, 1),
                new Point(0, 1, 1),
                new Point(1, 1, 0),
                new Point(1, 0, -1),
                new Point(0, -1, 1),
                new Point(-1, 1, 0),
                new Point(1, 1, 1),
                new Point(-1, 1, 1),
                new Point(1, -1, 1),
                new Point(1, 1, -1)
            };
            var lines = new List<List<Point>>();

            foreach (var point in Points)
            {
                foreach (var dir in directions)
                {
                    var direction = dir;
                    for (var i = 0; i < 2; i++)
                    {
                        if (i == 1)
                        {
                            direction = new Point(-direction.X, -direction.Y, -direction.Z);
                        }

                        var line = new List<Point>(badgesToWin)
                        {
                            new Point(point)
                        };

                        var currentPoint = new Point(point);
                        while (currentPoint != null)
                        {
                            currentPoint += direction;

                            if (currentPoint >= 0 && currentPoint < dimension && line.Count < badgesToWin)
                                line.Add(new Point(currentPoint));
                            else currentPoint = null;
                        }
                        if (line.Count == badgesToWin)
                        {
                            lines.Add(line);
                        }
                    }
                }
            }
            return lines.Distinct(new ListCoordinatesEqualityComparer()).ToList();
        }

        private IEnumerable<Point> GetAllPoints(int dimension)
        {
            for (var x = 0; x < dimension; x++)
            {
                for (var y = 0; y < dimension; y++)
                {
                    for (var z = 0; z < dimension; z++)
                    {
                        yield return new Point(x, y, z);
                    }
                }
            }
        }
    }

    internal class ListCoordinatesEqualityComparer : IEqualityComparer<List<Point>>
    {
        public bool Equals(List<Point> x, List<Point> y)
        {
            if (x.Count != y.Count)
            {
                return false;
            }
            return x.TrueForAll(y.Contains) && y.TrueForAll(x.Contains);
        }

        public int GetHashCode(List<Point> obj)
        {
            return obj.Sum(x => x.GetHashCode());
        }
    }
}