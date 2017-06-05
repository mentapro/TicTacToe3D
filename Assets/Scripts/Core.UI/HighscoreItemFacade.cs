using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToe3D
{
    public class HighscoreItemFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(HighscoreItemModel model)
        {
            _model = model;
            model.SetFacade(this);
        }

        private HighscoreItemModel _model;
        [SerializeField]
        private Text _playerNameText = null;
        [SerializeField]
        private Text _totalScoreText = null;
        [SerializeField]
        private Text _wonRoundsText = null;

        public Text PlayerNameText
        {
            get { return _playerNameText; }
        }

        public Text TotalScoreText
        {
            get { return _totalScoreText; }
        }

        public Text WonRoundsText
        {
            get { return _wonRoundsText; }
        }

        public class Factory : Factory<HighscoreItemFacade>
        { }
    }
}