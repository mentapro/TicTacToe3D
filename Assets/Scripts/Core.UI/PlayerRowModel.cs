using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe3D
{
    public class PlayerRowModel
    {
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
            InitializePlayerTypes();
            InitializePlayerColors();

            Facade.PlayerNameInputField.onEndEdit.AddListener(OnPlayerNameFieldChanged);
            Facade.PlayerColorDropdown.onValueChanged.AddListener(OnPlayerColorDropdownChanged);
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

        }

        private void OnPlayerNameFieldChanged(string value)
        {
            Facade.PlayerNameInputField.text = value.Trim();
        }

        private void OnPlayerColorDropdownChanged(int index)
        {

        }

        [Serializable]
        public class Settings
        {
            public Color[] PlayerColors;
        }
    }
}