using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionCreator : MonoBehaviour
{
    private bool isCategoryBoxOpen = false;

    public GameObject QuestionPrefab;
    private string currentCategory;
    private bool editing;

    [SerializeField] GameObject blocker;
    [SerializeField] GameObject AddQuestionBox;
    [SerializeField] GameObject QuestionBoxText;
    [SerializeField] GameObject QuestionInputField;
    [SerializeField] GameObject AnswerInputField;
    [SerializeField] GameObject InputBackButton;
    [SerializeField] GameObject ConfirmNewQuestionButton;
    [SerializeField] GameObject RightBox;
    [SerializeField] GameObject CategoryText;

    private List<Question> ReadingQuestions;
    private List<Question> ListeningQuestions;
    private List<Question> SpeakingQuestions;
    private List<Question> WritingQuestions;

    private List<GameObject> DisplayedQuestions;

    void Start() {
        ReadingQuestions = new List<Question>();
        ListeningQuestions = new List<Question>();
        SpeakingQuestions = new List<Question>();
        WritingQuestions = new List<Question>();
        DisplayedQuestions = new List<GameObject>();
    }

    void Update() {
        ButtonCheck();
    }

    public void OpenAddNewQuestion() {
        AddQuestionBox.SetActive(true);
        if(!editing){
            QuestionBoxText.GetComponent<Text>().text = "New " + currentCategory.ToLower() + " question";
        } else {
            QuestionBoxText.GetComponent<Text>().text = "Edit " + currentCategory + " question";
        }
    
        blocker.SetActive(true);
    }

    public void CloseAddNewQuestion() {
        AddQuestionBox.SetActive(false);
        blocker.SetActive(false);
        QuestionInputField.GetComponent<InputField>().text = "";
        AnswerInputField.GetComponent<InputField>().text = "";
    }

    public void AddQuestion() {
        Question question = new Question();
        question.question = QuestionInputField.GetComponent<InputField>().text;
        question.answer = AnswerInputField.GetComponent<InputField>().text;
        switch (currentCategory)  {
            case "Listening":
                //set the path to match the file thats gonna be saved
                this.ListeningQuestions.Add(question);
            break;
            case "Reading":
                this.ReadingQuestions.Add(question);
            break;
            case "Writing":
                this.WritingQuestions.Add(question);
            break;
            case "Speaking":
                this.SpeakingQuestions.Add(question);
            break; 
        }
        CloseAddNewQuestion();
        LoadCategoryQuestions();
        InputBackButton.SetActive(true);
        editing = false;
    }

    public void ToggleCategorySelection(string category) {
        if(isCategoryBoxOpen && category != currentCategory) {
            GameObject oldCategoryArrow = GameObject.Find(currentCategory + "Arrow");
            oldCategoryArrow.transform.eulerAngles = new Vector3(
                oldCategoryArrow.transform.eulerAngles.x,
                oldCategoryArrow.transform.eulerAngles.y,
                270
            );
        } else {
            isCategoryBoxOpen = !isCategoryBoxOpen;
        }
        
        CategoryText.GetComponent<Text>().text = category;
        currentCategory = category;
        GameObject arrow = GameObject.Find(category + "Arrow");
        if(isCategoryBoxOpen) {
            SoundManager.Instance.PlaySFX("openGrade");
            RightBox.SetActive(isCategoryBoxOpen);
            arrow.transform.eulerAngles = new Vector3(
                arrow.transform.eulerAngles.x,
                arrow.transform.eulerAngles.y,
                90
            );
        } else {
            SoundManager.Instance.PlaySFX("closeGrade");
            arrow.transform.eulerAngles = new Vector3(
                arrow.transform.eulerAngles.x,
                arrow.transform.eulerAngles.y,
                270
            );
            currentCategory = "";
           
        }
        RightBox.SetActive(isCategoryBoxOpen);
        if(RightBox.activeSelf) {
            LoadCategoryQuestions();
        }
    }

    private void LoadCategoryQuestions() {
        UnloadCategoryQuestions();
        List<Question> questions = new List<Question>();
        switch (currentCategory)  {
            case "Listening":
                questions = ListeningQuestions;
            break;
            case "Reading":
                questions = ReadingQuestions;
            break;
            case "Writing":
                questions = WritingQuestions;
            break;
            case "Speaking":
                questions = SpeakingQuestions;
            break; 
        }

        foreach(var question in questions) {
            string category = currentCategory;
            GameObject childObject = Instantiate(QuestionPrefab) as GameObject;
            childObject.transform.SetParent(GameObject.Find("Panel").transform, false);
            childObject.transform.Find("QuestionText").gameObject.GetComponent<Text>().text = question.question;
            childObject.transform.Find("Delete").gameObject.GetComponent<Button>().onClick.AddListener(delegate { RemoveQuestion(question, category); });
            childObject.transform.Find("Edit").gameObject.GetComponent<Button>().onClick.AddListener(delegate { EditQuestion(question, category); });
            DisplayedQuestions.Add(childObject);
        }
    }

    private void UnloadCategoryQuestions(){
        foreach(var question in DisplayedQuestions) {
            Destroy(question);
        }
    }

    private void ButtonCheck() {
        ConfirmNewQuestionButton.GetComponent<Button>().interactable = QuestionInputField.GetComponent<InputField>().text.Length > 0 && AnswerInputField.GetComponent<InputField>().text.Length > 0 ? true : false;
    }

    public void EditQuestion(Question question, string category) {
        editing = true;
        QuestionInputField.GetComponent<InputField>().text = question.question;
        AnswerInputField.GetComponent<InputField>().text = question.answer;
        OpenAddNewQuestion();
        InputBackButton.SetActive(false);
        switch (category)  {
            case "Listening":                
                ListeningQuestions.Remove(question);
            break;
            case "Reading":
                ReadingQuestions.Remove(question);
            break;
            case "Writing":
                WritingQuestions.Remove(question);
            break;
            case "Speaking":
                SpeakingQuestions.Remove(question);
            break;
        }
    }

    public void RemoveQuestion(Question question, string category) {
        switch (category)  {
            case "Listening":
                ListeningQuestions.Remove(question);
            break;
            case "Reading":
                ReadingQuestions.Remove(question);
            break;
            case "Writing":
                WritingQuestions.Remove(question);
            break;
            case "Speaking":
                SpeakingQuestions.Remove(question);
            break;
        }
        LoadCategoryQuestions();
    }

    public void MainMenu() {
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("MainMenu");
    }
}
