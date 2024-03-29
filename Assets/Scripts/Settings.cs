using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject blockerSave;
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject SavedChanges;
    [SerializeField] GameObject SavedText;
    [SerializeField] GameObject sfxSlider;
    [SerializeField] GameObject musicSlider;

    private bool FadeOut = false;

    void Update() {
        if(FadeOut) {
            Color objectColor = SavedChanges.GetComponent<Image>().color;
            Color textColor = SavedText.GetComponent<Text>().color;
            
            float fadeAmount = objectColor.a - (0.2f * Time.deltaTime);
            objectColor = new Color (objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            textColor = new Color (textColor.r, textColor.g, textColor.b, fadeAmount);

            SavedChanges.GetComponent<Image>().color = objectColor;
            SavedText.GetComponent<Text>().color = textColor;

            if(objectColor.a <= 0) {
                FadeOut = false;
                SavedChanges.SetActive(false);
                SavedChanges.GetComponent<Image>().color = new Color (objectColor.r, objectColor.g, objectColor.b, 120);
                SavedText.GetComponent<Text>().color = new Color (textColor.r, textColor.g, textColor.b, 150);
            }
        }
    }

    void Start() {
        sfxSlider.GetComponent<Slider>().value = GameManager.sfxVolume;
        musicSlider.GetComponent<Slider>().value = GameManager.musicVolume;
    }

    public void MainMenu() {
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("MainMenu");
    }

    public void DeleteSavesPress() {
        blockerSave.SetActive(true);
        SoundManager.Instance.PlaySFX("defaultButton");
    }

    public void BackDeleteSave() {
        SoundManager.Instance.PlaySFX("backButton");
        blockerSave.SetActive(false);
    }

    public void DeletePress() {
        blocker.SetActive(true);
        SoundManager.Instance.PlaySFX("defaultButton");
    }

    public void BackDelete() {
        SoundManager.Instance.PlaySFX("backButton");
        blocker.SetActive(false);
    }

    public void DeleteSaveFiles(){
        PlayerPrefs.DeleteAll();
        Directory.CreateDirectory(Application.persistentDataPath);
        BackDelete();
        SavedChanges.SetActive(true);
        blockerSave.SetActive(false);
        FadeOut = true;
    }

    public void DeleteEverything(){
        PlayerPrefs.DeleteAll();
        if (Directory.Exists(Application.persistentDataPath)) { Directory.Delete(Application.persistentDataPath, true); }
        Directory.CreateDirectory(Application.persistentDataPath);
        BackDelete();
        SavedChanges.SetActive(true);
        FadeOut = true;
    }

    public void SfxVolume(){
        SoundManager.Instance.SetSFXVolume(sfxSlider.GetComponent<Slider>().value);
    }

    public void MusicVolume(){
        SoundManager.Instance.SetMusicVolume(musicSlider.GetComponent<Slider>().value);
    }
}
