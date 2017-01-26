using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D
{
    public class PlayerRowModel
    {
        private int _currentColorItem = -1;
        private PlayerRowFacade Facade { get; set; }
        private PlayerRowRegistry Registry { get; set; }
        private Settings _Settings { get; set; }

        public PlayerRowModel(PlayerRowRegistry registry, Settings settings)
        {
            Registry = registry;
            _Settings = settings;

            registry.AddRow(this);
        }

        public void SetFacade(PlayerRowFacade facade)
        {
            Facade = facade;
            Initialize();
        }

        public void Dispose()
        {
            Facade.PlayerNameInputField.onEndEdit.RemoveAllListeners();
            Facade.PlayerColorDropdown.onValueChanged.RemoveAllListeners();
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
                var item = new Dropdown.OptionData(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0)));
                Facade.PlayerColorDropdown.options.Add(item);
            }
            Facade.PlayerColorDropdown.value = Registry.RowsCount - 1;
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
            }
        }

        private void OnNonePlayerSelected()
        {
            Facade.PlayerNameInputField.text = string.Empty;
            _currentColorItem = -1;

            Facade.PlayerNameInputField.gameObject.SetActive(false);
            Facade.PlayerColorDropdown.gameObject.SetActive(false);
        }
        
        private void OnHumanPlayerSelected()
        {
            var activeCount = Registry.Rows.Count(row => row.Facade.PlayerColorDropdown.gameObject.activeSelf);
            if (activeCount == 0)
            {
                Facade.PlayerColorDropdown.value = 0;
            }
            else
            {
                var activeColors = Registry.Rows.Where(row => row.Facade.PlayerColorDropdown.gameObject.activeSelf)
                    .Select(activeRow => activeRow.Facade.PlayerColorDropdown.captionImage.sprite.texture.GetPixel(0, 0)).ToArray();
                var neededColor = _Settings.PlayerColors.Except(activeColors).First();
                Facade.PlayerColorDropdown.value = Facade.PlayerColorDropdown.options
                    .IndexOf(Facade.PlayerColorDropdown.options.First(x => x.image.texture.GetPixel(0, 0) == neededColor));
            }
            _currentColorItem = Facade.PlayerColorDropdown.value;
            Facade.PlayerNameInputField.gameObject.SetActive(true);
            Facade.PlayerColorDropdown.gameObject.SetActive(true);
        }

        private void OnPlayerColorDropdownChanged(int index)
        {
            if (Registry.Rows.Any(row => row._currentColorItem == index))
            {
                Facade.PlayerColorDropdown.value = _currentColorItem;
            }
            else
            {
                _currentColorItem = Facade.PlayerColorDropdown.value;
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