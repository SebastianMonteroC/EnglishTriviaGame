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
using SimpleFileBrowser;

public class QuestionCreator : MonoBehaviour
{
    private bool isCategoryBoxOpen = false;

    public GameObject QuestionPrefab;
    private string currentCategory;
    private bool editing = false;

    [SerializeField] GameObject blocker;
    [SerializeField] GameObject AddQuestionBox;
    [SerializeField] GameObject QuestionBoxText;
    [SerializeField] GameObject QuestionInputField;
    [SerializeField] GameObject AnswerInputField;
    [SerializeField] GameObject InputBackButton;
    [SerializeField] GameObject ConfirmNewQuestionButton;
    [SerializeField] GameObject RightBox;
    [SerializeField] GameObject CategoryText;
    [SerializeField] GameObject QuestionBankName;
    [SerializeField] GameObject SaveQuestionBank;
    [SerializeField] GameObject UploadAudioFileButton;
    [SerializeField] GameObject SelectedFileText;
    [SerializeField] GameObject SavedChanges;
    [SerializeField] GameObject SavedText;
    [SerializeField] GameObject ConfirmExit;

    private bool FadeOut = false;
    private bool saved = true;
    private bool editingBank = false;
    private string editingName = "";

    private List<Question> ReadingQuestions;
    private List<Question> ListeningQuestions;
    private List<Question> SpeakingQuestions;
    private List<Question> WritingQuestions;

    private List<GameObject> DisplayedQuestions;
    private string SelectedAudioPath = "";

    private List<AudioPath> ListeningAudios;
    private int CustomQuestionBankCount;

    void Start() {
        DisplayedQuestions = new List<GameObject>();
        if(GameManager.customQuestionBank != "" && GameManager.customQuestionBank != null) {
            LoadQuestionBank();
            editingBank = true;
            editingName = GameManager.customQuestionBank;
        } else {
            ReadingQuestions = new List<Question>();
            ListeningQuestions = new List<Question>();
            SpeakingQuestions = new List<Question>();
            WritingQuestions = new List<Question>();
        }
    }

