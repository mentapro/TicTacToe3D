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
            get { return Model.Owner; }
            set { Model.Owner = value; }
        }
        
        public Point Coordinates
        {
            set { Model.Coordinates = value; }
        }

        public Color Color
        {
            set { Model.SetBadgeColor(value); }
        }

        public bool IsConfirmed
        {
            set { Model.IsConfirmed = value; }
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