using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class AdvancedSettingsView : MenuView
    {
        [Inject]
        public void Construct(AdvancedSettingsPresenter menuPresenter)
        {
            menuPresenter.SetView(this);
        }

        [SerializeField]
        private Button _gameInformationButton = null;

        [SerializeField]
        private Slider _dimensionSlider = null;
        [SerializeField]
        private Slider _badgesToWinSlider = null;
        [SerializeField]
        private Slider _stepSizeSlider = null;

        [SerializeField]
        private Text _dimensionAmountText = null;
        [SerializeField]
        private Text _badgesToWinAmountText = null;
        [SerializeField]
        private Text _stepSizeAmountText = null;

        public Button GameInformationButton
        {
            get { return _gameInformationButton; }
        }

        public Slider DimensionSlider
        {
            get { return _dimensionSlider; }
        }

        public Slider BadgesToWinSlider
        {
            get { return _badgesToWinSlider; }
        }

        public Slider StepSizeSlider
        {
            get { return _stepSizeSlider; }
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