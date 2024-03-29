using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Start() {
        if(!GameManager.menuMusic) {
            SoundManager.Instance.PlayMusic("main-theme");
            GameManager.menuMusic = true;
        }
        
        SoundManager.Instance.LoopMusic(true);
        Time.timeScale = 1;
    }

    public void NewGame() {
        SoundManager.Instance.PlaySFX("newGame");
        SceneManager.LoadScene("GameModeSelect");
    }

    public void LoadGame() {
        SoundManager.Instance.PlaySFX("newGame");
        SceneManager.LoadScene("LoadGame");
    }

    public void Create() {
        SoundManager.Instance.PlaySFX("settings");
        SceneManager.LoadScene("CustomBankMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Settings() {
        
        SoundManager.Instance.PlaySFX("settings");
        SceneManager.LoadScene("SettingsMenu");
    }
}
