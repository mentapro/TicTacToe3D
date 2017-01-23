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

        [SerializeField]
        private Text _dimensionAmountText = null;
        [SerializeField]
        private Text _badgesToWinAmountText = null;
        [SerializeField]
        private Text _stepSizeAmountText = null;

        public Button AdvancedSettingsButton
        {
            get { return _advancedSettingsButton; }
        }

        public Text DimensionAmountText
        {
            get { return _dimensionAmountText; }
        }

        public Text BadgesToWinAmountText
        {
            get { return _badgesToWinAmountText; }
        }

        public Text StepSizeAmountText
        {
            get { return _stepSizeAmountText; }
        }
    }
}