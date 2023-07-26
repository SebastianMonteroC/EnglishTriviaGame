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
    public List<Sound> unitListeningSounds;
    public AudioSource sfxSource;
    private string currentSound;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            this.unitListeningSounds = new List<Sound>();
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

    public IEnumerator LoadAudio(string filename)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + Application.streamingAssetsPath + "/ListeningFiles/" + filename, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip listeningClip = DownloadHandlerAudioClip.GetContent(www);
                Sound listeningSound = new Sound();
                listeningSound.name = filename;
                listeningSound.audio = listeningClip;
                unitListeningSounds.Add(listeningSound);
            }
        }        
    }

    public void PlayListeningAudio(string filename) {
        Sound sound = unitListeningSounds.Find(x => x.name == filename);
        if (sound != null) {
            sfxSource.PlayOneShot(sound.audio);
        }
    }

    public void ClearUnitSounds() {
        this.unitListeningSounds.Clear();
    }
}
