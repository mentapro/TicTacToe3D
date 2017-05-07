using System;
using System.ComponentModel;
using Zenject;

namespace TicTacToe3D
{
    public class PlayAgainWindowPresenter : MenuPresenter<PlayAgainWindowView>, IInitializable, IDisposable
    {
        private MenuManager MenuManager { get; set; }
        private GameInfo Info { get; set; }

        public PlayAgainWindowPresenter(MenuManager menuManager,
            GameEvents gameEvents,
            GameInfo info)
        {
            MenuManager = menuManager;
            Info = info;

            menuManager.SetMenu(this);
        }

        public void Initialize()
        {
            View.YesButton.onClick.AddListener(OnYesButtonClicked);
            View.NoButton.onClick.AddListener(OnNoButtonClicked);

            Info.PropertyChanged += OnGameInfoPropertyChanged;
        }

        public void Dispose()
        {
            View.YesButton.onClick.RemoveAllListeners();
            View.NoButton.onClick.RemoveAllListeners();

            Info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState")
            {
                OnGameStateChanged(Info.GameState);
            }
        }

        private void OnGameStateChanged(GameStates state)
        {
            if (state == GameStates.GameEnded)
            {
                MenuManager.OpenMenu(Menus.PlayAgainWindow);
            }
        }

        private void OnYesButtonClicked()
        {

        }

        private void OnNoButtonClicked()
        {

        }
    }
}