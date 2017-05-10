using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class NewGameMenuView : MenuView
    {
        [Inject]
        public void Construct(NewGameMenuPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }

        [SerializeField]
        private Button _startButton = null;
        [SerializeField]
        private Button _cancelButton = null;

        [SerializeField]
        private Transform _playersRows = null;

        public Button StartButton
        {
            get { return _startButton; }
        }

        public Button CancelButton
        {
            get { return _cancelButton; }
        }
        
        public Transform PlayersRows
        {
            get { return _playersRows; }
        }
    }
}