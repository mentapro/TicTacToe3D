using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public enum TimerTypes
    {
        None,
        FixedTimePerStep,
        FixedTimePerRound,
        DynamicTime
    }

    public class TimerHandler : ITickable, IDisposable
    {
        private bool _tick;
        private Player _activePlayer;
        
        private GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }

        public TimerHandler(GameInfo info, GameEvents gameEvents)
        {
            Info = info;
            GameEvents = gameEvents;

            Info.PropertyChanged += OnGameInfoPropertyChanged;
        }

        public void Dispose()
        {
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        public void Tick()
        {
            if (_tick == false)
            {
                return;
            }

            switch (Info.GameSettings.TimerType)
            {
                case TimerTypes.None:
                    break;
                case TimerTypes.FixedTimePerStep:
                    UpdateFixedTimePerStep();
                    break;
            }
        }

        private void UpdateFixedTimePerStep()
        {
            _activePlayer.TimeLeft -= Time.deltaTime;
            if (_activePlayer.TimeLeft <= 0)
            {
                GameEvents.TimePassed();
            }
        }

        private void OnGlobalStepChanged()
        {
            if (Info.GameSettings.TimerType == TimerTypes.FixedTimePerStep)
            {
                foreach (var player in Info.Players)
                {
                    player.TimeLeft = Info.TimerTime;
                }
            }
        }

        private void OnActivePlayerChanged(Player activePlayer)
        {
            _activePlayer = activePlayer;
        }

        private void OnGameStateChanged(GameStates state)
        {
            _tick = state == GameStates.Started;
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                OnGameStateChanged(Info.GameState);
            }
            if (e.PropertyName == "ActivePlayer")
            {
                OnActivePlayerChanged(Info.ActivePlayer);
            }
            if (e.PropertyName == "GlobalStep")
            {
                OnGlobalStepChanged();
            }
        }
    }
}