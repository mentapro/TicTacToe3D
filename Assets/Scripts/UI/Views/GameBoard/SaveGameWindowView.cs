using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class SaveGameWindowView : MenuView
    {
        [Inject]
        public void Construct(SaveGameWindowPresenter presenter)
        {
            presenter.SetView(this);
        }

        [SerializeField]
        private ToggleGroup _savesToggleGroup = null;
        [SerializeField]
        private Text _saveInformationText = null;
        [SerializeField]
        private InputField _saveNameInputField = null;
        [SerializeField]
        private Button _backButton = null;
        [SerializeField]
        private Button _saveButton = null;

        public Button BackButton
        {
            get { return _backButton; }
        }

        public Button SaveButton
        {
            get { return _saveButton; }
        }

        public InputField SaveNameInputField
        {
            get { return _saveNameInputField; }
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