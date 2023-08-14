using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{
    public GameObject QuestionPrefab;

    void Start()
    {
        if(PlayerPrefs.HasKey("custom1")){
            GameObject childObject = Instantiate(QuestionPrefab) as GameObject;
            childObject.transform.SetParent(GameObject.Find("Panel").transform, false);
            childObject.transform.Find("QuestionBankName").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("custom1");
            childObject.GetComponent<Button>().onClick.AddListener(delegate { SelectCustomQuestionBank(PlayerPrefs.GetString("custom1")); });
            if(PlayerPrefs.HasKey("custom2")){
                GameObject childObject1 = Instantiate(QuestionPrefab) as GameObject;
                childObject1.transform.SetParent(GameObject.Find("Panel").transform, false);
                childObject1.transform.Find("QuestionBankName").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("custom2");
                childObject1.GetComponent<Button>().onClick.AddListener(delegate { SelectCustomQuestionBank(PlayerPrefs.GetString("custom2")); });
                if(PlayerPrefs.HasKey("custom3")){
                    GameObject childObject2 = Instantiate(QuestionPrefab) as GameObject;
                    childObject2.transform.SetParent(GameObject.Find("Panel").transform, false);
                    childObject2.transform.Find("QuestionBankName").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("custom3");
                    childObject2.GetComponent<Button>().onClick.AddListener(delegate { SelectCustomQuestionBank(PlayerPrefs.GetString("custom3")); });
                }    
            }     
        }
    }

    public void SelectCustomQuestionBank(string name){
        GameManager.grade = "";
        GameManager.unit = "";
        GameManager.customQuestionBank = name;
        SoundManager.Instance.PlaySFX("newGame");
        SceneManager.LoadScene("TeamPicker");
    }
    
    public void SelectMEPQuestions(){
        SoundManager.Instance.PlaySFX("newGame");
        SceneManager.LoadScene("LevelPicker");
    }

    public void BackToMenu() {
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("MainMenu");
    }
}
