using System;
using UnityEngine;

namespace TicTacToe3D
{
    public class Player : IEquatable<Player>
    {
        public string Name { get; set; }
        public Color Color { get; set; }

        public Player() : this(string.Empty, Color.clear)
        { }

        public Player(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public bool IsValid()
        {
            return Name != string.Empty && Color != Color.clear;
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