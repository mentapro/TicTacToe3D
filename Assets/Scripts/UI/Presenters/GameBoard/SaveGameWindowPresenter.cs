using System;
using Zenject;

namespace TicTacToe3D
{
    public class SaveGameWindowPresenter : MenuPresenter<SaveGameWindowView>, IInitializable, IDisposable
    {
        private GameInfo Info { get; set; }
        private MenuManager MenuManager { get; set; }

        public SaveGameWindowPresenter(GameInfo info,
            MenuManager menuManager)
        {
            Info = info;
            MenuManager = menuManager;

            menuManager.SetMenu(this);
        }

        public void Initialize()
        {
            View.BackButton.onClick.AddListener(OnBackButtonClicked);
        }

        public void Dispose()
        {
            View.BackButton.onClick.RemoveAllListeners();
        }

        private void OnBackButtonClicked()
        {
            MenuManager.OpenMenu(Menus.PauseWindow);
        }
    }
}