using System.Collections.Generic;
using Zenject;

namespace TicTacToe3D
{
    public class GameInfo : IInitializable
    {
        public int Dimension { get; set; }
        public int BadgesToWin { get; set; }
        public int StepSize { get; set; }
        public IList<Player> Players { get; private set; }

        public void Initialize()
        {
            Players = new List<Player>();
        }
    }
}