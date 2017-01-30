using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class ModalDialogView : MonoBehaviour
    {
        [Inject]
        public void Construct(ModalDialog presenter)
        {
            presenter.SetView(this);
        }

        [SerializeField]
        private Text _dialogMessageText = null;
        
        [SerializeField]
        private Button _button1 = null;
        [SerializeField]
        private Button _button2 = null;
        [SerializeField]
        private Button _button3 = null;

        [SerializeField]
        private Text _button1Text = null;
        [SerializeField]
        private Text _button2Text = null;
        [SerializeField]
        private Text _button3Text = null;

        public Text DialogMessageText
        {
            get { return _dialogMessageText; }
        }

        public Button Button1
        {
            get { return _button1; }
        }

        public Button Button2
        {
            get { return _button2; }
        }

        public Button Button3
        {
            get { return _button3; }
        }

        public Text Button1Text
        {
            get { return _button1Text; }
        }

        public Text Button2Text
        {
            get { return _button2Text; }
        }
        
        public Text Button3Text
        {
            get { return _button3Text; }
        }
    }
}