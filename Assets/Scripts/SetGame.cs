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
    public Text pointsText;
    public InputField inputField;
    public Button addTeamButton;
    public Button undoTeamButton;
    public Button addPointsButton;
    public Button subtractPointsButton;
    public Button startButton;

    void Start() {
        GameManager.teams.Clear();
        inputField = GameObject.Find("TeamNameInput").GetComponent<InputField>();
        addTeamButton = GameObject.Find("AddButton").GetComponent<Button>();
        undoTeamButton = GameObject.Find("UndoButton").GetComponent<Button>();
        addPointsButton = GameObject.Find("HigherPoints").GetComponent<Button>();
        subtractPointsButton = GameObject.Find("LowerPoints").GetComponent<Button>();
        pointsText = GameObject.Find("PointsToWin").GetComponent<Text>();
        startButton = GameObject.Find("StartGame").GetComponent<Button>();
        teamList = new List<Team>();
        pointsToWin = 5;
        pointsText.text = "Points to win: " + pointsToWin.ToString();

        GameObject.Find("Grade").GetComponent<Text>().text += "Grade: " + GameManager.grade + "th grade";
        GameObject.Find("Unit").GetComponent<Text>().text += "Unit: " + GameManager.unit;
    }

    void Update() {
        ButtonCheck();
    }

    public void AddNewTeam() {
        GameManager.teams.Add(new Team(inputField.text));
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
            case > 7:
                teamlistText.fontSize = 40;
                GameObject.Find("MinMaxText").GetComponent<Text>().text = "Max amount of teams reached! (8)";
                break;
            case > 6:
                teamlistText.fontSize = 40;
                break;
            case > 5:
                teamlistText.fontSize = 50;
                break;
            case > 4:
                teamlistText.fontSize = 60;
                break;
            case > 3:
                teamlistText.fontSize = 65;
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
        RefreshTeamListScreen();
    }

    public void ButtonCheck() {
        //Check the add team button: if there is no text in the inputfield, the button is non interactable
        addTeamButton.interactable = inputField.text.Length > 0 && GameManager.teams.Count < 8 ? true : false;
        startButton.interactable = GameManager.teams.Count >= 2 ? true : false;
        undoTeamButton.interactable = GameManager.teams.Count > 0 ? true : false;

        //Check the add/subtract points to win buttons to put a cap on the amount of points
        subtractPointsButton.interactable = pointsToWin == 3 ? false : true;
        addPointsButton.interactable = pointsToWin == 10 ? false : true;
    }

    public void StartGame() {
        GameManager.pointsToWin = this.pointsToWin;
        SceneManager.LoadScene("WheelScreen");
    }

    public void changePointAmount(bool add){ //where true is add and false is subtract
        if(add){
            pointsToWin++;
        }
        else{
            pointsToWin--;
        }
        pointsText.text = "Points to win: " + pointsToWin.ToString();
    }

    public void BackToMenu() {
        GameManager.teams.Clear();
        SceneManager.LoadScene("LevelPicker");
    }
}
