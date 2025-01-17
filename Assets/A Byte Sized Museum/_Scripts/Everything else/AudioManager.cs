using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace KaChow.AByteSizedMuseum
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public Sound[] musicSounds, sfxSounds;
        public AudioSource musicSource, sfxSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }

            else
            {
                Destroy(gameObject);
            }
        }

        // private void Start()
        // {
        //     PlayMusic("Theme");
        // }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            string music = scene.name switch
            {
                "MainMenu" or "Tutorial" or "A Byte Sized Museum" => "Theme",
                "Cutscene" => "ThemeCutscene",
                _ => "Theme",
            };

            PlayMusic(music);
        }

        public void PlayMusic(string name)
        {
            Sound s = Array.Find(musicSounds, x => x.name == name);

            if (s == null)
            {
                Debug.Log("Sound Not Found");
            }

            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }

        public void PlaySFX(string name)

        {
            Sound s = Array.Find(sfxSounds, x => x.name == name);

            if (s == null)
            {
                Debug.Log("Sound Not Found");
            }

            else
            {
                sfxSource.PlayOneShot(s.clip);
            }
        }

        public void ToggleMusic()
        {
            musicSource.mute = !musicSource.mute;
        }

        public void ToggleSFX()
        {
            sfxSource.mute = !sfxSource.mute;
        }

        public void MusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        public void SFXVolume(float volume)
        {
            sfxSource.volume = volume;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
