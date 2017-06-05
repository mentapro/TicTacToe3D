using System;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;

namespace TicTacToe3D
{
    public enum PlayerTypes
    {
        Human,
        AI
    }
    
    public enum PlayerStates
    {
        Plays,
        Winner,
        Loser
    }

    public class Player : IEquatable<Player>, INotifyPropertyChanged
    {
        private PlayerStates _state;
        private int _score;
        private float _timeLeft;
        private int _wonRounds;

        public PlayerTypes Type { get; private set; }
        public string Name { get; private set; }
        public Color Color { get; private set; }

        public float TimeLeft
        {
            get { return _timeLeft; }
            set
            {
                if (value.Equals(_timeLeft)) return;
                _timeLeft = value;
                OnPropertyChanged("TimeLeft");
            }
        }

        public PlayerStates State
        {
            get { return _state; }
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged("State");
            }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                if (value == _score) return;
                _score = value;
                OnPropertyChanged("Score");
            }
        }

        public int WonRounds
        {
            get { return _wonRounds; }
            set
            {
                if (value == _wonRounds) return;
                _wonRounds = value;
                OnPropertyChanged("WonRounds");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Player(PlayerTypes type, string name, Color color)
        {
            Type = type;
            Name = name;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public bool Equals(Player other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Color.Equals(other.Color) && Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Color.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Player left, Player right)
        {
            return ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right);
        }

        public static bool operator !=(Player left, Player right)
        {
            return !(left == right);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}