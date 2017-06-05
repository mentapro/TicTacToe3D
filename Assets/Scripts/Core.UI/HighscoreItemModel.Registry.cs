using System.Collections.Generic;

namespace TicTacToe3D
{
    public partial class HighscoreItemModel
    {
        public class Registry
        {
            private readonly List<HighscoreItemModel> _highscoreItems = new List<HighscoreItemModel>();

            public IEnumerable<HighscoreItemModel> Items
            {
                get { return _highscoreItems; }
            }

            public int ItemsCount
            {
                get { return _highscoreItems.Count; }
            }

            public void AddRow(HighscoreItemModel highscoreItem)
            {
                _highscoreItems.Add(highscoreItem);
            }

            public void RemoveRow(HighscoreItemModel highscoreItem)
            {
                _highscoreItems.Remove(highscoreItem);
            }

            public void Clear()
            {
                for (var i = _highscoreItems.Count - 1; i >= 0; i--)
                {
                    _highscoreItems[i].Destroy();
                }
            }
        }
    }
}