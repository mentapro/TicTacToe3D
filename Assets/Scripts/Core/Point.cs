using System;

namespace TicTacToe3D
{
    public class Point : IEquatable<Point>
    {
        public int X, Y, Z;

        public Point()
        { }

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Point);
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 27;
                hash = 13 * hash + X.GetHashCode();
                hash = 13 * hash + Y.GetHashCode();
                hash = 13 * hash + Z.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return ReferenceEquals(null, left) ? ReferenceEquals(null, right) : left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        public static bool operator >=(Point point, int value)
        {
            return point.X >= value && point.Y >= value && point.Z >= value;
        }

        public static bool operator <=(Point point, int value)
        {
            return point.X <= value && point.Y <= value && point.Z <= value;
        }

        public static bool operator >(Point point, int value)
        {
            return point.X > value && point.Y > value && point.Z > value;
        }

        public static bool operator <(Point point, int value)
        {
            return point.X < value && point.Y < value && point.Z < value;
        }

        public static Point operator +(Point point1, Point point2)
        {
            point1.X += point2.X;
            point1.Y += point2.Y;
            point1.Z += point2.Z;
            return point1;
        }

        public static Point operator -(Point point1, Point point2)
        {
            point1.X -= point2.X;
            point1.Y -= point2.Y;
            point1.Z -= point2.Z;
            return point1;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", X, Y, Z);
        }
    }
}