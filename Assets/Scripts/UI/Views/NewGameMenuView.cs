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
        private Button _cancelButton = null;

        public Button CancelButton
        {
            get { return _cancelButton; }
        }
    }
}