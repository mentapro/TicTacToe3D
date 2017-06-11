using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class StatsItem : IComparable<StatsItem>
    {
        public string PlayerName { get; set; }
        public int TotalScore { get; set; }
        public int WonRounds { get; set; }
        public int CompareTo(StatsItem other)
        {
            if (this.TotalScore > other.TotalScore)
                return -1;
            if (this.TotalScore < other.TotalScore)
                return 1;
            return 0;
        }
    }
    
    public class Stats : IInitializable, IDisposable
    {
        private readonly GameInfo _info;
        private readonly IFetchService<Stats> _statsFetchService;

        public List<StatsItem> StatsItems { get; set; }

        public Stats(GameInfo info, IFetchService<Stats> statsFetchService)
        {
            _info = info;
            _statsFetchService = statsFetchService;
        }

        public void Initialize()
        {
            var dir = new DirectoryInfo(Application.dataPath + "/Stats/");
            if (dir.Exists && File.Exists(Application.dataPath + "/Stats/Stats.json"))
            {
                var statsInstance = _statsFetchService.Load("Stats");
                StatsItems = statsInstance.StatsItems;
            }
            else
            {
                StatsItems = new List<StatsItem>();
            }

            _info.PropertyChanged += OnGameInfoPropertyChanged;
        }

        public void Dispose()
        {
            _info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "GameState":
                    OnGameStateChanged(_info.GameState);
                    break;
            }
        }

        private void OnGameStateChanged(GameStates state)
        {
            if (state == GameStates.GameEnded)
            {
                foreach (var player in _info.Players)
                {
                    var playerStats = StatsItems.FirstOrDefault(x => x.PlayerName == player.Name);
                    if (playerStats == null)
                    {
                        StatsItems.Add(playerStats = new StatsItem {PlayerName = player.Name});
                    }
                    playerStats.WonRounds += player.WonRounds;
                    playerStats.TotalScore += player.Score;
                }
                _statsFetchService.Save(this, "Stats");
            }
        }
    }
}