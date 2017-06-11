using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class InformationPanelPresenter : MenuPresenter<InformationPanelView>, IInitializable, IDisposable
    {
        private GameInfo Info { get; set; }

        public InformationPanelPresenter(GameInfo info, AudioController audioController) : base(audioController)
        {
            Info = info;
        }

        public void Initialize()
        {
            if (Info.GameSettings.TimerType != TimerTypes.None)
            {
                Info.PropertyChanged += OnGameInfoPropertyChanged;
                Info.Players.ForEach(player => player.PropertyChanged += OnPlayerPropertyChanged);
            }
            else
            {
                View.TimerTimeText.gameObject.SetActive(false);
            }
        }

        public void Dispose()
        {
            if (Info.GameSettings.TimerType != TimerTypes.None)
            {
                Info.PropertyChanged -= OnGameInfoPropertyChanged;
                Info.Players.ForEach(player => player.PropertyChanged -= OnPlayerPropertyChanged);
            }
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                OnGameStateChanged(Info.GameState);
            }
        }

        private void OnPlayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TimeLeft")
            {
                OnPlayerTimeLeftChanged();
            }
        }

        private void OnPlayerTimeLeftChanged()
        {
            if (Info.ActivePlayer.TimeLeft >= 0f)
            {
                View.TimerTimeText.text = string.Format("{0:00} : {1:00}",
                Mathf.FloorToInt(Info.ActivePlayer.TimeLeft / 60f), Mathf.FloorToInt(Info.ActivePlayer.TimeLeft % 60f));
            }
        }

        private void OnGameStateChanged(GameStates state)
        {
            if (state == GameStates.GameEnded)
            {
                View.TimerTimeText.gameObject.SetActive(false);
            }
        }
    }
}