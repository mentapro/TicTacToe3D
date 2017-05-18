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
    }
}