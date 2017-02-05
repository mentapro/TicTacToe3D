using System.Collections;
using System.Linq;
using UnityEngine;

namespace TicTacToe3D
{
    public class BadgeSpawner
    {
        private GameInfo Info { get; set; }
        private BadgeFacade.Factory BadgeFactory { get; set; }
        private BadgeFacade.Settings BadgeSettings { get; set; }
        private BadgeSpawnPoint.Registry SpawnPointRegistry { get; set; }
        private BadgeSpawned BadgeSpawned { get; set; }

        public BadgeSpawner(GameInfo info,
            BadgeFacade.Factory badgeFactory,
            BadgeFacade.Settings badgeSettings,
            BadgeSpawnPoint.Registry spawnPointRegistry,
            BadgeSpawned badgeSpawned)
        {
            Info = info;
            BadgeFactory = badgeFactory;
            BadgeSettings = badgeSettings;
            SpawnPointRegistry = spawnPointRegistry;
            BadgeSpawned = badgeSpawned;
        }

        public void MakeStep(Point coordinates)
        {
            var badge = BadgeFactory.Create();
            badge.transform.localScale = new Vector3(BadgeSettings.Diameter, BadgeSettings.Diameter, BadgeSettings.Diameter);
            badge.transform.SetParent(SpawnPointRegistry.Spawns.First(x => x.Coordinates == coordinates).transform, true);
            badge.transform.localPosition = Vector3.zero;
            CreateAndPlayBadgeSpawnAnimation(badge);
            
            var activePlayer = Info.Players.First(x => x.IsActive);
            badge.Owner = activePlayer;
            badge.Coordinates = coordinates;
            badge.SetColor(activePlayer.Color);

            BadgeSpawned.Fire(badge.Model);
        }

        private void CreateAndPlayBadgeSpawnAnimation(BadgeFacade badge)
        {
            var animation = badge.gameObject.AddComponent<Animation>();

            var keys = new Keyframe[3];
            keys[0] = new Keyframe(0.0f, 0.0f);
            keys[1] = new Keyframe(0.2f, badge.transform.localScale.x + badge.transform.localScale.x / 100 * 40);
            keys[2] = new Keyframe(0.4f, badge.transform.localScale.x);
            var curveX = new AnimationCurve(keys);

            keys[0] = new Keyframe(0.0f, 0.0f);
            keys[1] = new Keyframe(0.2f, badge.transform.localScale.y + badge.transform.localScale.y / 100 * 40);
            keys[2] = new Keyframe(0.4f, badge.transform.localScale.y);
            var curveY = new AnimationCurve(keys);

            keys[0] = new Keyframe(0.0f, 0.0f);
            keys[1] = new Keyframe(0.2f, badge.transform.localScale.z + badge.transform.localScale.z / 100 * 40);
            keys[2] = new Keyframe(0.4f, badge.transform.localScale.z);
            var curveZ = new AnimationCurve(keys);

            var clip = new AnimationClip { legacy = true };
            clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
            clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
            clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
            animation.AddClip(clip, "BadgeSpawnAnim");
            animation.Play("BadgeSpawnAnim");
            badge.StartCoroutine(DestroyAnimation(animation));
        }

        private IEnumerator DestroyAnimation(Animation animation)
        {
            while (animation.isPlaying)
            {
                yield return null;
            }
            Object.Destroy(animation);
        }
    }
}