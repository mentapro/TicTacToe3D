using System.Collections.Generic;

namespace TicTacToe3D
{
    public partial class SaveItemModel
    {
        public class Registry
        {
            private readonly List<SaveItemModel> _saveItems = new List<SaveItemModel>();

            public IEnumerable<SaveItemModel> Items
            {
                get { return _saveItems; }
            }
            
            public int ItemsCount
            {
                get { return _saveItems.Count; }
            }

            public void AddRow(SaveItemModel saveItem)
            {
                _saveItems.Add(saveItem);
            }

            public void RemoveRow(SaveItemModel saveItem)
            {
                _saveItems.Remove(saveItem);
            }

            public void Clear()
            {
                for (var i = _saveItems.Count - 1; i >= 0; i--)
                {
                    _saveItems[i].Destroy();
                }
            }
        }
    }
}