    void Update() {
        ButtonCheck();
        if(FadeOut) {
            Color objectColor = SavedChanges.GetComponent<Image>().color;
            Color textColor = SavedText.GetComponent<Text>().color;
            
            float fadeAmount = objectColor.a - (0.2f * Time.deltaTime);
            objectColor = new Color (objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            textColor = new Color (textColor.r, textColor.g, textColor.b, fadeAmount);

            SavedChanges.GetComponent<Image>().color = objectColor;
            SavedText.GetComponent<Text>().color = textColor;

            if(objectColor.a <= 0) {
                FadeOut = false;
                SavedChanges.SetActive(false);
                SavedChanges.GetComponent<Image>().color = new Color (objectColor.r, objectColor.g, objectColor.b, 120);
                SavedText.GetComponent<Text>().color = new Color (textColor.r, textColor.g, textColor.b, 150);
            }
        }
    }

    public void OpenAddNewQuestion() {
        AddQuestionBox.SetActive(true);
        SoundManager.Instance.PlaySFX("defaultButton");
        if(currentCategory == "Listening"){
            UploadAudioFileButton.SetActive(true);
        }

        if(!editing){
            QuestionBoxText.GetComponent<Text>().text = "New " + currentCategory.ToLower() + " question";
        } else {
            QuestionBoxText.GetComponent<Text>().text = "Edit " + currentCategory + " question";
            SelectedFileText.SetActive(true);
            SelectedFileText.GetComponent<Text>().text = "Selected File:\n" + Path.GetFileName(SelectedAudioPath);
        }
    
        blocker.SetActive(true);
    }

    public void CloseAddNewQuestion() {
        SoundManager.Instance.PlaySFX("closeGrade");
        UploadAudioFileButton.SetActive(false);
        AddQuestionBox.SetActive(false);
        blocker.SetActive(false);
        QuestionInputField.GetComponent<InputField>().text = "";
        AnswerInputField.GetComponent<InputField>().text = "";
        SelectedAudioPath = "";
        SelectedFileText.SetActive(true);
        SelectedFileText.GetComponent<Text>().text = "";
    }

    public void AddQuestion() {
        Question question = new Question();
        question.question = QuestionInputField.GetComponent<InputField>().text;
        question.answer = AnswerInputField.GetComponent<InputField>().text;
        question.path = SelectedAudioPath;
        switch (currentCategory)  {
            case "Listening":
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
        saved = false;
        SoundManager.Instance.PlaySFX("addTeam");
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
        if(currentCategory == "Listening") {
            ConfirmNewQuestionButton.GetComponent<Button>().interactable = SelectedAudioPath != "" ? true : false;
        }

        SaveQuestionBank.GetComponent<Button>().interactable = QuestionBankName.GetComponent<InputField>().text.Length > 0 ? true : false;

        if(editingBank == false) {
            switch(GameManager.questionBankSpace) {
                case "custom1":
                SaveQuestionBank.GetComponent<Button>().interactable =
                QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString("custom2")
                && QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString("custom3") ? true : false;
                break;
                case "custom2":
                SaveQuestionBank.GetComponent<Button>().interactable =
                QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString("custom1")
                && QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString("custom3") ? true : false;
                break;
                case "custom3":
                SaveQuestionBank.GetComponent<Button>().interactable =
                QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString("custom2")
                && QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString("custom1") ? true : false;
                break;
            }
            
        }
    }

    public void SelectFile() {
        FileBrowser.SetFilters( true, new FileBrowser.Filter("Audio Files", ".mp3"));
        FileBrowser.ShowLoadDialog(OnSuccess, null, FileBrowser.PickMode.Files, false, null, null, "Select an audio file for this question (.mp3)", "Select");
    }

    public void OnSuccess(string[] paths) {
        if(paths[0].Length != 0) {
            SelectedAudioPath = paths[0];
            SelectedFileText.SetActive(true);
            SelectedFileText.GetComponent<Text>().text = "Selected File:\n" + Path.GetFileName(SelectedAudioPath);
        }
        
    }
    public void EditQuestion(Question question, string category) {
        editing = true;
        QuestionInputField.GetComponent<InputField>().text = question.question;
        AnswerInputField.GetComponent<InputField>().text = question.answer;
        SelectedAudioPath = question.path;
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
        saved = false;
    }

    public void RemoveQuestion(Question question, string category) {
        switch (category)  {
            case "Listening":
                ListeningQuestions.Remove(question);
                File.Delete(question.path);
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
        saved = false;
    }

    public void SaveQuestions() {
        Directory.CreateDirectory(Application.persistentDataPath + "/" + QuestionBankName.GetComponent<InputField>().text + "_audios");

        //writing listening
        string listening_path = QuestionBankName.GetComponent<InputField>().text + "_listening.json";
        string listening_json = "{\n\t\"Question\": [ \n";
        foreach(var listening in ListeningQuestions) {
            string path = listening.path;
            if(!File.Exists(Application.persistentDataPath + "/" + QuestionBankName.GetComponent<InputField>().text + "_audios/" + Path.GetFileName(path))) {
                File.Copy(path, Application.persistentDataPath + "/" + QuestionBankName.GetComponent<InputField>().text + "_audios/" + Path.GetFileName(path));
                listening.path = Application.persistentDataPath + "/" + QuestionBankName.GetComponent<InputField>().text + "_audios/" + Path.GetFileName(path);
            }
            
            listening_json += 
            "\t\t{\n\t\t\t\"question\" :" + " \"" + listening.question + "\",\n" + "\t\t\t\"answer\" :" + " \"" + listening.answer + "\",\n" + "\t\t\t\"path\" :" + " \"" + listening.path + "\"\n" + "\t\t}";
            if(listening != ListeningQuestions.Last()){
                listening_json += ",\n";
            }
        }

        listening_json += "\n\t]\n}";
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + listening_path, listening_json);

        //writing writing
        string writing_path = QuestionBankName.GetComponent<InputField>().text + "_writing.json";
        string writing_json = "{\n\t\"Question\": [ \n";
        foreach(var writing in WritingQuestions) {
            writing_json += 
            "\t\t{\n\t\t\t\"question\" :" + " \"" + writing.question + "\",\n" + "\t\t\t\"answer\" :" + " \"" + writing.answer + "\",\n" + "\t\t\t\"path\" :" + " \"" + writing.path + "\"\n" + "\t\t}";
            if(writing != WritingQuestions.Last()){
                writing_json += ",\n";
            }
        }
        writing_json += "\n\t]\n}";
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + writing_path, writing_json);

        //writing speaking
        string speaking_path = QuestionBankName.GetComponent<InputField>().text + "_speaking.json";
        string speaking_json = "{\n\t\"Question\": [ \n";
        foreach(var speaking in SpeakingQuestions) {
            speaking_json += 
            "\t\t{\n\t\t\t\"question\" :" + " \"" + speaking.question + "\",\n" + "\t\t\t\"answer\" :" + " \"" + speaking.answer + "\",\n" + "\t\t\t\"path\" :" + " \"" + speaking.path + "\"\n" + "\t\t}";
            if(speaking != SpeakingQuestions.Last()){
                speaking_json += ",\n";
            }
        }
        speaking_json += "\n\t]\n}";
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + speaking_path, speaking_json);

        //writing reading
        string reading_path = QuestionBankName.GetComponent<InputField>().text + "_reading.json";
        string reading_json = "{\n\t\"Question\": [ \n";
        foreach(var reading in ReadingQuestions) {
            reading_json += 
            "\t\t{\n\t\t\t\"question\" :" + " \"" + reading.question + "\",\n" + "\t\t\t\"answer\" :" + " \"" + reading.answer + "\",\n" + "\t\t\t\"path\" :" + " \"" + reading.path + "\"\n" + "\t\t}";
            if(reading != ReadingQuestions.Last()){
                reading_json += ",\n";
            }
        }
        reading_json += "\n\t]\n}";
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + reading_path, reading_json);
        SoundManager.Instance.PlaySFX("addTeam");
        SavedChanges.SetActive(true);
        FadeOut = true;
        saved = true;
        if(!editingBank){
            if(!PlayerPrefs.HasKey("custom1") || QuestionBankName.GetComponent<InputField>().text == PlayerPrefs.GetString("custom1")){
                PlayerPrefs.SetString("custom1", QuestionBankName.GetComponent<InputField>().text);
            } else if(!PlayerPrefs.HasKey("custom2") || QuestionBankName.GetComponent<InputField>().text == PlayerPrefs.GetString("custom2")){
                PlayerPrefs.SetString("custom2", QuestionBankName.GetComponent<InputField>().text);
            } else if (!PlayerPrefs.HasKey("custom3") || QuestionBankName.GetComponent<InputField>().text == PlayerPrefs.GetString("custom3")){
                PlayerPrefs.SetString("custom3", QuestionBankName.GetComponent<InputField>().text);
            }
        } else {
            if(PlayerPrefs.HasKey("custom1") && PlayerPrefs.GetString("custom1") == editingName){
                PlayerPrefs.SetString("custom1", QuestionBankName.GetComponent<InputField>().text);
            } else if(PlayerPrefs.HasKey("custom2") && PlayerPrefs.GetString("custom1") == editingName){
                PlayerPrefs.SetString("custom2", QuestionBankName.GetComponent<InputField>().text);
            } else {
                PlayerPrefs.SetString("custom3", QuestionBankName.GetComponent<InputField>().text);
            }
        }
    }

    public void Exit() {
        Debug.Log(saved);
        if(QuestionBankName.GetComponent<InputField>().text != "" && !saved){
            if(!GameManager.editingBank) {
                string customX = "custom1";
                string customY = "custom2";
                switch (GameManager.questionBankSpace) {
                    case "custom1":
                        customX = "custom2";
                        customY = "custom3";
                    break;
                    case "custom2":
                        customX = "custom1";
                        customY = "custom3";
                    break;
                    case "custom3":
                        customX = "custom2";
                        customY = "custom1";
                    break;
                }
                if(QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString(customX)
                && QuestionBankName.GetComponent<InputField>().text != PlayerPrefs.GetString(customY)) {
                    SoundManager.Instance.PlaySFX("addTeam");
                    ConfirmExit.SetActive(true);
                } else {
                    MainMenu();
                }
            } else {
                SoundManager.Instance.PlaySFX("addTeam");
                ConfirmExit.SetActive(true);
            }
        } else {
            MainMenu();
        }
    }

    public void SaveAndExit(){
        SaveQuestions();
        MainMenu();
    }

    public void NameChanged(){
        saved = false;
    }

    public void CancelExit() {
        SoundManager.Instance.PlaySFX("closeGrade");
        ConfirmExit.SetActive(false);
    }

    public void MainMenu() {
        GameManager.editingBank = false;
        GameManager.customQuestionBank = "";
        SoundManager.Instance.PlaySFX("backButton");
        SceneManager.LoadScene("CustomBankMenu");
    }

    private void LoadQuestionBank(){
        QuestionBankName.GetComponent<InputField>().text = GameManager.customQuestionBank;
        ReadingQuestions = LoadFromJSON(Application.persistentDataPath + "/" +  GameManager.customQuestionBank + "_reading.json");
        ListeningQuestions = LoadFromJSON(Application.persistentDataPath + "/" +  GameManager.customQuestionBank + "_listening.json");
        SpeakingQuestions = LoadFromJSON(Application.persistentDataPath + "/" +  GameManager.customQuestionBank + "_speaking.json");
        WritingQuestions = LoadFromJSON(Application.persistentDataPath + "/" +  GameManager.customQuestionBank + "_writing.json");
    }

    public List<Question> LoadFromJSON(string file) {
        string jsonContent = File.ReadAllText(file);
        QuestionData questionData = JsonUtility.FromJson<QuestionData>(jsonContent);
        List<Question> questionList = new List<Question>();
        if (questionData != null && questionData.Question != null) {
            questionList = questionData.Question;
        } else {
            Debug.LogError("Failed to parse JSON data or no questions found!");
        }
        if (questionList.Count == 0) {
            Debug.LogError("No questions loaded.");
        }
        return questionList;
    }
}

public class AudioPath {
    string originalPath;
    string newPath;
    string filename;
}