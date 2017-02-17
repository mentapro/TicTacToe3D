using System;
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
        Loser,
        Disconnected
    }

    public class Player : IEquatable<Player>
    {
        public PlayerTypes Type { get; private set; }
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public PlayerStates State { get; set; }
        public float TimeLeft { get; set; }

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
    }
}