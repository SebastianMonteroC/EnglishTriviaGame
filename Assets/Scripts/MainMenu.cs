using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void NewGame() {
        SceneManager.LoadScene("LevelPicker");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Settings() {
        SceneManager.LoadScene("SettingsMenu");
    }
}
