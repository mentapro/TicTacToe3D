using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class PauseWindowView : MenuView
    {
        [Inject]
        public void Construct(PauseWindowPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }

        [SerializeField]
        private Button _resumeButton = null;
        [SerializeField]
        private Button _saveGameButton = null;
        [SerializeField]
        private Button _loadGameButton = null;
        [SerializeField]
        private Button _settingsButton = null;
        [SerializeField]
        private Button _exitToMenuButton = null;
        [SerializeField]
        private Button _exitGameButton = null;

        public Button ResumeButton
        {
            get { return _resumeButton; }
        }

        public Button SaveGameButton
        {
            get { return _saveGameButton; }
        }

        public Button LoadGameButton
        {
            get { return _loadGameButton; }
        }

        public Button SettingsButton
        {
            get { return _settingsButton; }
        }

        public Button ExitToMenuButton
        {
            get { return _exitToMenuButton; }
        }

        public Button ExitGameButton
        {
            get { return _exitGameButton; }
        }
    }
}