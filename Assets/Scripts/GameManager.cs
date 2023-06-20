using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static List<Team> teams = new List<Team>();
    public static int currentTeamID;
    public static int pointsToWin;
    public static bool winner;
    public static string unit;
    public static string grade;

    public void Start(){
        currentTeamID = 0;
        winner = false;
        TeamTurn();
    }

    public void AddPoint(){
        teams[currentTeamID].score++;
        Debug.Log("The team " + teams[currentTeamID].teamName + " has " + teams[currentTeamID].score + " points.");
        VerifyWin();
    }

    private void VerifyWin(){
        if(teams[currentTeamID].score == pointsToWin){
            Debug.Log("The team " + teams[currentTeamID].teamName + " has won");
            winner = true;
        }
    }

    private void DisplayTeams(){
        string scoreboard = "";
        foreach(var team in teams){
            scoreboard += team.teamName + " - " + team.score + "\n";
        }
        GameObject.Find("Groups").GetComponent<Text>().text = scoreboard;
    }

    private void TeamTurn(){
        DisplayTeams();
        string team = teams[currentTeamID].teamName;
        string itsTeamTurnMessage = "";

        itsTeamTurnMessage = team.ToLower().EndsWith("s") ? "It's " + team + "' turn" : "It's " + team + "'s turn";

        GameObject.Find("TeamTurn").GetComponent<Text>().text = itsTeamTurnMessage;
    }

    public void TurnChange(){
        currentTeamID = currentTeamID == teams.Count-1 ? 0 : currentTeamID + 1;
        TeamTurn();
    }

    public void ResetGame(){
        currentTeamID = 0;
        winner = false;
        teams.Clear();
    }
}
