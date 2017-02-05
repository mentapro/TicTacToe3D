using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class PlayerInputHandler : ITickable, IDisposable
    {
        private bool _tick;
        private bool _isHumanActive;
        
        private BadgeSpawnPoint.Registry SpawnRegistry { get; set; }
        private BadgeSpawner BadgeSpawner { get; set; }
        private ActivePlayerChanged ActivePlayerChanged { get; set; }
        private GameInfo Info { get; set; }

        public PlayerInputHandler(BadgeSpawnPoint.Registry spawnRegistry,
            BadgeSpawner badgeSpawner,
            ActivePlayerChanged activePlayerChanged,
            GameInfo info)
        {
            SpawnRegistry = spawnRegistry;
            BadgeSpawner = badgeSpawner;
            ActivePlayerChanged = activePlayerChanged;
            Info = info;

            ActivePlayerChanged += OnActivePlayerChanged;
            Info.PropertyChanged += OnGameInfoPropertyChanged;
        }

        public void Tick()
        {
            if (_tick == false) return;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (SpawnRegistry.Spawns.Any(spawn => spawn.gameObject.GetInstanceID() == hit.collider.gameObject.GetInstanceID()))
                {
                    var spawnPoint = hit.collider.GetComponent<BadgeSpawnPoint>();
                    if (spawnPoint.Badge != null)
                    {
                        return;
                    }

                    if (Input.GetMouseButton(1))
                    {
                        spawnPoint.OnMouseExit();
                        return;
                    }

                    spawnPoint.MakeVisible();

                    if (Input.GetMouseButtonDown(0))
                    {
                        BadgeSpawner.MakeStep(spawnPoint.Coordinates);
                        spawnPoint.OnMouseExit();
                    }
                }
            }
        }

        public void Dispose()
        {
            ActivePlayerChanged -= OnActivePlayerChanged;
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentState")
            {
                OnGameStateChanged(Info.CurrentState);
            }
        }

        private void OnGameStateChanged(GameStates state)
        {
            _tick = state == GameStates.Started && _isHumanActive;
        }

        private void OnActivePlayerChanged(Player activePlayer)
        {
            _isHumanActive = activePlayer.Type == PlayerTypes.Human;
        }
    }
}