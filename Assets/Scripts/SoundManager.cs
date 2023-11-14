using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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
    public Sound[] musicSounds;
    public List<Sound> unitListeningSounds;
    public AudioImporter importer;
    public AudioSource sfxSource;
    public AudioSource musicSource;
    private string currentSound;
    private string currentSong;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            this.unitListeningSounds = new List<Sound>();
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name) {
        Sound sound = Array.Find(musicSounds, x => x.name == name);
        currentSong = name;
        if (sound != null) {
            musicSource.PlayOneShot(sound.audio);
            musicSource.loop = true;
        }
    }

    public void LowerVolumeForListening() {
        musicSource.volume = 0.05f;
    }
    
    public void ResetVolume() {
        musicSource.volume = GameManager.musicVolume;
    }

    public void LoopMusic(bool loop) {
        musicSource.loop = loop;
    }

    public void StopMusic() {
        musicSource.Stop();
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

    public IEnumerator LoadAudio(string filename, int count)
    {
        for(int i = 0; i < count; i++) {
            string i_filename = filename.Replace("{#}", i.ToString());
            string path = Application.streamingAssetsPath + "/ListeningFiles/" + i_filename + ".mp3";
            
            importer.Import(path);

            while (!importer.isInitialized && !importer.isError)
                yield return null;

            if (importer.isError)
                Debug.LogError(importer.error);

            AudioClip listeningClip = importer.audioClip;
            Sound listeningSound = new Sound();
            listeningSound.name = i_filename;
            listeningSound.audio = listeningClip;
            unitListeningSounds.Add(listeningSound);
        }
    }

    public IEnumerator LoadCustomAudio(List<Question> audios){
        foreach(var audioPath in audios) {
            string path = audioPath.path;
            
            importer.Import(path);

            while (!importer.isInitialized && !importer.isError)
                yield return null;

            if (importer.isError)
                Debug.LogError(importer.error);

            AudioClip listeningClip = importer.audioClip;
            Sound listeningSound = new Sound();
            listeningSound.name = audioPath.path;
            listeningSound.audio = listeningClip;
            unitListeningSounds.Add(listeningSound);
        }
    }

    public void PlayListeningAudio(string filename) {
        Sound sound = unitListeningSounds.Find(x => x.name == filename);
        if (sound != null) {
            Debug.Log("playing");
            sfxSource.PlayOneShot(sound.audio);
        } else {
            Debug.Log("null audio");
        }
    }

    public void ClearUnitSounds() {
        this.unitListeningSounds.Clear();
    }

    public void SetMusicVolume(float volume){
        musicSource.volume = volume;
        GameManager.musicVolume = volume;
    }

    public void SetSFXVolume(float volume){
        sfxSource.volume = volume;
        GameManager.sfxVolume = volume;
    }
}
