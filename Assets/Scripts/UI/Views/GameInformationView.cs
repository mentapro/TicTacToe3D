using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class GameInformationView : MenuView
    {
        [Inject]
        public void Construct(GameInformationPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }

        [SerializeField]
        private Button _advancedSettingsButton = null;

        public Button AdvancedSettingsButton
        {
            get { return _advancedSettingsButton; }
        }
    }
}