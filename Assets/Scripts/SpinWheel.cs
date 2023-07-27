using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
    [SerializeField] private Button spinButtonUI;
    [SerializeField] private Text spinButtonText;
    [SerializeField] private PickerWheel wheel;
    
    [SerializeField] private GameObject GameManagerObject;

    private string currentWheelPiece;
    [SerializeField] private GameObject interactionBox;

    //containers
    
    [SerializeField] private GameObject questionContainer;
    [SerializeField] private GameObject timeupContainer;
    [SerializeField] private GameObject confirmPlayContainer;
    [SerializeField] private GameObject answerContainer;
    [SerializeField] private GameObject winnerContainer;
    [SerializeField] private GameObject timerContainer;
    [SerializeField] private GameObject listeningContainer;
    [SerializeField] private GameObject playAudio;
    [SerializeField] private GameObject stopAudio;

    //Play box displayed after spinning the wheel
    [SerializeField] private Text learningAbility;
    [SerializeField] private Button playButton;

    //question box displayed after pressing "play"
    [SerializeField] private Text questionAbility;
    [SerializeField] private Text question;
    [SerializeField] private Text timer;
    [SerializeField] private GameObject noTimerAnswer;

    [SerializeField] private Image ListeningImage;
    [SerializeField] private Image SpeakingImage;
    [SerializeField] private Image ReadingImage;
    [SerializeField] private Image WritingImage;

    private float timerValue;
    private bool timerOn;

    //Question Loader
    private QuestionManager questionManager;
    Question currentQuestion;

    private void Start(){
        timerValue = GameManager.time;
        spinButtonUI.onClick.AddListener(Spin);
        
        ListeningImage.enabled = false;
        SpeakingImage.enabled = false;
        ReadingImage.enabled = false;
        WritingImage.enabled = false;

        questionManager = new QuestionManager(GameManager.grade, GameManager.unit);
        LoadListeningAudios();
    }

    private void Spin(){
        SoundManager.Instance.PlaySFX("wheelButton");
        spinButtonUI.interactable = false;
        spinButtonText.text = "Spinning";

        wheel.OnSpinEnd(wheelPiece => {
            currentWheelPiece = wheelPiece.Label;
            ShowPlayScreen();
        });
        wheel.Spin();
    }

    private void ShowPlayScreen(){
        interactionBox.SetActive(true);
        confirmPlayContainer.SetActive(true);
        SoundManager.Instance.PlaySFX("categoryChosen");
        learningAbility.text = currentWheelPiece;
        confirmPlayContainer.transform.Find(currentWheelPiece + "Image").gameObject.GetComponent<Image>().enabled = true;
    }

    public void BeginTurn(){
        SoundManager.Instance.PlaySFX("defaultButton");
        this.currentQuestion = GetQuestion(currentWheelPiece);
        GameObject.Find(currentWheelPiece + "Image").GetComponent<Image>().enabled = false;
        confirmPlayContainer.SetActive(false);
        questionContainer.SetActive(true);
        GameObject.Find("QuestionTheme").GetComponent<Text>().text = currentWheelPiece;
        Text question = GameObject.Find("Question").GetComponent<Text>();
        question.resizeTextForBestFit = true;
        question.text = this.currentQuestion.question;
        timer.text = timerValue.ToString();
        
        if(currentWheelPiece == "Listening") {
            listeningContainer.SetActive(true);
            playAudio.SetActive(true);
            timerContainer.SetActive(false);
            noTimerAnswer.SetActive(true);
        }

        if(GameManager.timerEnabled && currentWheelPiece != "Listening") {
            timerContainer.SetActive(true);
            noTimerAnswer.SetActive(false);
            StartCoroutine(RunTimer());
        } else {
            timerContainer.SetActive(false);
            noTimerAnswer.SetActive(true);
        }
    }

    Question GetQuestion(string category) {
        Question question = new Question();
        switch(category) {
            case "Reading":
                question = this.questionManager.GetReadingQuestion();
                Debug.Log("path is: " + question.path);
            break;
            case "Writing":
                question = this.questionManager.GetWritingQuestion();
                Debug.Log("path is: " + question.path);
            break;
            case "Speaking":
                question = this.questionManager.GetSpeakingQuestion();
                Debug.Log("path is: " + question.path);
            break;
            case "Listening":
                question = this.questionManager.GetListeningQuestion();
                Debug.Log("path is: " + question.path);
            break;
        }
        return question;
    }

    private IEnumerator RunTimer(){
        Color color = Color.black;
        SoundManager.Instance.PlaySFX("timerTick");
        while (timerValue > 0){
            yield return new WaitForSeconds(1f);
            timerValue --;
            if(timerValue < (GameManager.time / 100 * 70)){
                color = Color.green;
            }
            if(timerValue < (GameManager.time / 100 * 30)){
                color = Color.red;
            }
            ChangeTimerDisplay(color);
        }
        SoundManager.Instance.StopSFX();
        SoundManager.Instance.PlaySFX("timesUp");
        questionContainer.SetActive(false);
        timeupContainer.SetActive(true);
    }

    private void ChangeTimerDisplay(Color color){
        timer.color = color;
        timer.text = timerValue.ToString();
    }

    public void ProceedToAnswer(){
        questionContainer.SetActive(false);
        timeupContainer.SetActive(true);
    }

    public void ShowAnswer(){
        SoundManager.Instance.PlaySFX("defaultButton");
        timeupContainer.SetActive(false);
        answerContainer.SetActive(true);
        Text answer = GameObject.Find("Answer").GetComponent<Text>();
        answer.resizeTextForBestFit = true;
        answer.text = this.currentQuestion.answer;
        GameObject.Find("RightOrWrong").GetComponent<Text>().text = "Did " + GameManager.teams[GameManager.currentTeamID].teamName + " get it right?";
        timerValue = GameManager.time;
    }

    public void VerifyAnswer(bool answer){
        if(answer){
            SoundManager.Instance.PlaySFX("rightAnswer");
            GameManagerObject.GetComponent<GameManager>().AddPoint();
        }
        else{
            SoundManager.Instance.PlaySFX("wrongAnswer");
            this.questionManager.ReadingIncorrectAnswer(this.currentQuestion);
        }

        answerContainer.SetActive(false);
        if(!GameManager.winner){
            GameManagerObject.GetComponent<GameManager>().TurnChange();
            interactionBox.SetActive(false);
            spinButtonUI.interactable = true;
            spinButtonText.text = "Spin!";
        }
        else{
            ShowWinnerScreen();
        }
    }

    private void ShowWinnerScreen(){
        winnerContainer.SetActive(true);
        GameObject.Find("WinnerTitle").GetComponent<Text>().text = GameManager.teams[GameManager.currentTeamID].teamName + " wins!";
    }

    public void BackToMainMenu(){
        SoundManager.Instance.PlaySFX("backButton");
        winnerContainer.SetActive(false);
        SceneManager.LoadScene("TeamPicker");
    }

    public void PlayAudio(){
        stopAudio.SetActive(true);
        playAudio.SetActive(false);
        SoundManager.Instance.PlayListeningAudio(currentQuestion.path);
    }

    public void StopAudio(){
        stopAudio.SetActive(false);
        playAudio.SetActive(true);
        SoundManager.Instance.StopSFX();
    }

    public void LoadListeningAudios() {
        string fileBase = "listening-{GRADE}-U{X}-{#}";
        fileBase = fileBase.Replace("{GRADE}", GameManager.grade);
        fileBase = fileBase.Replace("{X}", GameManager.unit);
        for(int i = 0; i < questionManager.listeningQuestions.Count; i++) {
            StartCoroutine(SoundManager.Instance.LoadAudio(fileBase.Replace("{#}", i.ToString())));
        }
    }
}
