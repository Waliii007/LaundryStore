using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace LaundaryMan
{
    public class SoundManager : MonoBehaviour
    {
        public Sound[] sounds;
        public static SoundManager instance;
        public AudioMixer masterMixer;
        private bool isSFXMuted = false;
        private bool isBGMuted = false;
        private AudioSource bgPlayer;
        private List<Sound> backgroundSounds = new List<Sound>();
        private int currentBGIndex = 0;

        public event Action<SoundName> OnSoundPlayed;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = false;
                s.source.spatialBlend = s.spatialBlend;
                s.source.playOnAwake = false;
                var groups = masterMixer.FindMatchingGroups(s.soundType.ToString());
                if (groups.Length > 0)
                {
                    s.source.outputAudioMixerGroup = groups[0];
                }
                else
                {
                    Debug.LogWarning($"No Audio Mixer Group found for {s.soundType}");
                }

                if (s.soundType == SoundType.Background)
                {
                    backgroundSounds.Add(s);
                }
            }

            if (backgroundSounds.Count > 0)
            {
                PlayNextBackgroundMusic();
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void Play(SoundName soundName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            if (s == null)
            {
                if (GlobalConstant.isLogger)
                    print("Sound: " + soundName + " not found!");
                return;
            }

            if ((s.soundType == SoundType.SFX && isSFXMuted) || (s.soundType == SoundType.Background && isBGMuted))
                return;

            s.source.Play();
            OnSoundPlayed?.Invoke(soundName);
        }

        private void PlayNextBackgroundMusic()
        {
            if (backgroundSounds.Count == 0) return;

            if (bgPlayer != null)
            {
                bgPlayer.Stop();
            }

            bgPlayer = backgroundSounds[currentBGIndex].source;
            bgPlayer.loop = false;
            bgPlayer.Play();

            currentBGIndex = (currentBGIndex + 1) % backgroundSounds.Count;

            DOTween.Sequence()
                .AppendInterval(bgPlayer.clip.length)
                .AppendCallback(PlayNextBackgroundMusic);
        }

        public void ToggleMute(SoundType type)
        {
            if (type == SoundType.SFX)
            {
                isSFXMuted = !isSFXMuted;
                foreach (Sound s in sounds)
                {
                    if (s.soundType == SoundType.SFX)
                        s.source.mute = isSFXMuted;
                }
            }
            else if (type == SoundType.Background)
            {
                isBGMuted = !isBGMuted;
                if (bgPlayer != null)
                {
                    bgPlayer.mute = isBGMuted;
                }
            }
        }
    }

    [System.Serializable]
    public class Sound
    {
        [SerializeReference] public SoundName name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
        [Range(.1f, 3f)] public float pitch;
        public bool loop;
        [Range(0f, 1f)] public float spatialBlend = 1f; // 3D Sound
        public SoundType soundType;
        [HideInInspector] public AudioSource source;
    }

    public enum SoundType
    {
        SFX,
        Background
    }

    [Serializable]
    public class EnumWrapper<T> where T : Enum
    {
        public T value;
    }

    [Serializable]
    public class SoundNameWrapper : EnumWrapper<SoundName>
    {
    }

    public enum SoundName
    {
        Pick,
        Drop,
        BackGround,
        CashOut,
        Upgrade,
        Washing,
        Click,
        PopUp,
        Happy,
        Angry,
        CoffeeOut
    }
}