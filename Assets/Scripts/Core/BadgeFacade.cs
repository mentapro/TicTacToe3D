using System;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class BadgeFacade : MonoBehaviour
    {
        [Inject]
        public void Construct(BadgeModel model)
        {
            Model = model;

            model.SetFacade(this);
            Renderer = GetComponent<Renderer>();
        }

        public Renderer Renderer { get; private set; }
        public BadgeModel Model { get; private set; }

        public Player Owner
        {
            set { Model.SetOwner(value); }
        }
        
        public Point Coordinates
        {
            set { Model.SetCoordinates(value); }
        }

        public void SetColor(Color color)
        {
            Model.SetBadgeColor(color);
        }

        private void OnDestroy()
        {
            Model.Dispose();
        }

        [Serializable]
        public class Settings
        {
            public float Diameter;
        }

        public class Factory : Factory<BadgeFacade>
        { }
    }
}