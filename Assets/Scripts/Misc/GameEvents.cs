using System;
using System.Collections.Generic;

namespace TicTacToe3D
{
    public class GameEvents
    {
        public Action<BadgeModel, bool> BadgeSpawned = delegate { };
        public Action StepConfirmed = delegate { };
        public Action PlayerWonSignal = delegate { };
        public Action PlayerLostSignal = delegate { };
        public Action TimePassed = delegate { };
        public Action<List<HistoryItem>> UndoSignal = delegate { };
    }
}