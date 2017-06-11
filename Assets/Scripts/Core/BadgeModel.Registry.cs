using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject;

namespace TicTacToe3D
{
    public partial class BadgeModel
    {
        public class Registry : IInitializable, IDisposable
        {
            private readonly List<BadgeModel> _badges = new List<BadgeModel>();
            private GameInfo Info { get; set; }
            private GameEvents GameEvents { get; set; }
            private AudioController AudioController { get; set; }

            public Registry(GameInfo info, GameEvents gameEvents, AudioController audioController)
            {
                Info = info;
                GameEvents = gameEvents;
                AudioController = audioController;
            }

            public IEnumerable<BadgeModel> Badges
            {
                get { return _badges; }
            }

            public int BadgesCount
            {
                get { return _badges.Count; }
            }

            public void AddBadge(BadgeModel badge)
            {
                _badges.Add(badge);
            }

            public void RemoveBadge(BadgeModel badge)
            {
                _badges.Remove(badge);
            }

            public void Initialize()
            {
                GameEvents.BadgeSpawned += OnBadgeSpawned;
                GameEvents.UndoSignal += OnUndo;
                GameEvents.StepConfirmed += OnStepConfirmed;
            }

            public void Dispose()
            {
                GameEvents.BadgeSpawned -= OnBadgeSpawned;
                GameEvents.UndoSignal -= OnUndo;
                GameEvents.StepConfirmed -= OnStepConfirmed;
            }

            private void OnBadgeSpawned(BadgeModel badge, bool isVictorious)
            {
                AudioController.Source.PlayOneShot(AudioController.AudioSettings.BadgeSpawnedClip);

                _badges.ForEach(x => x.Glowing.Stop());
                if (Info.ActivePlayerMadeSteps == 0)
                {
                    _badges.TakeLast(Info.StepSize).ForEach(x => x.Glowing.Play());
                }
                else // > 0
                {
                    _badges.TakeLast(Info.ActivePlayerMadeSteps).ForEach(x => x.Glowing.Play());
                }
            }

            private void OnStepConfirmed()
            {
                foreach (var badge in _badges.Where(x => x.IsConfirmed == false))
                {
                    badge.IsConfirmed = true;
                }
            }

            private void OnUndo(List<HistoryItem> canceledSteps)
            {
                //if (_badges.Count - 1 >= 0)
                //{
                //    _badges[_badges.Count - 1].Glowing.Play();
                //}
                _badges.ForEach(x => x.Glowing.Stop());
                if (Info.ActivePlayerMadeSteps == 0)
                {
                    if (_badges.Count > 0)
                    {
                        _badges.TakeLast(Info.StepSize).ForEach(x => x.Glowing.Play());
                    }
                }
                else // > 0
                {
                    _badges.TakeLast(Info.ActivePlayerMadeSteps).ForEach(x => x.Glowing.Play());
                }
            }
        }
    }

    public static class MiscExtensions
    {
        // Ex: collection.TakeLast(5);
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        {
            return source.Skip(Math.Max(0, source.Count() - n));
        }
    }
}