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
    }

    void Update() {
        ButtonCheck();
    }

    public void AddNewTeam() {
        Text teamlistText = GameObject.Find("TempTeamText").GetComponent<Text>();
        GameManager.teams.Add(new Team(inputField.text));
        RefreshTeamListScreen();
        teamlistText.text += GameManager.teams.Count + ". " + inputField.text + "\n";
        inputField.text = "";
    }

    void RefreshTeamListScreen() {
        Text teamlistText = GameObject.Find("TempTeamText").GetComponent<Text>();
        GameObject.Find("TeamsAddedCount").GetComponent<Text>().text = "Teams added: " + GameManager.teams.Count;
        switch(GameManager.teams.Count) {
            case > 7:
                teamlistText.fontSize = 45;
                GameObject.Find("MinMaxText").GetComponent<Text>().text = "Max amount of teams reached! (8)";
                break;
            case > 6:
                teamlistText.fontSize = 55;
                break;
            case > 5:
                teamlistText.fontSize = 60;
                break;
            case > 4:
                teamlistText.fontSize = 70;
                break;
            case > 3:
                teamlistText.fontSize = 80;
                break;
            case > 2:
                teamlistText.fontSize = 90;
                break;
            case > 1:
                teamlistText.fontSize = 100;
                GameObject.Find("MinMaxText").GetComponent<Text>().text = "";
                break;
            case > 0:
                teamlistText.fontSize = 110;
                break;
        }
    }
    
    public void UndoLastAdded() {
        Text teamlistText = GameObject.Find("TempTeamText").GetComponent<Text>();
        RefreshTeamListScreen();

        teamlistText.text = "";
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
}
