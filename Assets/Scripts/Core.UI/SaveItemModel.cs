using UnityEngine;

namespace TicTacToe3D
{
    public partial class SaveItemModel
    {
        private readonly Registry _registry;
        private SaveItemFacade _facade;

        public SaveItemModel(Registry registry)
        {
            _registry = registry;

            registry.AddRow(this);
        }

        public History History { get; set; }

        public bool IsActive
        {
            get { return _facade.SaveItemToggle.isOn; }
        }

        public string Name
        {
            get { return _facade.SaveItemToggleText.text; }
        }

        public void SetFacade(SaveItemFacade facade)
        {
            _facade = facade;
        }

        private void Destroy()
        {
            _facade.SaveItemToggle.onValueChanged.RemoveAllListeners();
            _registry.RemoveRow(this);
            Object.Destroy(_facade.gameObject);
        }
    }
}