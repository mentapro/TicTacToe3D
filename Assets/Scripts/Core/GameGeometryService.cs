using System.Collections.Generic;
using System.Linq;

namespace TicTacToe3D
{
    public class GameGeometryService
    {
        public List<Point> Points { get; private set; }
        public List<List<Point>> Lines { get; private set; }

        public void Update(int dimension, int badgesToWin)
        {
            Points = GetAllPoints(dimension).ToList();
            Lines = GetAllLines(dimension, badgesToWin);
        }

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