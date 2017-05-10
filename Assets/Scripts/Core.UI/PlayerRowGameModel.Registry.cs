using System.Collections.Generic;

namespace TicTacToe3D
{
    public partial class PlayerRowGameModel
    {
        public class Registry
        {
            private readonly List<PlayerRowGameModel> _playerRows = new List<PlayerRowGameModel>();

            public IEnumerable<PlayerRowGameModel> Rows
            {
                get { return _playerRows; }
            }

            public int RowsCount
            {
                get { return _playerRows.Count; }
            }

            public void AddRow(PlayerRowGameModel row)
            {
                _playerRows.Add(row);
            }
        }
    }
}