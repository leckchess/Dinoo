using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource musicAudioSource;
    private AudioSource soundAudioSource;

    [SerializeField]
    private AudioClip _menuMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        soundAudioSource = gameObject.AddComponent<AudioSource>();
        PlayMusic(_menuMusic, true);
    }

    public void MuteMusic()
    {
        musicAudioSource.volume = 1 - musicAudioSource.volume;
     }

    public void MuteSounds()
    {
        soundAudioSource.volume = 1 - soundAudioSource.volume;
    }

    public void PlayMusic(AudioClip music, bool loop)
    {
        musicAudioSource.clip = music;
        musicAudioSource.Play();
        musicAudioSource.loop = loop;
    }
    public void PlaySound(AudioClip sound)
    {
        soundAudioSource.PlayOneShot(sound);
    }
    public void PlaySound(AudioSource source, AudioClip sound)
    {
        source.PlayOneShot(sound);
    }
}
