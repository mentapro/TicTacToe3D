using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class PlayAgainWindowView : MenuView
    {
        [Inject]
        public void Construct(PlayAgainWindowPresenter presenter)
        {
            presenter.SetView(this);
        }

        [SerializeField]
        private Button _yesButton = null;
        [SerializeField]
        private Button _noButton = null;

        public Button YesButton
        {
            get { return _yesButton; }
        }
        
        public Button NoButton
        {
            get { return _noButton; }
        }
    }
}