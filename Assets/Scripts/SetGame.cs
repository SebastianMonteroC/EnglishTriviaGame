using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetGame : MonoBehaviour
{
    private List<Team> teamList;
    public InputField inputField;
    public Button addButton;

    void Start()
    {
        inputField = GameObject.Find("TeamNameInput").GetComponent<InputField>();
        addButton = GameObject.Find("AddButton").GetComponent<Button>();
        addButton.interactable = false;
        teamList = new List<Team>();
    }

    void Update(){
        ButtonCheck();
    }

    public void AddNewTeam()
    {
        GameManager.teams.Add(new Team(inputField.text));
        GameObject.Find("TempTeamText").GetComponent<Text>().text += "\n" + inputField.text;
    }

    public void ButtonCheck(){
        if(inputField.text.Length > 0){
            addButton.interactable = true;
        }
        else{
            addButton.interactable = false;
        }
    }

    public void StartGame(){
        SceneManager.LoadScene("WheelScreen");
    }
}
