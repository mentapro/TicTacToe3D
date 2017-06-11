using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.SceneManagement;
using Zenject;

namespace TicTacToe3D
{
    public class PlayAgainWindowPresenter : MenuPresenter<PlayAgainWindowView>, IInitializable, IDisposable
    {
        private MenuManager MenuManager { get; set; }
        private GameInfo Info { get; set; }
        private ZenjectSceneLoader SceneLoader { get; set; }
        private AudioController AudioController { get; set; }

        public PlayAgainWindowPresenter(MenuManager menuManager,
            GameInfo info,
            ZenjectSceneLoader sceneLoader,
            AudioController audioController) : base(audioController)
        {
            MenuManager = menuManager;
            Info = info;
            SceneLoader = sceneLoader;
            AudioController = audioController;

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
            Info.Players.Sort(new PlayerScoreComparer());
            AudioController.Source.Stop();
            SceneLoader.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single, container =>
            {
                container.BindInstance(Info).WhenInjectedInto<GameBoardInstaller>();
            });
        }

        private void OnNoButtonClicked()
        {
            AudioController.Source.Stop();
            AudioController.SourceBackground.Stop();
            SceneManager.LoadScene("MainMenu");
        }
    }

    internal class PlayerScoreComparer : IComparer<Player>
    {
        public int Compare(Player x, Player y)
        {
            if (x.Score > y.Score) return -1;
            if (x.Score < y.Score) return 1;
            return 0;
        }
    }
}