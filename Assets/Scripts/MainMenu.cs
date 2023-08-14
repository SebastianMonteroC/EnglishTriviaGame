using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public void NewGame() {
        SoundManager.Instance.PlaySFX("newGame");
        SceneManager.LoadScene("GameModeSelect");
    }

    public void Create() {
        SoundManager.Instance.PlaySFX("settings");
        SceneManager.LoadScene("CustomBankMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Settings() {
        //PlayerPrefs.DeleteAll();
        SoundManager.Instance.PlaySFX("settings");
        SceneManager.LoadScene("SettingsMenu");
    }
}
