using System;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public partial class BadgeSpawnPoint : MonoBehaviour
    {
        [Inject]
        public void Construct(Settings settings, Registry registry)
        {
            _Settings = settings;
            registry.AddSpawn(this);

            Renderer = GetComponent<Renderer>();
        }

        private Renderer Renderer { get; set; }
        private Settings _Settings { get; set; }

        public Point Coordinates { get; set; }
        public BadgeModel Badge { get; set; }

        public void OnMouseExit()
        {
            Renderer.material.color = Color.clear;
        }

        public void MakeVisible()
        {
            Renderer.material.color = _Settings.SelectedColor;
        }

        [Serializable]
        public class Settings
        {
            public Color SelectedColor;
        }
    }
}