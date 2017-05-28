using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class SaveItemFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(SaveItemModel model)
        {
            _model = model;
            model.SetFacade(this);
        }

        private SaveItemModel _model;
        [SerializeField]
        private Toggle _saveItemToggle = null;
        [SerializeField]
        private Text _saveItemToggleText = null;

        public History History
        {
            set { _model.History = value; }
        }

        public Toggle SaveItemToggle
        {
            get { return _saveItemToggle; }
        }

        public Text SaveItemToggleText
        {
            get { return _saveItemToggleText; }
        }

        public class Factory : Factory<SaveItemFacade>
        { }
    }
}
