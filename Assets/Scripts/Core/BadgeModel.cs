using System.Linq;
using ModestTree;
using UnityEngine;

namespace TicTacToe3D
{
    public partial class BadgeModel
    {
        private BadgeFacade Facade { get; set; }
        private Registry _Registry { get; set; }

        public bool IsConfirmed { get; set; }
        public Point Coordinates { get; set; }
        public Player Owner { get; set; }
        public ParticleSystem Glowing
        {
            get { return Facade.Glowing; }
        }

        public BadgeModel(Registry registry)
        {
            _Registry = registry;

            registry.AddBadge(this);
        }

        public void SetFacade(BadgeFacade facade)
        {
            Facade = facade;
        }
        
        public void Destroy()
        {
            Assert.That(_Registry.Badges.Last() == this, "You can destroy only last badge in registry");

            _Registry.RemoveBadge(this);
            Object.Destroy(Facade.gameObject);
        }

        public void SetBadgeColor(Color color)
        {
            Facade.Renderer.material.color = color;
            var mainModule = Glowing.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(Owner.Color);
        }
    }
}