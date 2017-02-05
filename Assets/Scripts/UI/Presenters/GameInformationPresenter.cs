using System;
using System.ComponentModel;
using Zenject;

namespace TicTacToe3D
{
    public class GameInformationPresenter : IMenuPresenter, IInitializable, IDisposable
    {
        private GameInformationView View { get; set; }
        private MenuManager MenuManager { get; set; }
        private GameInfo Info { get; set; }

        public GameInformationPresenter(MenuManager menuManager, GameInfo info)
        {
            MenuManager = menuManager;
            Info = info;

            MenuManager.SetMenu(this);
        }

        public void SetView(GameInformationView view)
        {
            View = view;
        }

        public void Initialize()
        {
            View.AdvancedSettingsButton.onClick.AddListener(OnAdvancedSettingsButtonClicked);

            Info.PropertyChanged += OnGameInfoPropertyChanged;
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
            }
        }

        public void Dispose()
        {
            View.AdvancedSettingsButton.onClick.RemoveAllListeners();

            Info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        public void Open()
        {
            View.IsOpen = true;
        }

        public void Close()
        {
            View.IsOpen = false;
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
            MenuManager.CloseMenu(Menus.GameInfoMenu);
            MenuManager.OpenMenu(Menus.AdvancedSettingsMenu);
        }
    }
}