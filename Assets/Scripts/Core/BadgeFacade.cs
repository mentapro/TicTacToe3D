using System;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class BadgeFacade : MonoBehaviour
    {
        public Renderer Renderer;
        public ParticleSystem Glowing;

        [Inject]
        public void Construct(BadgeModel model)
        {
            Model = model;

            model.SetFacade(this);
            Renderer.material.renderQueue++;
        }

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

        [Serializable]
        public class Settings
        {
            public float Diameter;
        }

        public class Factory : Factory<BadgeFacade>
        { }
    }
}