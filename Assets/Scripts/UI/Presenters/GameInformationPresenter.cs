using System;
using System.ComponentModel;
using Zenject;

namespace TicTacToe3D
{
    public class GameInformationPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private GameInformationView View { get; set; }
        private NewGameMenuPresenter NewGameMenu { get; set; }
        private GameInfo Info { get; set; }

        public GameInformationPresenter(NewGameMenuPresenter newGameMenu, GameInfo info)
        {
            NewGameMenu = newGameMenu;
            Info = info;
        }

        public void SetView(GameInformationView view)
        {
            View = view;
        }

        public void Initialize()
        {
            View.AdvancedSettingsButton.onClick.AddListener(OnAdvancedSettingsButtonClicked);

            Info.PropertyChanged += GameInfoOnPropertyChanged;
        }

        private void GameInfoOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
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
                default:
                    break;
            }
        }

        public void Dispose()
        {
            View.AdvancedSettingsButton.onClick.RemoveAllListeners();

            Info.PropertyChanged -= GameInfoOnPropertyChanged;
        }

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
        }

        public bool IsOpen()
        {
            return View.IsOpen;
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

        private void OnAdvancedSettingsButtonClicked()
        {
            NewGameMenu.SwitchPanel();
        }
    }
}