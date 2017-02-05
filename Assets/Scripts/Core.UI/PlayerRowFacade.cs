using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class PlayerRowFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(PlayerRowModel model)
        {
            Model = model;
            model.SetFacade(this);
        }

        private PlayerRowModel Model { get; set; }

        [SerializeField]
        private Dropdown _playerTypeDropdown = null;
        [SerializeField]
        private Dropdown _playerColorDropdown = null;
        [SerializeField]
        private InputField _playerNameInputField = null;

        public Dropdown PlayerTypeDropdown
        {
            get { return _playerTypeDropdown; }
        }

        public Dropdown PlayerColorDropdown
        {
            get { return _playerColorDropdown; }
        }

        public InputField PlayerNameInputField
        {
            get { return _playerNameInputField; }
        }

        private void OnDestroy()
        {
            Model.Dispose();
        }

        public class Factory : Factory<PlayerRowFacade>
        { }
    }
}