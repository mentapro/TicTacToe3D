using System;
using System.IO;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class HighscoresMenuPresenter : MenuPresenter<HighscoresMenuView>, IInitializable, IDisposable
    {
        private readonly MenuManager _menuManager;
        private readonly IFetchService<Stats> _statsFetchService;
        private readonly HighscoreItemModel.Registry _highscoresRegistry;
        private readonly HighscoreItemFacade.Factory _highscoreItemFactory;

        public HighscoresMenuPresenter(MenuManager menuManager,
            IFetchService<Stats> statsFetchService,
            HighscoreItemModel.Registry highscoresRegistry,
            HighscoreItemFacade.Factory highscoreItemFactory, AudioController audioController) : base(audioController)
        {
            _menuManager = menuManager;
            _statsFetchService = statsFetchService;
            _highscoresRegistry = highscoresRegistry;
            _highscoreItemFactory = highscoreItemFactory;

            _menuManager.SetMenu(this);
        }

        public void Initialize()
        {
            View.BackButton.onClick.AddListener(OnBackButtonClicked);
        }

        public void Dispose()
        {
            View.BackButton.onClick.RemoveAllListeners();
        }

        public override void Open()
        {
            UpdateHighscores();
            base.Open();
        }

        private void UpdateHighscores()
        {
            _highscoresRegistry.Clear();
            if (File.Exists(Application.dataPath + "/Stats/Stats.json") == false)
            {
                return;
            }
            var statsInstance = _statsFetchService.Load("Stats");
            statsInstance.StatsItems.Sort();

            foreach (var stat in statsInstance.StatsItems)
            {
                var highscoreItem = _highscoreItemFactory.Create();
                highscoreItem.transform.SetParent(View.TableContent, false);
                highscoreItem.PlayerNameText.text = stat.PlayerName;
                highscoreItem.TotalScoreText.text = stat.TotalScore.ToString();
                highscoreItem.WonRoundsText.text = stat.WonRounds.ToString();
            }
        }

        private void OnBackButtonClicked()
        {
            _menuManager.OpenMenu(Menus.MainMenu);
        }
    }
}