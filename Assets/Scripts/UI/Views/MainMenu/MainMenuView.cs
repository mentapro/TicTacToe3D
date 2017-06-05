using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class MainMenuView : MenuView
    {
        [Inject]
        public void Construct(MainMenuPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }

        [SerializeField]
        private Button _newGameButton = null;
        [SerializeField]
        private Button _loadGameButton = null;
        [SerializeField]
        private Button _multiplayerButton = null;
        [SerializeField]
        private Button _highscoresButton = null;
        [SerializeField]
        private Button _settingsButton = null;
        [SerializeField]
        private Button _exitButton = null;

        public Button NewGameButton
        {
            get { return _newGameButton; }
        }
        
        public Button LoadGameButton
        {
            get { return _loadGameButton; }
        }

        public Button MultiplayerButton
        {
            get { return _multiplayerButton; }
        }

        public Button HighscoresButton
        {
            get { return _highscoresButton; }
        }

        public Button SettingsButton
        {
            get { return _settingsButton; }
        }

        public Button ExitButton
        {
            get { return _exitButton; }
        }
    }
}