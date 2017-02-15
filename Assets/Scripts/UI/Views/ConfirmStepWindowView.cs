using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class ConfirmStepWindowView : MenuView
    {
        [Inject]
        public void Construct(ConfirmStepWindowPresenter presenter)
        {
            presenter.SetView(this);
        }

        [SerializeField]
        private Button _confirmStepButton = null;
        [SerializeField]
        private Button _undoStepButton = null;

        public Button ConfirmStepButton
        {
            get { return _confirmStepButton; }
        }

        public Button UndoStepButton
        {
            get { return _undoStepButton; }
        }
    }
}