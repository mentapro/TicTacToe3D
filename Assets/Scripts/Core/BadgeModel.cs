using UnityEngine;

namespace TicTacToe3D
{
    public partial class BadgeModel
    {
        private BadgeFacade Facade { get; set; }
        private Registry _Registry { get; set; }

        public Point Coordinates { get; private set; }
        public Player Owner { get; private set; }
        
        public BadgeModel(Registry registry)
        {
            _Registry = registry;

            registry.AddBadge(this);
        }

        public void SetFacade(BadgeFacade facade)
        {
            Facade = facade;
        }

        public void Dispose()
        {
            
        }

        public void SetCoordinates(Point coordinates)
        {
            Coordinates = coordinates;
        }

        public void SetOwner(Player owner)
        {
            Owner = owner;
        }

        public void SetBadgeColor(Color color)
        {
            Facade.Renderer.material.color = color;
        }
    }
}