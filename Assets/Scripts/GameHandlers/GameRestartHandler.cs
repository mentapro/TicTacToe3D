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
            foreach (var player in Info.Players)
            {
                player.State = PlayerStates.Plays;
                if (Info.GameSettings.TimerType == TimerTypes.FixedTimePerRound ||
                    Info.GameSettings.TimerType == TimerTypes.FixedTimePerStep)
                {
                    player.TimeLeft = Info.TimerTime;
                }
            }

            Info.GameState = GameStates.Started;
        }
    }
}