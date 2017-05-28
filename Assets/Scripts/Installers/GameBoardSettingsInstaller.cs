using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class GameBoardSettingsInstaller : ScriptableObjectInstaller<GameBoardSettingsInstaller>
    {
        public GameBoardSpawner.Settings GameBoardSpawnerSettings;
        public CameraHandler.Settings CameraSettings;
        public BadgeSpawnPoint.Settings BadgeSpawnPointSettings;
        public BadgeFacade.Settings BadgeSettings;
        public GameSettings GameSettings;
        public GameBoardInstaller.Settings GameBoardInstallerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(GameBoardSpawnerSettings);
            Container.BindInstance(CameraSettings);
            Container.BindInstance(BadgeSpawnPointSettings);
            Container.BindInstance(BadgeSettings);
            Container.BindInstance(GameSettings);
            Container.BindInstance(GameBoardInstallerSettings);
        }
    }

    [Serializable]
    public class GameSettings : INotifyPropertyChanged
    {
        [SerializeField]
        private bool _gameOverAfterFirstWinner;
        [SerializeField]
        private bool _confirmStep;
        [SerializeField]
        private TimerTypes _timerType;

        public bool GameOverAfterFirstWinner
        {
            get { return _gameOverAfterFirstWinner; }
            set
            {
                if (value == _gameOverAfterFirstWinner) return;
                _gameOverAfterFirstWinner = value;
                OnPropertyChanged("GameOverAfterFirstWinner");
            }
        }

        [JsonIgnore]
        public bool ConfirmStep
        {
            get { return _confirmStep; }
            set
            {
                if (value == _confirmStep) return;
                _confirmStep = value;
                OnPropertyChanged("ConfirmStep");
            }
        }

        public TimerTypes TimerType
        {
            get { return _timerType; }
            set
            {
                if (value == _timerType) return;
                _timerType = value;
                OnPropertyChanged("TimerType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}