using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D
{
    public partial class PlayerRowMenuModel
    {
        private int _currentColorIndex = -1;
        private PlayerRowMenuFacade Facade { get; set; }
        private Registry _Registry { get; set; }
        private Settings _Settings { get; set; }

        public PlayerRowMenuModel(Registry registry, Settings settings)
        {
            _Registry = registry;
            _Settings = settings;

            registry.AddRow(this);
        }

        public void SetFacade(PlayerRowMenuFacade facade)
        {
            Facade = facade;
            Initialize();
        }

        public void Dispose()
        {
            Facade.PlayerNameInputField.onEndEdit.RemoveAllListeners();
            Facade.PlayerColorDropdown.onValueChanged.RemoveAllListeners();
            Facade.PlayerTypeDropdown.onValueChanged.RemoveAllListeners();
        }

        private void Initialize()
        {
            _Settings.RepairColors();
            InitializePlayerTypes();
            InitializePlayerColors();

            Facade.PlayerNameInputField.onEndEdit.AddListener(OnPlayerNameFieldEndEdit);
            Facade.PlayerColorDropdown.onValueChanged.AddListener(OnPlayerColorDropdownChanged);
            Facade.PlayerTypeDropdown.onValueChanged.AddListener(OnPlayerTypeDropdownChanged);

            Facade.PlayerNameInputField.gameObject.SetActive(false);
            Facade.PlayerColorDropdown.gameObject.SetActive(false);
        }

        private void InitializePlayerTypes()
        {
            var playerTypes = new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData("None"),
                new Dropdown.OptionData("Human"),
                new Dropdown.OptionData("Computer")
            };
            Facade.PlayerTypeDropdown.AddOptions(playerTypes);
        }

        private void InitializePlayerColors()
        {
            foreach (var color in _Settings.PlayerColors)
            {
                var texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, color);
                texture.Apply();
                var item = new Dropdown.OptionData(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
                Facade.PlayerColorDropdown.options.Add(item);
            }
            Facade.PlayerColorDropdown.value = _Registry.RowsCount - 1;
            Facade.PlayerColorDropdown.captionImage.sprite = Facade.PlayerColorDropdown.options[Facade.PlayerColorDropdown.value].image;
            Facade.PlayerColorDropdown.captionImage.enabled = true;
        }

        private void OnPlayerTypeDropdownChanged(int index)
        {
            switch (index)
            {
                case 0:
                    OnNonePlayerSelected();
                    break;
                case 1:
                    OnHumanPlayerSelected();
                    break;
                case 2:
                    OnComputerPlayerSelected();
                    break;
            }
        }

        private void OnNonePlayerSelected()
        {
            Facade.PlayerNameInputField.text = string.Empty;
            _currentColorIndex = -1;

            Facade.PlayerNameInputField.gameObject.SetActive(false);
            Facade.PlayerColorDropdown.gameObject.SetActive(false);
            UpdateComputerNames();
        }

        private void OnHumanPlayerSelected()
        {
            if (Facade.PlayerNameInputField.gameObject.activeSelf == false)
            {
                var activeCount = _Registry.Rows.Count(row => row.Facade.PlayerColorDropdown.gameObject.activeSelf);
                if (activeCount == 0)
                {
                    Facade.PlayerColorDropdown.value = 0;
                }
                else
                {
                    var activeColors = _Registry.Rows.Where(row => row.Facade.PlayerColorDropdown.gameObject.activeSelf)
                        .Select(activeRow => activeRow.Facade.PlayerColorDropdown.captionImage.sprite.texture.GetPixel(0, 0)).ToArray();
                    var neededColor = _Settings.PlayerColors.Except(activeColors).First();
                    Facade.PlayerColorDropdown.value = Facade.PlayerColorDropdown.options
                        .IndexOf(Facade.PlayerColorDropdown.options.First(x => x.image.texture.GetPixel(0, 0) == neededColor));
                }
                _currentColorIndex = Facade.PlayerColorDropdown.value;
            }

            Facade.PlayerNameInputField.interactable = true;
            Facade.PlayerNameInputField.gameObject.SetActive(true);
            Facade.PlayerColorDropdown.gameObject.SetActive(true);
            UpdateComputerNames();
        }

        private void OnComputerPlayerSelected()
        {
            OnHumanPlayerSelected();
            Facade.PlayerNameInputField.interactable = false;
        }

        private void UpdateComputerNames()
        {
            var computers = _Registry.Rows.Where(row => row.Facade.PlayerTypeDropdown.value == 2).ToList();
            for (var i = 0; i < computers.Count; i++)
            {
                computers[i].Facade.PlayerNameInputField.text = "Computer " + (i + 1);
            }
        }

        private void OnPlayerColorDropdownChanged(int index)
        {
            if (_Registry.Rows.Any(row => row._currentColorIndex == index))
            {
                Facade.PlayerColorDropdown.value = _currentColorIndex;
            }
            else
            {
                _currentColorIndex = Facade.PlayerColorDropdown.value;
            }
        }

        private void OnPlayerNameFieldEndEdit(string value)
        {
            Facade.PlayerNameInputField.text = value.Trim();
        }

        [Serializable]
        public class Settings
        {
            public Color[] PlayerColors;

            public void RepairColors()
            {
                for (var i = 0; i < PlayerColors.Length; i++)
                {
                    var color = PlayerColors[i];
                    if (color.r < 0.0001f)
                        PlayerColors[i].r = 0;
                    if (color.g < 0.0001f)
                        PlayerColors[i].g = 0;
                    if (color.b < 0.0001f)
                        PlayerColors[i].b = 0;
                    if (color.a < 0.0001f)
                        PlayerColors[i].a = 0;
                }
            }
        }
    }
}