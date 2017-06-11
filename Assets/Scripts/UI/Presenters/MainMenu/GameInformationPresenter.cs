using System;
using System.ComponentModel;
using System.Linq;
using Zenject;

namespace TicTacToe3D
{
    public class GameInformationPresenter : MenuPresenter<GameInformationView>, IInitializable, IDisposable
    {
        private MenuManager MenuManager { get; set; }
        private GameInfo Info { get; set; }

        public GameInformationPresenter(MenuManager menuManager, GameInfo info, AudioController audioController) : base(audioController)
        {
            MenuManager = menuManager;
            Info = info;

            MenuManager.SetMenu(this);
        }

        public void Initialize()
        {
            View.AdvancedSettingsButton.onClick.AddListener(OnAdvancedSettingsButtonClicked);

            Info.PropertyChanged += OnGameInfoPropertyChanged;
            Info.GameSettings.PropertyChanged += OnGameSettingsPropertyChanged;

            OnDimensionChanged();
            OnBadgesToWinChanged();
            OnStepSizeChanged();
            OnTimerTypeChanged();
            OnGameOverAfterFirstWinnerChanged();
            OnConfirmStepChanged();
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Dimension":
                    OnDimensionChanged();
                    break;
                case "BadgesToWin":
                    OnBadgesToWinChanged();
                    break;
                case "StepSize":
                    OnStepSizeChanged();
                    break;
                case "TimerTime":
                    OnTimerTimeChanged();
                    break;
            }
        }

        private void OnGameSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "GameOverAfterFirstWinner":
                    OnGameOverAfterFirstWinnerChanged();
                    break;
                case "ConfirmStep":
                    OnConfirmStepChanged();
                    break;
                case "TimerType":
                    OnTimerTypeChanged();
                    break;
            }
        }

        public void Dispose()
        {
            View.AdvancedSettingsButton.onClick.RemoveAllListeners();

            Info.PropertyChanged -= OnGameInfoPropertyChanged;
            Info.GameSettings.PropertyChanged -= OnGameSettingsPropertyChanged;
        }

        private void OnDimensionChanged()
        {
            View.DimensionAmountText.text = Info.Dimension.ToString();
        }

        private void OnBadgesToWinChanged()
        {
            View.BadgesToWinAmountText.text = Info.BadgesToWin.ToString();
        }

        private void OnStepSizeChanged()
        {
            View.StepSizeAmountText.text = Info.StepSize.ToString();
        }

        private void OnTimerTimeChanged()
        {
            var timerTypeName = Enum.GetName(typeof(TimerTypes), Info.GameSettings.TimerType);
            timerTypeName = string.Concat(timerTypeName.Select(x => char.IsUpper(x) ? " " + x : x.ToString()).ToArray()).TrimStart(' ');
            View.TimerTypeText.text = timerTypeName + " (" + Info.TimerTime + " sec)";
        }

        private void OnGameOverAfterFirstWinnerChanged()
        {
            View.GameOverAfterFirstWinnerText.text = Info.GameSettings.GameOverAfterFirstWinner ? "On" : "Off";
        }

        private void OnConfirmStepChanged()
        {
            View.StepConfirmationText.text = Info.GameSettings.ConfirmStep ? "On" : "Off";
        }

        private void OnTimerTypeChanged()
        {
            var timerTypeName = Enum.GetName(typeof(TimerTypes), Info.GameSettings.TimerType);
            timerTypeName = string.Concat(timerTypeName.Select(x => char.IsUpper(x) ? " " + x : x.ToString()).ToArray()).TrimStart(' ');
            if (Info.GameSettings.TimerType == TimerTypes.None || Info.GameSettings.TimerType == TimerTypes.DynamicTime)
            {
                View.TimerTypeText.text = timerTypeName;
            }
            else
            {
                View.TimerTypeText.text = timerTypeName + " (" + Info.TimerTime + " sec)";
            }
        }

        private void OnAdvancedSettingsButtonClicked()
        {
            MenuManager.CloseMenu(Menus.GameInfoMenu);
            MenuManager.OpenMenu(Menus.AdvancedSettingsMenu);
        }
    }
}