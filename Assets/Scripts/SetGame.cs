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
    public Button addPointsButton;
    public Button subtractPointsButton;

    void Start()
    {
        inputField = GameObject.Find("TeamNameInput").GetComponent<InputField>();
        addTeamButton = GameObject.Find("AddButton").GetComponent<Button>();
        addPointsButton = GameObject.Find("HigherPoints").GetComponent<Button>();
        subtractPointsButton = GameObject.Find("LowerPoints").GetComponent<Button>();
        pointsText = GameObject.Find("PointsToWin").GetComponent<Text>();
        addTeamButton.interactable = false;
        teamList = new List<Team>();
        pointsToWin = 5;
        pointsText.text = "Points to win: " + pointsToWin.ToString();
    }

    void Update(){
        ButtonCheck();
    }

    public void AddNewTeam()
    {
        GameManager.teams.Add(new Team(inputField.text));
        GameObject.Find("TempTeamText").GetComponent<Text>().text += "\n" + inputField.text;
        inputField.text = "";
    }

    public void ButtonCheck(){
        //Check the add team button: if there is no text in the inputfield, the button is non interactable
        addTeamButton.interactable = inputField.text.Length > 0 ? true : false;

        //Check the add/subtract points to win buttons to put a cap on the amount of points
        subtractPointsButton.interactable = pointsToWin == 3 ? false : true;
        addPointsButton.interactable = pointsToWin == 10 ? false : true;
    }

    public void StartGame(){
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
