using UnityEngine;

namespace TicTacToe3D
{
    public partial class HighscoreItemModel
    {
        private readonly Registry _registry;
        private HighscoreItemFacade _facade;

        public HighscoreItemModel(Registry registry)
        {
            _registry = registry;

            registry.AddRow(this);
        }

        public void SetFacade(HighscoreItemFacade facade)
        {
            _facade = facade;
        }

        private void Destroy()
        {
            _registry.RemoveRow(this);
            Object.Destroy(_facade.gameObject);
        }
    }
}