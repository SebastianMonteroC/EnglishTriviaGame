using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetGame : MonoBehaviour
{
    private List<Team> teamList;
    private int pointsToWin;
    private int timerSeconds;
    private bool timerEnabled;
    public Text pointsText;
    public InputField inputField;
    public GameObject timerSetting;
    public Button addTeamButton, undoTeamButton, addPointsButton, subtractPointsButton, startButton, addSeconds, subtractSeconds;

    void Start() {
        GameManager.teams.Clear();
        inputField = GameObject.Find("TeamNameInput").GetComponent<InputField>();
        addTeamButton = GameObject.Find("AddButton").GetComponent<Button>();
        undoTeamButton = GameObject.Find("UndoButton").GetComponent<Button>();
        addPointsButton = GameObject.Find("HigherPoints").GetComponent<Button>();
        subtractPointsButton = GameObject.Find("LowerPoints").GetComponent<Button>();
        pointsText = GameObject.Find("PointsToWin").GetComponent<Text>();
        startButton = GameObject.Find("StartGame").GetComponent<Button>();
        addSeconds = GameObject.Find("HigherSecs").GetComponent<Button>();
        subtractSeconds = GameObject.Find("LowerSecs").GetComponent<Button>();
        timerSetting = GameObject.Find("TimerAmount");
        timerEnabled = true;
        teamList = new List<Team>();
        pointsToWin = 5;
        timerSeconds = 10;
        pointsText.text = "Points to win: " + pointsToWin.ToString();

        if(GameManager.grade != "" && GameManager.unit != ""){
            GameObject.Find("Grade").GetComponent<Text>().text += "Grade: " + GameManager.grade + "th grade";
            GameObject.Find("Unit").GetComponent<Text>().text += "Unit: " + GameManager.unit;
        } else {
            GameObject.Find("Grade").GetComponent<Text>().text += GameManager.customQuestionBank;
            GameObject.Find("Unit").GetComponent<Text>().text += "";
        }
    }

    void Update() {
        ButtonCheck();
    }

    public void AddNewTeam() {
        GameManager.teams.Add(new Team(inputField.text));
        SoundManager.Instance.PlaySFX("addTeam");
        RefreshTeamListScreen();
    }

    public string GenerateTeamList(){
        string teamList = "";
        int teamCount = 0;
        foreach (var i in GameManager.teams) {
            teamCount++;
            teamList += teamCount + ". " + i.teamName + "\n";
        }
        return teamList;
    }
    void RefreshTeamListScreen() {
        Text teamlistText = GameObject.Find("TempTeamText").GetComponent<Text>();
        teamlistText.text = GenerateTeamList();
        GameObject.Find("TeamsAddedCount").GetComponent<Text>().text = "Teams added: " + GameManager.teams.Count;
        switch(GameManager.teams.Count) {
            case > 3:
                teamlistText.fontSize = 65;
                GameObject.Find("MinMaxText").GetComponent<Text>().text = "A maximum of 4 teams is allowed!";
                break;
            case > 2:
                teamlistText.fontSize = 70;
                break;
            case > 1:
                teamlistText.fontSize = 75;
                GameObject.Find("MinMaxText").GetComponent<Text>().text = "";
                break;
            case > 0:
                teamlistText.fontSize = 80;
                GameObject.Find("MinMaxText").GetComponent<Text>().text = "A minimum of 2 teams is required to start the game!";
                break;
        }
    }
    
    public void UndoLastAdded() {
        GameManager.teams.RemoveAt(GameManager.teams.Count - 1);
        SoundManager.Instance.PlaySFX("removeTeam");
        RefreshTeamListScreen();
    }

    public void ButtonCheck() {
        //Check the add team button: if there is no text in the inputfield, the button is non interactable
        addTeamButton.interactable = inputField.text.Length > 0 && GameManager.teams.Count < 4 ? true : false;
        startButton.interactable = GameManager.teams.Count >= 2 ? true : false;
        undoTeamButton.interactable = GameManager.teams.Count > 0 ? true : false;

        //Check the add/subtract points to win buttons to put a cap on the amount of points
        subtractPointsButton.interactable = pointsToWin == 3 ? false : true;
        addPointsButton.interactable = pointsToWin == 10 ? false : true;

        //Check the add/subtract seconds to timer buttons to put a cap on the amount of seconds
        subtractSeconds.interactable = timerSeconds == 10 ? false : true;
        addSeconds.interactable = timerSeconds == 30 ? false : true;
    }

    public void StartGame() {
        GameManager.pointsToWin = this.pointsToWin;
        GameManager.timerEnabled = this.timerEnabled;
        GameManager.time = this.timerSeconds;
        GameManager.turnCounter = 1;
        GameManager.newGame = true;
        SoundManager.Instance.PlaySFX("beginGame");
        SceneManager.LoadScene("WheelScreen");
    }

    public void changePointAmount(bool add){ //where true is add and false is subtract
        SoundManager.Instance.PlaySFX("settingArrow");
        if(add){
            pointsToWin++;
        }
        else{
            pointsToWin--;
        }
        pointsText.text = "Points to win: " + pointsToWin.ToString();
    }

    public void changeSecondsAmount(bool add){ //where true is add and false is subtract
        SoundManager.Instance.PlaySFX("settingArrow");
        if(add){
            timerSeconds++;
        }
        else{
            timerSeconds--;
        }
        timerSetting.GetComponent<Text>().text = "Timer duration: " + timerSeconds.ToString() + " secs";
    }

    public void ToggleTimer() {
        SoundManager.Instance.PlaySFX("settingArrow");
        Toggle timerToggle = GameObject.Find("Toggle").GetComponent<Toggle>();
        if(timerToggle.isOn){
            timerEnabled = true;
            timerSetting.SetActive(true);
        } else {
            timerEnabled = false;
            timerSetting.SetActive(false);
        }

    }

    public void BackToMenu() {
        GameManager.teams.Clear();
        SoundManager.Instance.PlaySFX("backButton");
        if(GameManager.customQuestionBank != ""){
            GameManager.customQuestionBank = "";
            SceneManager.LoadScene("GameModeSelect");
        } else {
            SceneManager.LoadScene("LevelPicker");
        }
    }
}
