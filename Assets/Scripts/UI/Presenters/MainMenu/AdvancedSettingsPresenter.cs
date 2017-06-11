using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Zenject;

namespace TicTacToe3D
{
    public class AdvancedSettingsPresenter : MenuPresenter<AdvancedSettingsView>, IInitializable, IDisposable
    {
        private MenuManager MenuManager { get; set; }
        private GameInfo Info { get; set; }
        private Settings _Settings { get; set; }

        public AdvancedSettingsPresenter(MenuManager menuManager, GameInfo info, Settings settings, AudioController audioController) : base(audioController)
        {
            MenuManager = menuManager;
            Info = info;
            _Settings = settings;

            MenuManager.SetMenu(this);
        }

        public void Initialize()
        {
            View.DimensionSlider.maxValue = _Settings.DimensionMax;
            View.BadgesToWinSlider.maxValue = View.DimensionSlider.value;
            View.StepSizeSlider.maxValue = _Settings.StepSizeMax;

            View.GameInformationButton.onClick.AddListener(OnGameInformationButtonClicked);

            View.DimensionSlider.onValueChanged.AddListener(OnDimensionSliderChanged);
            View.BadgesToWinSlider.onValueChanged.AddListener(OnBadgesToWinSliderChanged);
            View.StepSizeSlider.onValueChanged.AddListener(OnStepSizeSliderChanged);
            View.TimerTypeDropdown.onValueChanged.AddListener(OnTimerTypeDropdownChanged);
            View.TimerTimeInputField.onEndEdit.AddListener(OnTimerTimeInputFieldEndEdit);
            View.TimerTimeInputField.onValidateInput += OnTimerTimeInputFieldValidateInput;
            View.FirstWinnerToggle.onValueChanged.AddListener(OnFirstWinnerToggleChanged);
            View.StepConfirmationToggle.onValueChanged.AddListener(OnStepConfirmationToggleChanged);

            Info.Dimension = (int) View.DimensionSlider.value;
            Info.BadgesToWin = (int) View.BadgesToWinSlider.value;
            Info.StepSize = (int) View.StepSizeSlider.value;

            View.DimensionAmountText.text = Info.Dimension.ToString();
            View.BadgesToWinAmountText.text = Info.BadgesToWin.ToString();
            View.StepSizeAmountText.text = Info.StepSize.ToString();

            var timerTypesList = Enum.GetNames(typeof(TimerTypes)).ToList();
            for (var i = 0; i < timerTypesList.Count; i++)
            {
                timerTypesList[i] = string.Concat(timerTypesList[i].Select(x => char.IsUpper(x) ? " " + x : x.ToString()).ToArray()).TrimStart(' ');
            }
            View.TimerTypeDropdown.AddOptions(timerTypesList);

            View.TimerTypeDropdown.value = (int) Info.GameSettings.TimerType;
            View.TimerTimeInputField.gameObject.SetActive(Info.GameSettings.TimerType == TimerTypes.FixedTimePerStep ||
                                                          Info.GameSettings.TimerType == TimerTypes.FixedTimePerRound);
            View.TimerTimeInputField.text = Info.TimerTime.ToString(CultureInfo.InvariantCulture);
            View.FirstWinnerToggle.isOn = Info.GameSettings.GameOverAfterFirstWinner;
            View.StepConfirmationToggle.isOn = Info.GameSettings.ConfirmStep;
        }

        public void Dispose()
        {
            View.GameInformationButton.onClick.RemoveAllListeners();

            View.DimensionSlider.onValueChanged.RemoveAllListeners();
            View.BadgesToWinSlider.onValueChanged.RemoveAllListeners();
            View.StepSizeSlider.onValueChanged.RemoveAllListeners();
            View.TimerTypeDropdown.onValueChanged.RemoveAllListeners();
            View.TimerTimeInputField.onEndEdit.RemoveAllListeners();
            View.TimerTimeInputField.onValidateInput -= OnTimerTimeInputFieldValidateInput;
            View.FirstWinnerToggle.onValueChanged.RemoveAllListeners();
            View.StepConfirmationToggle.onValueChanged.RemoveAllListeners();
        }

        private void OnDimensionSliderChanged(float value)
        {
            View.BadgesToWinSlider.maxValue = value;
            Info.Dimension = (int) value;
            View.DimensionAmountText.text = Info.Dimension.ToString();
            View.BadgesToWinAmountText.text = Info.BadgesToWin.ToString();
        }

        private void OnBadgesToWinSliderChanged(float value)
        {
            Info.BadgesToWin = (int) value;
            View.BadgesToWinAmountText.text = Info.BadgesToWin.ToString();
        }

        private void OnStepSizeSliderChanged(float value)
        {
            Info.StepSize = (int) value;
            View.StepSizeAmountText.text = Info.StepSize.ToString();
        }
        
        private void OnTimerTypeDropdownChanged(int value)
        {
            Info.GameSettings.TimerType = (TimerTypes) value;
            if (Info.GameSettings.TimerType == TimerTypes.FixedTimePerStep ||
                Info.GameSettings.TimerType == TimerTypes.FixedTimePerRound)
            {
                View.TimerTimeInputField.gameObject.SetActive(true);
            }
            else
            {
                View.TimerTimeInputField.gameObject.SetActive(false);
            }
        }

        private void OnTimerTimeInputFieldEndEdit(string text)
        {
            View.TimerTimeInputField.text = text.TrimStart('0');
            if (text == string.Empty || Regex.IsMatch(text, "[1-9][0-9]*") == false)
            {
                View.TimerTimeInputField.text = Info.TimerTime.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                Info.TimerTime = int.Parse(View.TimerTimeInputField.text);
            }
        }

        private char OnTimerTimeInputFieldValidateInput(string text, int charIndex, char addedChar)
        {
            if (Regex.IsMatch(addedChar.ToString(), "[0-9]") == false)
            {
                addedChar = '\0';
            }
            return addedChar;
        }

        private void OnFirstWinnerToggleChanged(bool value)
        {
            Info.GameSettings.GameOverAfterFirstWinner = value;
        }

        private void OnStepConfirmationToggleChanged(bool value)
        {
            Info.GameSettings.ConfirmStep = value;
        }

        private void OnGameInformationButtonClicked()
        {
            MenuManager.CloseMenu(Menus.AdvancedSettingsMenu);
            MenuManager.OpenMenu(Menus.GameInfoMenu);
        }

        [Serializable]
        public class Settings
        {
            public int DimensionMax;
            public int StepSizeMax;
        }
    }
}