using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class LoadGameWindowView : MenuView
    {
        [Inject]
        public void Construct(LoadGameWindowPresenter presenter)
        {
            presenter.SetView(this);
        }

        [SerializeField]
        private ToggleGroup _savesToggleGroup = null;
        [SerializeField]
        private Text _saveInformationText = null;
        [SerializeField]
        private Button _backButton = null;
        [SerializeField]
        private Button _loadButton = null;

        public Button BackButton
        {
            get { return _backButton; }
        }

        public Button LoadButton
        {
            get { return _loadButton; }
        }

        public ToggleGroup SavesToggleGroup
        {
            get { return _savesToggleGroup; }
        }

        public Text SaveInformationText
        {
            get { return _saveInformationText; }
        }
    }
}