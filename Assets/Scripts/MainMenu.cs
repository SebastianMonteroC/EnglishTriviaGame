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
        SceneManager.LoadScene("LevelPicker");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Settings() {
        SoundManager.Instance.PlaySFX("settings");
        SceneManager.LoadScene("SettingsMenu");
    }
}
