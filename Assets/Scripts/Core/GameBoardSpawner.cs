using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class GameBoardSpawner : IInitializable
    {
        private const float TOLERANCE_Y = 0.0001f;
        private const float TOLERANCE_X_Z = 0.01f;
        private Transform _gameBoard { get; set; }

        private Settings _Settings { get; set; }
        private GameInfo Info { get; set; }
        private BoardFactory _BoardFactory { get; set; }

        public GameBoardSpawner(Settings settings, GameInfo info, BoardFactory boardFactory)
        {
            _Settings = settings;
            Info = info;
            _BoardFactory = boardFactory;
        }

        public void Initialize()
        {
            SpawnGameBoard(Info.Dimension);
        }

        private void SpawnGameBoard(int dimension)
        {
            var side = _Settings.DistanceBetweenSticks;
            var radius = Mathf.Sqrt(2.0f) * ((dimension - 1) * side) / 2.0f + 0.2f;
            _gameBoard = CreateGameBoard(radius);
            //CreateSticks(dimension, side);
        }

        private Transform CreateGameBoard(float radius)
        {
            var gameBoard = _BoardFactory.Create();
            gameBoard.transform.localScale = new Vector3(radius * 2, 0.1f, radius * 2);
            return gameBoard.transform;
        }

        //private Transform[,] CreateSticks(int dimension, float side)
        //{
        //    var dimStart = -dimension / 2.0f - 0.5f;
        //    var dim = Range(dimStart, dimension);
        //    var xCoordinates = dim.Select(x => x * side).ToList();

        //    var sticks = new Transform[dimension, dimension];
        //    for (var i = 0; i < dimension; i++)
        //        for (var j = 0; j < dimension; j++)
        //        {
        //            var stick = Instantiate(Resources.Load("Prefabs/InGame/Stick"), transform) as GameObject;
        //            stick.transform.localScale = new Vector3(stick.transform.localScale.x, DistanceBetweenBadges * dimension / 2, stick.transform.localScale.z);
        //            stick.transform.localPosition = new Vector3(xCoordinates[i], _gameBoard.localScale.y + stick.transform.localScale.y, xCoordinates[j]);

        //            var stickPart = Instantiate(Resources.Load("Prefabs/InGame/StickPartition"), stick.transform) as GameObject;
        //            stickPart.transform.localScale = new Vector3(1 + ToleranceXZ, 1f / dimension + ToleranceY, 1 + ToleranceXZ);
        //            stickPart.transform.localPosition = new Vector3(0, -(1f - stickPart.transform.localScale.y), 0);
        //            stickPart.GetComponent<BadgeSpawnPoint>().Coordinates = new Point(i, j, 0);
        //            for (var k = 1; k < dimension; k++)
        //            {
        //                stickPart = Instantiate(stickPart, stick.transform) as GameObject;
        //                stickPart.transform.localPosition = new Vector3(0, stickPart.transform.localPosition.y + 1f / dimension * 2, 0);
        //                stickPart.GetComponent<BadgeSpawnPoint>().Coordinates = new Point(i, j, k);
        //            }
        //            sticks[i, j] = stick.transform;
        //        }
        //    return sticks;
        //}

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
            public float DistanceBetweenSticks;
            public float DistanceBetweenBadges;
        }
        
        public class BoardFactory : Factory<GameObject>
        { }
    }
}