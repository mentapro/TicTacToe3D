using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TicTacToe3D
{
    public class PlayerInputHandler : ITickable, IDisposable
    {
        private bool _tick;
        private bool _aiMadeStep;

        private GameInfo Info { get; set; }
        private BadgeSpawnPoint.Registry SpawnRegistry { get; set; }
        private BadgeSpawner BadgeSpawner { get; set; }
        private GameEvents GameEvents { get; set; }
        private IArtificialIntelligence Ai { get; set; }

        public PlayerInputHandler(GameInfo info,
            BadgeSpawnPoint.Registry spawnRegistry,
            BadgeSpawner badgeSpawner,
            GameEvents gameEvents,
            IArtificialIntelligence ai)
        {
            Info = info;
            SpawnRegistry = spawnRegistry;
            BadgeSpawner = badgeSpawner;
            GameEvents = gameEvents;
            Ai = ai;

            Info.PropertyChanged += OnGameInfoPropertyChanged;
            GameEvents.BadgeSpawned += OnBadgeSpawned;
        }

        public void Tick()
        {
            if (Info.ActivePlayer.Type == PlayerTypes.AI && _aiMadeStep == false && Info.GameState == GameStates.Started)
            {
                _aiMadeStep = true;
                SpawnRegistry.Spawns.First().StartCoroutine(AiWaitAndMakeStep());
                return;
            }
            if (_tick == false)
            {
                return;
            }

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
            Info.PropertyChanged -= OnGameInfoPropertyChanged;
            GameEvents.BadgeSpawned -= OnBadgeSpawned;
        }

        private IEnumerator AiWaitAndMakeStep()
        {
            yield return new WaitForSeconds(1f);
            if (Info.GameState == GameStates.Started)
            {
                BadgeSpawner.MakeStep(Ai.FindBestPoint(Info.ActivePlayer));
            }
            _aiMadeStep = false;
        }

        private void OnGameInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GameState" || e.PropertyName == "ActivePlayer" || e.PropertyName == "ActivePlayerMadeSteps")
            {
                ValidateTick();
            }
        }

        private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
        {
            if (isVictorious)
            {
                _tick = false;
            }
            else
            {
                ValidateTick();
            }
        }

        private void ValidateTick()
        {
            if (Info.GameState != GameStates.Started)
            {
                _tick = false;
                return;
            }
            if (Info.ActivePlayer.Type == PlayerTypes.AI)
            {
                _tick = false;
                return;
            }
            if (Info.ActivePlayerMadeSteps == Info.StepSize)
            {
                _tick = false;
                return;
            }
            _tick = true;
        }
    }
}