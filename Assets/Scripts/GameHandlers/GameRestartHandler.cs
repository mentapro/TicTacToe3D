using Zenject;

namespace TicTacToe3D
{
    public class GameRestartHandler : IInitializable
    {
        private GameInfo Info { get; set; }
        
        public GameRestartHandler(GameInfo info)
        {
            Info = info;
        }

        public void Initialize()
        {
            Info.GameState = GameStates.Started;
        }
    }
}