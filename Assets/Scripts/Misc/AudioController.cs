using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace TicTacToe3D
{
    public class AudioController : MonoBehaviour
    {
        [Inject]
        public void Construct(Settings settings)
        {
            AudioSettings = settings;
            ShuffleBgMusic();
        }

        private void Update()
        {
            if (SourceBackground.isPlaying) return;
            SourceBackground.PlayOneShot(NextMusic());
        }

        [SerializeField]
        private AudioSource _audio;
        [SerializeField]
        private AudioSource _audioBackground;

        public List<AudioClip> BackgroundMusic = new List<AudioClip>();
        public Settings AudioSettings { get; private set; }

        public AudioSource Source
        {
            get { return _audio; }
        }

        public AudioSource SourceBackground
        {
            get { return _audioBackground; }
        }
        
        public AudioClip NextMusic()
        {
            var clip = BackgroundMusic[0];
            BackgroundMusic.RemoveAt(0);
            BackgroundMusic.Add(clip);
            return clip;
        }

        private void ShuffleBgMusic()
        {
            var random = new Random();
            var randomInts = new List<int>();
            while (randomInts.Count != AudioSettings.BackgroundMusicClips.Length)
            {
                var randomInt = random.Next(0, AudioSettings.BackgroundMusicClips.Length);
                if (randomInts.Contains(randomInt) == false)
                {
                    randomInts.Add(randomInt);
                }
            }
            BackgroundMusic.Clear();
            for (var i = 0; i < AudioSettings.BackgroundMusicClips.Length; i++)
            {
                BackgroundMusic.Add(AudioSettings.BackgroundMusicClips[randomInts[i]]);
            }
        }

        [Serializable]
        public class Settings
        {
            public AudioClip BadgeSpawnedClip;
            public AudioClip TimerTickClip;
            public AudioClip UndoBadgesClip;
            public AudioClip ConfirmStepClip;
            public AudioClip OnPlayerWon;
            public AudioClip Victory;
            public AudioClip MenuOpenedClip;
            public AudioClip[] BackgroundMusicClips;
        }
    }
}