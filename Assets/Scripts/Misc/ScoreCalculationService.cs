using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe3D
{
    public class ScoreCalculationService
    {
        private GameInfo Info { get; set; }
        private BadgeModel.Registry BadgeRegistry { get; set; }
        private Dictionary<Point, Player> BadgesField { get; set; }

        public ScoreCalculationService(GameInfo info, BadgeModel.Registry badgeRegistry)
        {
            Info = info;
            BadgeRegistry = badgeRegistry;
        }

        public Dictionary<Player, int> CalculateScores()
        {
            BadgesField = new Dictionary<Point, Player>();
            MapBadgesField(BadgeRegistry.Badges);

            var playersScores = new Dictionary<Player, int>();

            foreach (var player in Info.Players)
            {
                var scores = 0;
                var opponents = Info.Players.Except(new[] { player }).ToList();
                foreach (var line in Info.GameGeometry.Lines)
                {
                    if (line.All(point => BadgesField[point] == null))
                        continue;
                    if (line.Any(point => opponents.Contains(BadgesField[point])))
                    {
                        scores += line.Count(point => BadgesField[point] == player);
                        continue;
                    }
                    if (line.Count(point => BadgesField[point] == player) == 1)
                    {
                        scores += 1;
                        continue;
                    }
                    scores += line.Count(point => BadgesField[point] == player) * 2;
                }
                scores += CanForceVictoryAndFork(player) ? 35 : 0;
                scores += CountForks(player) * 25;
                scores += CountVictories(player) * 200;
                playersScores[player] = scores;
            }
            return playersScores;
        }

        private bool CanForceVictoryAndFork(Player player)
        {
            var opponents = Info.Players.Except(new[] { player }).ToList();
            foreach (var line in Info.GameGeometry.Lines)
            {
                if (line.All(point => BadgesField[point] == null))
                    continue;
                if (line.Any(point => opponents.Contains(BadgesField[point])))
                    continue;
                if (line.Count(point => BadgesField[point] == player) < Info.BadgesToWin - 2)
                    continue;
                foreach (var forcingVictoryPoint in line.Where(dot => BadgesField[dot] == null))
                {
                    BadgesField[forcingVictoryPoint] = player;
                    foreach (var forkPoint in FindEmptyPointsInLines(FindLinesWithPoint(forcingVictoryPoint)))
                    {
                        if (ForkExists(forkPoint, player))
                            return true;
                    }
                    BadgesField[forcingVictoryPoint] = null;
                }
            }
            return false;
        }

        private bool ForkExists(Point point, Player player)
        {
            if (BadgesField[point] != null)
                throw new ArgumentException("Point " + point + " already has an owner");
            var matches = FindLinesWithPoint(point).Count(line => line.Count(dot => BadgesField[dot] == null) == 2 && line.All(dot => BadgesField[dot] == player || BadgesField[dot] == null));
            return matches >= 2;
        }

        private int CountForks(Player player)
        {
            var opponents = Info.Players.Except(new[] { player }).ToList();
            var lines = (from line in Info.GameGeometry.Lines
                where line.Any(point => BadgesField[point] != null)
                where !line.Any(point => opponents.Contains(BadgesField[point]))
                where line.Count(point => BadgesField[point] == player) >= Info.BadgesToWin - 2
                select line).ToList();
            return lines.SelectMany(line => line.Where(x => BadgesField[x] == null)).Distinct().Count(emptyPoint => ForkExists(emptyPoint, player));
        }

        private int CountVictories(Player player)
        {
            var opponents = Info.Players.Except(new[] { player }).ToList();
            var count = 0;
            foreach (var line in Info.GameGeometry.Lines)
            {
                if (line.All(point => BadgesField[point] == null))
                    continue;
                if (line.Any(point => opponents.Contains(BadgesField[point])))
                    continue;
                if (line.Count(point => BadgesField[point] == player) < Info.BadgesToWin - 1)
                    continue;
                count++;
            }
            return count;
        }

        private IEnumerable<List<Point>> FindLinesWithPoint(Point point)
        {
            return Info.GameGeometry.Lines.Where(line => line.Contains(point));
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
}