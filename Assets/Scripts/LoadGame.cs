using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LoadGame : MonoBehaviour
{
    [SerializeField] public GameObject NoSavedGames;
    public GameObject SavedGamePrefab;
    private int savedGamesAmount = 0;

    void Start()
    {
        if(PlayerPrefs.HasKey("save1")){
            savedGamesAmount++;
            NoSavedGames.SetActive(false);
            InstantiateSaveFile(1);
            if(PlayerPrefs.HasKey("save2")){
                savedGamesAmount++;
                InstantiateSaveFile(2);
                if(PlayerPrefs.HasKey("save3")){
                    InstantiateSaveFile(3);
                }    
            }     
        }
    }

    private void InstantiateSaveFile(int save_id) {
        GameObject childObject = Instantiate(SavedGamePrefab) as GameObject;
        childObject.transform.SetParent(GameObject.Find("Panel").transform, false);
        childObject.transform.Find("SaveName").gameObject.GetComponent<Text>().text = "Save #" + save_id.ToString();
        childObject.transform.Find("Grade").gameObject.GetComponent<Text>().text = "Grade: " + PlayerPrefs.GetString("grade" + save_id.ToString());
        childObject.transform.Find("Unit").gameObject.GetComponent<Text>().text = "Unit: " + PlayerPrefs.GetString("unit" + save_id.ToString());
        childObject.transform.Find("Rounds").gameObject.GetComponent<Text>().text = "Turns: " + PlayerPrefs.GetInt("turnCounter" + save_id.ToString()).ToString();
        childObject.GetComponent<Button>().onClick.AddListener(delegate { SelectSavedGame(save_id); });
    }
    private void SelectSavedGame(int save_id) {
        GameManager.loadedGame = save_id;
        GameManager.pointsToWin = PlayerPrefs.GetInt("pointsToWin" + save_id);
        GameManager.time = PlayerPrefs.GetInt("time" + save_id);
        GameManager.timerEnabled = PlayerPrefs.GetInt("timerEnabled" + save_id) == 1 ? true : false; //1 = true, 0 = false;
        GameManager.currentTeamId = PlayerPrefs.GetInt("currentTeamId" + save_id);
        GameManager.turnCounter = PlayerPrefs.GetInt("turnCounter" + save_id);
        GameManager.unit = PlayerPrefs.GetString("unit" + save_id, GameManager.unit);
        GameManager.grade = PlayerPrefs.GetString("grade" + save_id, GameManager.grade);
        GameManager.customQuestionBank = PlayerPrefs.GetString("customQuestionBank" + save_id, GameManager.customQuestionBank);
        GameManager.teams = new List<Team>();
        for(int i = 0; i < PlayerPrefs.GetInt("save" + save_id.ToString() + "_team_count"); i++) {
            List<string> teamPowerUps = new List<string>();
            if(PlayerPrefs.GetInt("save" + save_id.ToString() + "_team" + i.ToString() + "_powerUpCount") > 0) {
                Debug.Log("Team " + i.ToString() + " has: " + PlayerPrefs.GetInt("save" + save_id.ToString() + "_team" + i + "_powerUpCount") + " powerups");
                for(int powerUps = 0; powerUps < PlayerPrefs.GetInt("save" + save_id.ToString() + "_team" + i + "_powerUpCount"); powerUps++) {
                    Debug.Log("Adding powerup #" + powerUps.ToString());
                    teamPowerUps.Add(PlayerPrefs.GetString("save" + save_id.ToString() + "_team" + i.ToString() + "_powerUp" + powerUps));
                }
            }
            Team newTeam = new Team(
                PlayerPrefs.GetString("save" + save_id.ToString() + "_team" + i.ToString() + "_name"),
                PlayerPrefs.GetInt("save" + save_id.ToString() + "_team" + i.ToString() + "_score"),
                teamPowerUps);
            GameManager.teams.Add(newTeam);
        }
        GameManager.justLoaded = true;
        SoundManager.Instance.PlaySFX("beginGame");
        SceneManager.LoadScene("WheelScreen");
    }

    public void BackToMenu() {
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("MainMenu");
    }
}
