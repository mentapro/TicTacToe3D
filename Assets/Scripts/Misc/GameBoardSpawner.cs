using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class GameBoardSpawner : IInitializable
    {
        private Settings _Settings { get; set; }
        private GameInfo Info { get; set; }
        private BoardFactory _BoardFactory { get; set; }
        private StickFactory _StickFactory { get; set; }
        private StickPartitionFactory _StickPartitionFactory { get; set; }

        public GameBoardSpawner(Settings settings,
            GameInfo info,
            BoardFactory boardFactory,
            StickFactory stickFactory,
            StickPartitionFactory stickPartitionFactory)
        {
            _Settings = settings;
            Info = info;
            _BoardFactory = boardFactory;
            _StickFactory = stickFactory;
            _StickPartitionFactory = stickPartitionFactory;
        }

        public void Initialize()
        {
            SpawnGameBoard(Info.Dimension);
        }

        private void SpawnGameBoard(int dimension)
        {
            var side = _Settings.DistanceBetweenSticks;
            var radius = Mathf.Sqrt(2.0f) * ((dimension - 1) * side) / 2.0f + _Settings.BoardRadiusOffset;
            CreateBoard(radius);
            CreateSticks(dimension, side);
        }
        
        private void CreateBoard(float radius)
        {
            var gameBoard = _BoardFactory.Create();
            gameBoard.transform.localScale = new Vector3(radius * 2, _Settings.BoardThickness, radius * 2);
        }

        private void CreateSticks(int dimension, float side)
        {
            var dimStart = -dimension / 2.0f - 0.5f;
            var dim = Range(dimStart, dimension);
            var xCoordinates = dim.Select(x => x * side).ToList();

            for (var i = 0; i < dimension; i++)
                for (var j = 0; j < dimension; j++)
                {
                    var stick = _StickFactory.Create();
                    stick.transform.localScale = new Vector3(stick.transform.localScale.x, _Settings.DistanceBetweenBadges * dimension / 2, stick.transform.localScale.z);
                    stick.transform.localPosition = new Vector3(xCoordinates[i], stick.transform.localScale.y + _Settings.BoardThickness, xCoordinates[j]);

                    var inc = 1;
                    for (var k = 0; k < dimension; k++)
                    {
                        var stickPart = _StickPartitionFactory.Create();
                        stickPart.transform.SetParent(stick.transform, false);
                        stickPart.transform.localScale = new Vector3(stickPart.transform.localScale.x, 1f / dimension + 0.0001f, stickPart.transform.localScale.z);
                        stickPart.transform.localPosition = new Vector3(0, -1 + stickPart.transform.localScale.y * inc, 0);
                        stickPart.Coordinates = new Point(i, j, k);
                        inc += 2;
                    }
                }
        }

        private static IEnumerable<float> Range(float start, int count)
        {
            var v = start;
            for (var i = 0; i < count; i++)
            {
                v += 1.0f;
                yield return v;
            }
        }

        [Serializable]
        public class Settings
        {
            public float BoardThickness;
            public float BoardRadiusOffset;
            public float DistanceBetweenSticks;
            public float DistanceBetweenBadges;
        }
        
        public class BoardFactory : GameObjectFactory
        { }
        
        public class StickFactory : GameObjectFactory
        { }

        public class StickPartitionFactory : Factory<BadgeSpawnPoint>
        { }
    }
}