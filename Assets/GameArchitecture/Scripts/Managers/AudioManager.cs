using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameArchitecture
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField]
        private Sound[] sounds;

        [Header("Settings")]
        [SerializeField]
        private bool dontDestroy;
        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            if (dontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.playOnAwake = false;
                s.source.loop = false;
                s.source.clip = s.clip;
            }
        }

        #region PlayClip
        public void PlayClip(int index)
        {
            Sound s = sounds[index];
            if (s == null)
            {
                return;
            }
            s.source.volume = s.volume * PlayerPrefs.GetInt("audio", 1);
            s.source.pitch = s.pitch;

            s.source.Play();
        }

        public void PlayClip(string name)
        {
            try
            {
                Sound s = Array.Find(sounds, item => item.name == name);
                if (s == null)
                {
                    return;
                }
                s.source.volume = s.volume * PlayerPrefs.GetInt("audio", 1);
                s.source.pitch = s.pitch;

                s.source.Play();
            }
            catch { }
        }

        public void PlayClipOneShot(string name){
            try
            {
                Sound s = Array.Find(sounds, item => item.name == name);
                if (s == null)
                {
                    return;
                }
                GameObject oneShot = Instantiate(new GameObject());
                oneShot.name = "OneShotAudio";
                AudioSource oneShotSrc = oneShot.AddComponent<AudioSource>();
                oneShotSrc.clip = s.source.clip;
                oneShotSrc.volume = s.volume * PlayerPrefs.GetInt("audio", 1);
                oneShotSrc.pitch = s.pitch;
                oneShotSrc.Play();
                Destroy(oneShot.gameObject, 1);
            }
            catch { }
        }
        #endregion
        #region PlayClipWithRandomPitch
        public void PlayClipWithRandomPitch(int index, float minPitch, float maxPitch)
        {
            float randomPitch = UnityEngine.Random.Range(minPitch, maxPitch);

            Sound s = sounds[index];
            if (s == null)
            {
                return;
            }
            s.source.volume = s.volume;
            s.source.pitch = randomPitch;

            s.source.Play();
        }

        public void PlayClipWithRandomPitch(string name, float minPitch, float maxPitch)
        {
            float randomPitch = UnityEngine.Random.Range(minPitch, maxPitch);

            Sound s = Array.Find(sounds, item => item.name == name);
            if (s == null)
            {
                return;
            }
            s.source.volume = s.volume * PlayerPrefs.GetInt("audio", 1);
            s.source.pitch = randomPitch;

            s.source.Play();
        }
        #endregion
        #region PlayClipWithRandomVolume
        public void PlayClipWithRandomVolume(int index, float minVolume, float maxVolume)
        {
            float randomVolume = UnityEngine.Random.Range(minVolume, maxVolume);

            Sound s = sounds[index];
            if (s == null)
            {
                return;
            }
            s.source.volume = randomVolume;
            s.source.pitch = s.pitch;

            s.source.Play();
        }

        public void PlayClipWithRandomVolume(string name, float minVolume, float maxVolume)
        {
            float randomVolume = UnityEngine.Random.Range(minVolume, maxVolume);

            Sound s = Array.Find(sounds, item => item.name == name);
            if (s == null)
            {
                return;
            }
            s.source.volume = randomVolume;
            s.source.pitch = s.pitch;

            s.source.Play();
        }
        #endregion
    }
}
