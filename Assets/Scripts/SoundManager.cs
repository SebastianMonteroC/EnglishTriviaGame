using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip audio;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public Sound[] sfxSounds;
    public AudioSource sfxSource;
    private string currentSound;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(string name) {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        currentSound = name;
        if (sound != null) {
            sfxSource.PlayOneShot(sound.audio);
        }
    }

    public void StopSFX() {
        sfxSource.Stop();
    }
}
