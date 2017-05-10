using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class PlayersTableWindowView : MenuView
    {
        [Inject]
        public void Construct(PlayersTableWindowPresenter presenter)
        {
            presenter.SetView(this);
        }

        [SerializeField]
        private Transform _playersRows = null;

        public Transform PlayersRows
        {
            get { return _playersRows; }
        }
    }
}