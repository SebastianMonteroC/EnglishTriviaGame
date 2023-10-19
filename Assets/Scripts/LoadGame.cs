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
        
    }

    public void BackToMenu() {
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("MainMenu");
    }
}
