using Zenject;

namespace TicTacToe3D
{
    public class GameControlHandler : IInitializable
    {
        private GameInfo Info { get; set; }
        
        public GameControlHandler(GameInfo info)
        {
            Info = info;
        }

        public void Initialize()
        {
            Info.GameState = GameStates.Started;
        }
    }
}