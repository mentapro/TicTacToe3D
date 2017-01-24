using System.Collections.Generic;

namespace TicTacToe3D
{
    public class PlayerRowRegistry
    {
        private readonly List<PlayerRowModel> _playerRows = new List<PlayerRowModel>();

        public IEnumerable<PlayerRowModel> Rows
        {
            get { return _playerRows; }
        }

        public int RowsCount
        {
            get { return _playerRows.Count; }
        }

        public void AddRow(PlayerRowModel row)
        {
            _playerRows.Add(row);
        }
    }
}