using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class HighscoresMenuView : MenuView
    {
        [Inject]
        public void Construct(HighscoresMenuPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }

        [SerializeField]
        private Button _backButton = null;
        [SerializeField]
        private Transform _tableContent = null;

        public Button BackButton
        {
            get { return _backButton; }
        }
        
        public Transform TableContent
        {
            get { return _tableContent; }
        }
    }
}