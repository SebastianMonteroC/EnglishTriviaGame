using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionCreator : MonoBehaviour
{
    public GameObject QuestionPrefab;
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject AddQuestionBox;
    [SerializeField] GameObject QuestionInputField;
    [SerializeField] GameObject AnswerInputField;
    [SerializeField] GameObject ConfirmNewQuestionButton;
    [SerializeField] GameObject RightBox;
    [SerializeField] GameObject CategoryText;

    private List<Question> ReadingQuestions;
    private List<Question> ListeningQuestions;
    private List<Question> SpeakingQuestions;
    private List<Question> WritingQuestions;

    void Update() {
        ReadingQuestions = new List<Question>();
        ListeningQuestions = new List<Question>();
        SpeakingQuestions = new List<Question>();
        WritingQuestions = new List<Question>();
        ButtonCheck();
    }

    public void OpenAddNewQuestion() {
        AddQuestionBox.SetActive(true);
        blocker.SetActive(true);
    }

    public void CloseAddNewQuestion() {
        AddQuestionBox.SetActive(false);
        blocker.SetActive(false);
        QuestionInputField.GetComponent<InputField>().text = "";
        AnswerInputField.GetComponent<InputField>().text = "";
    }

    public void AddQuestion() {
        GameObject childObject = Instantiate(QuestionPrefab) as GameObject;
        childObject.transform.SetParent(GameObject.Find("Panel").transform, false);
        CloseAddNewQuestion();
    }

    private void ButtonCheck() {
        ConfirmNewQuestionButton.GetComponent<Button>().interactable = QuestionInputField.GetComponent<InputField>().text.Length > 0 && AnswerInputField.GetComponent<InputField>().text.Length > 0 ? true : false;
    }
}
