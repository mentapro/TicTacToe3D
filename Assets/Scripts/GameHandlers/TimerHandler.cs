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

    public class TimerHandler : IInitializable, ITickable, IDisposable
    {
        private bool _tick;
        
        private GameInfo Info { get; set; }
        private GameEvents GameEvents { get; set; }

        public TimerHandler(GameInfo info, GameEvents gameEvents)
        {
            Info = info;
            GameEvents = gameEvents;

            Info.PropertyChanged += OnGameInfoPropertyChanged;
        }

        public void Initialize()
        {
            if (Info.GameSettings.TimerType == TimerTypes.DynamicTime)
            {
                throw new NotImplementedException("Dynamic timer is not implemented yet.");
            }
            Info.Players.ForEach(player =>
            {
                if (Info.GameSettings.TimerType == TimerTypes.FixedTimePerRound ||
                    Info.GameSettings.TimerType == TimerTypes.FixedTimePerStep)
                {
                    player.TimeLeft = Info.TimerTime;
                }
            });
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
                    UpdateFixedTimeTimer();
                    break;
                case TimerTypes.FixedTimePerRound:
                    UpdateFixedTimeTimer();
                    break;
                case TimerTypes.DynamicTime:
                    throw new NotImplementedException("Dynamic timer is not implemented yet.");
            }
        }

        private void UpdateFixedTimeTimer()
        {
            Info.ActivePlayer.TimeLeft -= Time.deltaTime;
            if (Info.ActivePlayer.TimeLeft <= 0)
            {
                GameEvents.TimePassed();
            }
        }

        private void OnGlobalStepChanged()
        {
            if (Info.GameSettings.TimerType == TimerTypes.FixedTimePerStep)
            {
                Info.Players.ForEach(player => player.TimeLeft = Info.TimerTime);
            }
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
            if (e.PropertyName == "GlobalStep")
            {
                OnGlobalStepChanged();
            }
        }
    }
}