using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class PlayerRowGameFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(PlayerRowGameModel model)
        {
            Model = model;
            model.SetFacade(this);
        }

        public Player Owner
        {
            get { return Model.Owner; }
            set { Model.Owner = value; }
        }

        private PlayerRowGameModel Model { get; set; }
        
        [SerializeField]
        private Image _ActivePlayerBgImage = null;
        [SerializeField]
        private Image _colorImage = null;
        [SerializeField]
        private Text _nameText = null;
        [SerializeField]
        private Text _stateText = null;
        [SerializeField]
        private Text _scoreAmountText = null;
        [SerializeField]
        private Text _wonAmountText = null;

        public Image ColorImage
        {
            get { return _colorImage; }
        }

        public Text StateText
        {
            get { return _stateText; }
        }

        public Image ActivePlayerBgImage
        {
            get { return _ActivePlayerBgImage; }
        }

        public Text NameText
        {
            get { return _nameText; }
        }

        public Text ScoreAmountText
        {
            get { return _scoreAmountText; }
        }

        public Text WonAmountText
        {
            get { return _wonAmountText; }
        }

        private void OnDestroy()
        {
            Model.Dispose();
        }

        public class Factory : Factory<PlayerRowGameFacade>
        { }
    }
}