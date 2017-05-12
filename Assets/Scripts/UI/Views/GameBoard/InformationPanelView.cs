using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class InformationPanelView : MenuView
    {
        [Inject]
        public void Construct(InformationPanelPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }

        [SerializeField]
        private Text _timerTimeText = null;

        public Text TimerTimeText
        {
            get { return _timerTimeText; }
        }
    }
}