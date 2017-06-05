namespace TicTacToe3D
{
    public partial class PlayerRowGameModel
    {
        public Player Owner { get; set; }

        private PlayerRowGameFacade Facade { get; set; }
        private Registry _Registry { get; set; }

        public PlayerRowGameModel(Registry registry)
        {
            _Registry = registry;

            registry.AddRow(this);
        }

        public void SetFacade(PlayerRowGameFacade facade)
        {
            Facade = facade;
            Initialize();
        }

        public void TurnOnBackground()
        {
            Facade.ActivePlayerBgImage.gameObject.SetActive(true);
        }

        public void TurnOffBackground()
        {
            Facade.ActivePlayerBgImage.gameObject.SetActive(false);
        }

        public void SetState(string state)
        {
            Facade.StateText.text = state;
        }

        public void SetScore(string score)
        {
            Facade.ScoreAmountText.text = score;
        }

        public void SetWonRounds(string wonRounds)
        {
            Facade.WonAmountText.text = wonRounds;
        }

        private void Initialize()
        {

        }

        public void Dispose()
        {

        }
    }
}