using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomQuestionMenu : MonoBehaviour
{
    public GameObject QuestionPrefab;
    [SerializeField] public GameObject NoCustoms;
    [SerializeField] public GameObject blocker;
    [SerializeField] public GameObject deleteConfirmationText;
    [SerializeField] public GameObject NewQuestionButton;

    private string nameToBeDeleted = "";
    private GameObject objectToBeDeleted;
    private int questionBankAmount = 0;

    void Update(){
        if(questionBankAmount >= 3) {
            NewQuestionButton.GetComponent<Button>().interactable = false;
        } else {
            NewQuestionButton.GetComponent<Button>().interactable = true;
        }
    }

    void Start()
    {
        if(PlayerPrefs.HasKey("custom1")){
            questionBankAmount++;
            NoCustoms.SetActive(false);
            InstantiateQuestionBank(1);
            if(PlayerPrefs.HasKey("custom2")){
                questionBankAmount++;
                InstantiateQuestionBank(2);
                if(PlayerPrefs.HasKey("custom3")){
                    questionBankAmount++;
                    InstantiateQuestionBank(3);
                }    
            }     
        }
    }

    private void InstantiateQuestionBank(int question_id) {
        GameObject childObject = Instantiate(QuestionPrefab) as GameObject;
        childObject.transform.SetParent(GameObject.Find("Panel").transform, false);
        childObject.transform.Find("QuestionBankName").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("custom" + question_id.ToString());
        childObject.transform.Find("Delete").gameObject.GetComponent<Button>().onClick.AddListener(delegate { RemoveQuestionBank("custom" + question_id.ToString(), childObject); });
        childObject.transform.Find("Edit").gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectCustomQuestionBank(PlayerPrefs.GetString("custom" + question_id.ToString())); });
    }

    public void RemoveQuestionBank(string name, GameObject deleted) {
        objectToBeDeleted = deleted;
        deleteConfirmationText.GetComponent<Text>().text =  "Are you sure you want to delete " + PlayerPrefs.GetString(name) + "?";
        nameToBeDeleted = name;
        DeletePress();
    }

    public void SelectCustomQuestionBank(string name){
        GameManager.grade = "";
        GameManager.unit = "";
        GameManager.customQuestionBank = name;
        SoundManager.Instance.PlaySFX("newGame");
        SceneManager.LoadScene("QuestionCreator");
    }

    public void NewQuestionBank() {
        SoundManager.Instance.PlaySFX("settings");
        SceneManager.LoadScene("QuestionCreator");
    }

    public void BackToMenu() {
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("MainMenu");
    }

    public void DeletePress() {
        SoundManager.Instance.PlaySFX("openGrade");
        blocker.SetActive(true);
    }

    public void BackDelete() {
        SoundManager.Instance.PlaySFX("backButton");
        blocker.SetActive(false);
    }

    public void DeleteEverything(){
        questionBankAmount--;
        string name = PlayerPrefs.GetString(nameToBeDeleted);
        if (Directory.Exists(Application.persistentDataPath)) {
            File.Delete(Application.persistentDataPath + "/" + name + "_reading.json");
            File.Delete(Application.persistentDataPath + "/" + name + "_listening.json");
            File.Delete(Application.persistentDataPath + "/" + name + "_speaking.json");
            File.Delete(Application.persistentDataPath + "/" + name + "_writing.json");
            Directory.Delete(Application.persistentDataPath + "/" + name + "_audios",true);
        }
        PlayerPrefs.DeleteKey(nameToBeDeleted);
        CheckDelete(nameToBeDeleted);
        nameToBeDeleted = "";
        Destroy(objectToBeDeleted);
        BackDelete();
        SoundManager.Instance.PlaySFX("closeGrade");
    }

    private void CheckDelete(string name){
        if(questionBankAmount == 0){
            NoCustoms.SetActive(true);
        }
    }
}