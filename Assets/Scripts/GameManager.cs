using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static List<Team> teams = new List<Team>();
    public static PowerUp powerUpManager = new PowerUp();
    public static int currentTeamId;
    public static int pointsToWin;
    public static int time;
    public static bool timerEnabled;
    public static bool winner;
    public static string unit;
    public static string grade;
    private int turnCounter;

    public void Start(){
        currentTeamId = 0;
        winner = false;
        TeamTurn();
    }

    public void AddPoint(){
        teams[currentTeamId].score++;
        Debug.Log("The team " + teams[currentTeamId].teamName + " has " + teams[currentTeamId].score + " points.");
        VerifyWin();
    }

    public List<string> GetCurrentTeamsPowerUps() {
        return teams[currentTeamId].powerUps;
    }

    private void VerifyWin(){
        if(teams[currentTeamId].score == pointsToWin){
            Debug.Log("The team " + teams[currentTeamId].teamName + " has won!!!");
            winner = true;
        }
    }

    private void DisplayTeams(){
        int i = 0;
        foreach(var team in teams){
            GameObject.Find("TeamName" + i.ToString()).GetComponent<Text>().text = team.teamName;
            GameObject.Find("TeamScore" + i.ToString()).GetComponent<Text>().text = team.score.ToString();
            i++;
        }
    }

    private void TeamTurn(){
        DisplayTeams();

        string team = teams[currentTeamId].teamName;
        string itsTeamTurnMessage = "";

        itsTeamTurnMessage = team.ToLower().EndsWith("s") ? "It's " + team + "' turn" : "It's " + team + "'s turn";

        GameObject.Find("TeamTurn").GetComponent<Text>().text = itsTeamTurnMessage;
        GameObject.Find("TurnTitle").GetComponent<Text>().text = "Turn: " + turnCounter.ToString();
    }

    public void TurnChange(){
        currentTeamId = currentTeamId == teams.Count-1 ? 0 : currentTeamId + 1;
        TeamTurn();
    }

    public void ResetGame(){
        currentTeamId = 0;
        winner = false;
        teams.Clear();
    }

       public void StartingPowerUps() {
        foreach(var team in GameManager.teams) {
            team.powerUps.Add(powerUpManager.GetRegularPowerUp());
        }
    }

    private void GrantPowerUps() {
        List<Team> teamPlacements = GameManager.teams.OrderByDescending(o => o.score).ToList();
        int powerUpsGranted = 0;
        foreach(var team in GameManager.teams) {
            if(team.powerUps.Count < 3) {
                if(powerUpsGranted == GameManager.teams.Count-1) {
                    team.powerUps.Add(powerUpManager.GetHandicapPowerUp());
                } else {
                    team.powerUps.Add(powerUpManager.GetRegularPowerUp());
                }
            }
        }
    }

    public void StartGamePowerUps() {
        foreach(var team in teams) {
            team.powerUps.Add(powerUpManager.GetRegularPowerUp());
        }
    }
}
