using System;
using System.Collections.Generic;

namespace TicTacToe3D
{
    public class GameEvents
    {
        public Action<BadgeModel, bool> BadgeSpawned = delegate { };
        public Action StepConfirmed = delegate { };
        public Action<Player> PlayerWonSignal = delegate { };
        public Action<Player> PlayerLostSignal = delegate { };
        public Action TimePassed = delegate { };
        public Action<List<HistoryItem>> UndoSignal = delegate { };
    }
}