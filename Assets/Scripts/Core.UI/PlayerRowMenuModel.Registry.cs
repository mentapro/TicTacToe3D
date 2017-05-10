using System.Collections.Generic;

namespace TicTacToe3D
{
    public partial class PlayerRowMenuModel
    {
        public class Registry
        {
            private readonly List<PlayerRowMenuModel> _playerRows = new List<PlayerRowMenuModel>();

            public IEnumerable<PlayerRowMenuModel> Rows
            {
                get { return _playerRows; }
            }

            public int RowsCount
            {
                get { return _playerRows.Count; }
            }

            public void AddRow(PlayerRowMenuModel row)
            {
                _playerRows.Add(row);
            }
            
            public List<Player> GetValidatedPlayers()
            {
                var list = new List<Player>();
                foreach (var playerRow in _playerRows)
                {
                    var playerTypeIndex = playerRow.Facade.PlayerTypeDropdown.value;
                    var playerName = playerRow.Facade.PlayerNameInputField.text.ToLower();

                    if (playerTypeIndex != 0 && playerName != string.Empty && playerRow._currentColorIndex != -1 && list.TrueForAll(x => x.Name.ToLower() != playerName))
                    {
                        var playerType = playerTypeIndex == 1 ? PlayerTypes.Human : PlayerTypes.AI;
                        var playerColor = playerRow.Facade.PlayerColorDropdown.captionImage.sprite.texture.GetPixel(0, 0);
                        list.Add(new Player(playerType, playerName, playerColor));
                    }
                }
                return list;
            }
        }
    }
}