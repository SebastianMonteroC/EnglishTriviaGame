using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomQuestionMenu : MonoBehaviour
{
    public GameObject QuestionPrefab;

    void Start()
    {
        if(PlayerPrefs.HasKey("custom1")){
            GameObject childObject = Instantiate(QuestionPrefab) as GameObject;
            childObject.transform.SetParent(GameObject.Find("Panel").transform, false);
            childObject.transform.Find("QuestionBankName").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("custom1");
            childObject.transform.Find("Delete").gameObject.GetComponent<Button>().onClick.AddListener(delegate { RemoveQuestionBank(PlayerPrefs.GetString("custom1")); });
            childObject.transform.Find("Edit").gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectCustomQuestionBank(PlayerPrefs.GetString("custom1")); });
            if(PlayerPrefs.HasKey("custom2")){
                GameObject childObject2 = Instantiate(QuestionPrefab) as GameObject;
                childObject2.transform.SetParent(GameObject.Find("Panel").transform, false);
                childObject2.transform.Find("QuestionBankName").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("custom2");
                childObject2.transform.Find("Delete").gameObject.GetComponent<Button>().onClick.AddListener(delegate { RemoveQuestionBank(PlayerPrefs.GetString("custom2")); });
                childObject2.transform.Find("Edit").gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectCustomQuestionBank(PlayerPrefs.GetString("custom2")); });
                if(PlayerPrefs.HasKey("custom3")){
                    GameObject childObject3 = Instantiate(QuestionPrefab) as GameObject;
                    childObject3.transform.SetParent(GameObject.Find("Panel").transform, false);
                    childObject3.transform.Find("QuestionBankName").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("custom3");
                    childObject3.transform.Find("Delete").gameObject.GetComponent<Button>().onClick.AddListener(delegate { RemoveQuestionBank(PlayerPrefs.GetString("custom3")); });
                    childObject3.transform.Find("Edit").gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectCustomQuestionBank(PlayerPrefs.GetString("custom3")); });
                }    
            }     
        }
    }

    public void RemoveQuestionBank(string name) {
        //prompt confirmation
    }

    public void SelectCustomQuestionBank(string name){
        GameManager.grade = "";
        GameManager.unit = "";
        GameManager.customQuestionBank = name;
        SoundManager.Instance.PlaySFX("newGame");
        SceneManager.LoadScene("TeamPicker");
    }

    public void NewQuestionBank() {
        SoundManager.Instance.PlaySFX("settings");
        SceneManager.LoadScene("QuestionCreator");
    }

    public void BackToMenu() {
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("MainMenu");
    }
}