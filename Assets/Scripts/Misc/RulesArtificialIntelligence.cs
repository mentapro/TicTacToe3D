using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace TicTacToe3D
{
    public interface IArtificialIntelligence
    {
        Point FindBestPoint(Player playerFor);
    }

    public class RulesArtificialIntelligence : IArtificialIntelligence
    {
        private GameInfo Info { get; set; }
        private BadgeModel.Registry BadgeRegistry { get; set; }
        private Player PlayerFor { get; set; }
        private IList<Player> Opponents { get; set; }
        private Dictionary<Point, Player> BadgesField { get; set; }

        public RulesArtificialIntelligence(GameInfo info, BadgeModel.Registry badgeRegistry)
        {
            Info = info;
            BadgeRegistry = badgeRegistry;
        }
        
        public Point FindBestPoint(Player playerFor)
        {
            PlayerFor = playerFor;
            Opponents = Info.Players.Except(new[] { playerFor }).ToList();
            BadgesField = new Dictionary<Point, Player>();

            MapBadgesField(BadgeRegistry.Badges);

            if (Info.Dimension % 2 == 1)
            {
                var centerPoint = new Point(Info.Dimension / 2, Info.Dimension / 2, Info.Dimension / 2);
                if (BadgesField[centerPoint] == null)
                {
                    return centerPoint;
                }
            }
            var emptyPoints = FindEmptyPoints().ToList();
            var bestPoint = Win();
            if (bestPoint != null)
                return bestPoint;
            bestPoint = Block();
            if (bestPoint != null)
                return bestPoint;
            bestPoint = CreateFork();
            if (bestPoint != null)
                return bestPoint;
            bestPoint = TryForceVictoryAndFork(emptyPoints);
            if (bestPoint != null)
                return bestPoint;
            bestPoint = BlockOpponentFork();
            if (bestPoint != null)
                return bestPoint;
            bestPoint = TryCreateFork(emptyPoints);
            if (bestPoint != null)
                return bestPoint;
            bestPoint = TryCreateLine(emptyPoints);
            if (bestPoint != null)
                return bestPoint;
            bestPoint = MakeBestRandomStep(emptyPoints);
            return bestPoint;
        }

        private Point MakeBestRandomStep(List<Point> allowedPoints)
        {
            var lines = new List<List<Point>>();
            //lines.AddRange(Lines.Except(Lines.Where(line => line.Any(dot => Opponents.Contains(BadgesField[dot])))));
            foreach (var line in Info.GameGeometry.Lines)
            {
                if (line.Any(dot => Opponents.Contains(BadgesField[dot])))
                    continue;
                lines.Add(line);
            }
            var maxCount = 0;
            var bestPoints = new List<Point>();
            foreach (var point in Info.GameGeometry.Points)
            {
                var temp = lines.Count(line => line.Contains(point));
                if (temp > maxCount)
                {
                    maxCount = temp;
                }
            }
            foreach (var point in Info.GameGeometry.Points)
            {
                var temp = lines.Count(line => line.Contains(point));
                if (temp == maxCount)
                    bestPoints.Add(point);
            }
            var random = new Random();
            if (lines.Count == 0)
            {
                var emptyPoints = BadgesField.Keys.Where(x => BadgesField[x] == null).Where(allowedPoints.Contains).ToArray();
                return emptyPoints[random.Next(emptyPoints.Length)];
            }
            var allowed = new List<Point>();
            if (allowedPoints.Any(allowedPoint => bestPoints.Contains(allowedPoint)))
            {
                allowed.AddRange(bestPoints.Where(allowedPoints.Contains));
            }
            if (allowed.Count > 0)
                return allowed[random.Next(allowed.Count)];
            return allowedPoints.First();
        }

        private Point TryCreateLine(List<Point> allowedPoints)
        {
            var lines = new List<List<Point>>();
            foreach (var line in Info.GameGeometry.Lines)
            {
                if (line.All(point => BadgesField[point] == null))
                    continue;
                if (line.Any(point => Opponents.Contains(BadgesField[point])))
                    continue;
                lines.Add(line);
            }
            var random = new Random();
            foreach (var line in lines.OrderByDescending(line => line.Count(point => BadgesField[point] != null)))
            {
                if (allowedPoints.Any(allowedPoint => line.Contains(allowedPoint)))
                {
                    var points = line.Where(point => BadgesField[point] == null);
                    var allowed = points.Where(allowedPoints.Contains).ToArray();
                    if (allowed.Length > 0)
                        return allowed[random.Next(allowed.Length)];
                }
            }
            return null;
        }

        private Point TryCreateFork(List<Point> allowedPoints)
        {
            var lines = new List<List<Point>>();
            foreach (var line in Info.GameGeometry.Lines)
            {
                if (line.All(point => BadgesField[point] == null))
                    continue;
                if (line.Any(point => Opponents.Contains(BadgesField[point])))
                    continue;
                if (line.Count(point => BadgesField[point] == PlayerFor) < Info.BadgesToWin - 2)
                    continue;
                lines.Add(line);
            }

            var intersectedLines = new List<List<Point>>();
            foreach (var line in lines)
            {
                foreach (var emptyPoint in line.Where(point => BadgesField[point] == null))
                {
                    foreach (var intersectedLine in Info.GameGeometry.Lines)
                    {
                        if (!intersectedLine.Contains(emptyPoint))
                            continue;
                        if (intersectedLine.Any(point => Opponents.Contains(BadgesField[point])))
                            continue;
                        if (intersectedLine.Count(point => line.Contains(point)) > 1)
                            continue;
                        if (ReferenceEquals(line, intersectedLine)) // redundant code
                            continue;
                        intersectedLines.Add(intersectedLine);
                    }
                }
            }

            var random = new Random();
            foreach (var intersectedLine in intersectedLines.OrderByDescending(line => line.Count(point => BadgesField[point] != null)))
            {
                if (allowedPoints.Any(allowedPoint => intersectedLine.Contains(allowedPoint)))
                {
                    var forkPoint = intersectedLine.Where(point => BadgesField[point] == null).FirstOrDefault(point => lines.Any(line => line.Contains(point)));
                    var points = intersectedLine.Where(point => BadgesField[point] == null).Except(new[] { forkPoint });
                    var allowed = points.Where(allowedPoints.Contains).ToArray();
                    if (allowed.Length > 0)
                        return allowed[random.Next(allowed.Length)];
                }
            }
            return null;
        }

        private Point TryForceVictoryAndFork(List<Point> allowedPoints)
        {
            var bestPoints = new List<Point>();
            foreach (var emptyPoint in FindEmptyPoints())
            {
                BadgesField[emptyPoint] = PlayerFor;
                foreach (var nextEmptyPoint in FindEmptyPointsInLines(FindLinesWithPoint(emptyPoint)))
                {
                    if (VictoryExists(nextEmptyPoint, PlayerFor))
                    {
                        foreach (var remainingEmptyPoint in FindEmptyPointsInLines(FindLinesWithPoint(emptyPoint)))
                        {
                            if (allowedPoints.Contains(emptyPoint) && ForkExists(remainingEmptyPoint, PlayerFor))
                            {
                                bestPoints.Add(emptyPoint);
                            }
                        }
                    }
                }
                BadgesField[emptyPoint] = null;
            }
            if (bestPoints.Count == 0)
                return null;
            var random = new Random();
            return bestPoints[random.Next(bestPoints.Count)];
        }

        private List<PlayerFork> FindPlayersForks()
        {
            var playersForks = new List<PlayerFork>();
            foreach (var opponent in Opponents)
            {
                playersForks.Add(new PlayerFork { Player = opponent });
                foreach (var emptyPoint in FindEmptyPoints())
                {
                    if (ForkExists(emptyPoint, opponent)) // find fork on next step
                    {
                        playersForks.First(x => x.Player == opponent).Forks[emptyPoint] = new List<Point>();
                    }
                }
                foreach (var emptyPoint in FindEmptyPoints())
                {
                    BadgesField[emptyPoint] = opponent;
                    foreach (var nextEmptyPoint in FindEmptyPointsInLines(FindLinesWithPoint(emptyPoint)))
                    {
                        if (VictoryExists(nextEmptyPoint, opponent))
                        {
                            foreach (var remainingEmptyPoint in FindEmptyPointsInLines(FindLinesWithPoint(emptyPoint)))
                            {
                                if (ForkExists(remainingEmptyPoint, opponent)) // find fork after next step if next step is forced victory
                                {
                                    playersForks.First(x => x.Player == opponent).Forks[remainingEmptyPoint] = new List<Point>();
                                }
                            }
                        }
                    }
                    BadgesField[emptyPoint] = null;
                }
            }
            // код вище працює правильно. перевірено.

            if (playersForks.All(fork => fork.Forks.Count == 0))
                return null;

            // for each fork point I find block points
            foreach (var playerFork in playersForks)
            {
                foreach (var fork in playerFork.Forks)
                {
                    if (ForkExists(fork.Key, playerFork.Player) == false) // when fork point will create after forcing victory (after one step)
                    {
                        if (!fork.Value.Contains(fork.Key)) // fork.Key 100% blocks current fork
                            fork.Value.Add(fork.Key);
                        BadgesField[fork.Key] = playerFork.Player;
                        foreach (var emptyPoint in FindEmptyPointsInLines(FindLinesWithPoint(fork.Key)))
                        {
                            if (ForkExists(emptyPoint, playerFork.Player))
                            {
                                fork.Value.Add(emptyPoint);
                            }
                        }
                        BadgesField[fork.Key] = null;
                    }
                    else // fork point exists on next step
                    {
                        foreach (var emptyPoint in FindEmptyPointsInLines(FindLinesWithPoint(fork.Key)))
                        {
                            if (emptyPoint == fork.Key) // this empty point 100% blocks this fork
                            {
                                fork.Value.Add(emptyPoint);
                                continue;
                            }
                            BadgesField[emptyPoint] = playerFork.Player;
                            if (ForkExists(fork.Key, playerFork.Player) == false)
                            {
                                fork.Value.Add(emptyPoint);
                            }
                            BadgesField[emptyPoint] = null;
                        }
                    }
                }
            }
            return playersForks;
        }

        private Point BlockOpponentFork()
        {
            var playersForks = FindPlayersForks();
            if (playersForks == null)
                return null;
            /* нужно: если пересечение всех списков конкретного плеера содержит 0 элементов -
             * значит там более одного форка (не возможно перекрыть оба форка одним беджиком) и нужно применить алгоритм максимальной атаки (которого нет:D)
             */
            foreach (var playerFork in playersForks)
            {
                var all = playerFork.Forks.Values.Aggregate((current, next) => current.Intersect(next).ToList());
                if (all.Count == 0)
                    return null; // в любой непонятной ситуации возвращай нул
            }

            var countPlayersWithFork = playersForks.Count(playerFork => playerFork.Forks.Count > 0);

            if (countPlayersWithFork == 1)
            {
                var playerFork = playersForks.First(fork => fork.Forks.Count > 0);
                var blockPoints = new List<Point>();
                var forkList = playerFork.Forks.Select(x => x.Value).ToArray();
                foreach (var fork in playerFork.Forks)
                {
                    foreach (var point in fork.Value)
                    {
                        if (forkList.All(x => x.Contains(point)) && blockPoints.Contains(point) == false)
                            blockPoints.Add(point);
                    }
                }
                var bestPoint = TryForceVictoryAndFork(blockPoints);
                if (bestPoint != null) return bestPoint;
                bestPoint = TryCreateFork(blockPoints);
                if (bestPoint != null) return bestPoint;
                bestPoint = TryCreateLine(blockPoints);
                if (bestPoint != null) return bestPoint;
                bestPoint = MakeBestRandomStep(blockPoints);
                if (bestPoint != null) return bestPoint;
            }
            return null;
        }

        private bool ForkExists(Point point, Player player)
        {
            if (BadgesField[point] != null)
                throw new ArgumentException("Point " + point + " already has an owner");
            var matches = 0;
            foreach (var line in FindLinesWithPoint(point))
            {
                if (line.Count(dot => BadgesField[dot] == null) == 2 &&
                    line.All(dot => BadgesField[dot] == player || BadgesField[dot] == null))
                    matches++;
            }
            return matches >= 2;
        }

        private bool VictoryExists(Point point, Player player)
        {
            if (BadgesField[point] != null)
                throw new ArgumentException("Point " + point + " already has an owner");
            Point[] exceptPoint = {point};
            foreach (var line in FindLinesWithPoint(point))
            {
                if (line.Except(exceptPoint).All(dot => BadgesField[dot] == player))
                    return true;
            }
            return false;
        }

        private Point CreateFork()
        {
            return FindEmptyPoints().FirstOrDefault(emptyPoint => ForkExists(emptyPoint, PlayerFor));
        }

        private Point Block()
        {
            return (from opponent in Opponents
                    from emptyPoint in FindEmptyPoints()
                    where VictoryExists(emptyPoint, opponent)
                    select emptyPoint).FirstOrDefault();
        }

        private Point Win()
        {
            return FindEmptyPoints().FirstOrDefault(emptyPoint => VictoryExists(emptyPoint, PlayerFor));
        }

        private IEnumerable<List<Point>> FindLinesWithPoint(Point point)
        {
            return Info.GameGeometry.Lines.Where(line => line.Contains(point));
        }

        private IEnumerable<Point> FindEmptyPoints()
        {
            return Info.GameGeometry.Points.Where(point => BadgesField[point] == null);
        }

        private IEnumerable<Point> FindEmptyPointsInLines(IEnumerable<List<Point>> lines)
        {
            return from line in lines
                   from point in line
                   where BadgesField[point] == null
                   select point;
        }

        private void MapBadgesField(IEnumerable<BadgeModel> badges)
        {
            foreach (var point in Info.GameGeometry.Points)
            {
                BadgesField[point] = null;
            }
            foreach (var badge in badges)
            {
                BadgesField[badge.Coordinates] = badge.Owner;
            }
        }
    }

    internal class PlayerFork
    {
        public Player Player { get; set; }
        public Dictionary<Point, List<Point>> Forks { get; private set; }
        /*
         * Point -> fork point
         * List<Point> -> list of points which block fork
         */

        public PlayerFork()
        {
            Forks = new Dictionary<Point, List<Point>>();
        }
    }
}