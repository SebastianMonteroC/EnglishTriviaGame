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

    public bool doublePointsActive = false;
    public bool triplePointsActive = false;
    public List<string> sabotages;
    public static bool sabotageActive = false;


    public void Start(){
        sabotages = new List<string>();
        currentTeamId = 0;
        winner = false;
        TeamTurn();
    }

    public void AddPoint(){
        if(doublePointsActive) {
            teams[currentTeamId].score += 2;
            doublePointsActive = false;
        } else if (triplePointsActive) {
            teams[currentTeamId].score += 3;
            triplePointsActive = false;
        } else {
            teams[currentTeamId].score++;
        }
        Debug.Log("The team " + teams[currentTeamId].teamName + " has " + teams[currentTeamId].score + " points.");
        VerifyWin();
    }

    public void RemovePoint() {
        teams[currentTeamId].score -= 1;
        sabotages.Remove(teams[currentTeamId].teamName);
        sabotageActive = false;
    }

    public void AddSabotage(string name) {
        sabotages.Add(name);
    }

    public void StealPoint(string name) {
        teams.Find(x => x.teamName == name).score -= 1;
        teams[currentTeamId].score += 1;
        DisplayTeams();
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
        if(currentTeamId == 0) {
            turnCounter++;
            if(turnCounter % 3 == 0) {
                GrantPowerUps();
            }
        }

        DisplayTeams();

        string team = teams[currentTeamId].teamName;
        string itsTeamTurnMessage = "";

        itsTeamTurnMessage = team.ToLower().EndsWith("s") ? "It's " + team + "' turn" : "It's " + team + "'s turn";

        if(sabotages.Contains(team)) {
            sabotageActive = true;
            sabotages.Remove(team);
        }

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
        Debug.Log("teams: " + teamPlacements.Count.ToString());
        int powerUpsGranted = 0;
        foreach(var team in GameManager.teams) {
            Debug.Log("Giving " + team.teamName + " power up");
            if(team.powerUps.Count < 3) {
                if(powerUpsGranted == GameManager.teams.Count-1) {
                    team.powerUps.Add(powerUpManager.GetHandicapPowerUp());
                } else {
                    team.powerUps.Add(powerUpManager.GetRegularPowerUp());
                }
            }
            powerUpsGranted++;
            Debug.Log(team.teamName + "power ups: " + team.powerUps);
        }
    }

    public void StartGamePowerUps() {
        foreach(var team in teams) {
            team.powerUps.Add(powerUpManager.GetRegularPowerUp());
        }
    }
}
