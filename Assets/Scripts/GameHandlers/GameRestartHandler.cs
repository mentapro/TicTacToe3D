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
            Reset();
            Info.GameState = GameStates.Started;
        }
        
        private void Reset()
        {
            foreach (var player in Info.Players)
            {
                player.State = PlayerStates.Plays;
                if (Info.GameSettings.TimerType == TimerTypes.FixedTimePerRound ||
                    Info.GameSettings.TimerType == TimerTypes.FixedTimePerStep)
                {
                    player.TimeLeft = Info.TimerTime;
                }
            }
        }
    }
}