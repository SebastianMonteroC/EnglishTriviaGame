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
    
    private GameObject GameManagerObject;

    private string currentWheelPiece;
    private GameObject interactionBox;

    //containers
    private GameObject questionContainer;
    private GameObject timeupContainer;
    private GameObject confirmPlayContainer;
    private GameObject answerContainer;
    private GameObject winnerContainer;
    private GameObject timerContainer;
    private GameObject listeningContainer;
    private GameObject playAudio;
    private GameObject stopAudio;

    //Play box displayed after spinning the wheel
    private Text learningAbility;
    private Button playButton;

    //question box displayed after pressing "play"
    private Text questionAbility;
    private Text question;
    private Text timer;
    private float timerValue;
    private bool timerOn;
    private GameObject noTimerAnswer;

    //Question Loader
    private QuestionManager questionManager;
    Question currentQuestion;

    private void Start(){
        timerValue = GameManager.time;
        GameManagerObject = GameObject.Find("GameManager");
        spinButtonUI.onClick.AddListener(Spin);

        interactionBox = GameObject.Find("InteractionBox");

        confirmPlayContainer = GameObject.Find("ConfirmPlayContainer");
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        learningAbility = GameObject.Find("Theme").GetComponent<Text>();
        
        GameObject.Find("ListeningImage").GetComponent<Image>().enabled = false;
        GameObject.Find("SpeakingImage").GetComponent<Image>().enabled = false;
        GameObject.Find("ReadingImage").GetComponent<Image>().enabled = false;
        GameObject.Find("WritingImage").GetComponent<Image>().enabled = false;

        questionContainer = GameObject.Find("QuestionContainer");
        listeningContainer = GameObject.Find("ListeningContainer");
        playAudio = GameObject.Find("PlayAudio");
        stopAudio = GameObject.Find("StopAudio");

        noTimerAnswer = GameObject.Find("NoTimerAnswer");
        noTimerAnswer.SetActive(false);
        questionAbility = GameObject.Find("Theme").GetComponent<Text>();
        timer = GameObject.Find("Timer").GetComponent<Text>();
        question = GameObject.Find("Question").GetComponent<Text>();
        timerContainer = GameObject.Find("TimeContainer");

        timeupContainer = GameObject.Find("TimeUpContainer");

        answerContainer = GameObject.Find("AnswerContainer");

        winnerContainer = GameObject.Find("WinnerContainer");

        timeupContainer.SetActive(false);
        questionContainer.SetActive(false);
        confirmPlayContainer.SetActive(false);
        answerContainer.SetActive(false);
        winnerContainer.SetActive(false);
        listeningContainer.SetActive(false);
        playAudio.SetActive(false);
        stopAudio.SetActive(false);

        interactionBox.SetActive(false);

        questionManager = new QuestionManager(GameManager.grade, GameManager.unit);
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
        listeningContainer.SetActive(true);
        GameObject.Find("QuestionTheme").GetComponent<Text>().text = currentWheelPiece;
        Text question = GameObject.Find("Question").GetComponent<Text>();
        question.resizeTextForBestFit = true;
        question.text = this.currentQuestion.question;
        timer.text = timerValue.ToString();
        
        if(currentWheelPiece == "Listening") {
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
            break;
            case "Writing":
                question = this.questionManager.GetWritingQuestion();
            break;
            case "Speaking":
                question = this.questionManager.GetSpeakingQuestion();
            break;
            case "Listening":
                question = this.questionManager.GetListeningQuestion();
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
    }

    public void StopAudio(){
        stopAudio.SetActive(false);
        playAudio.SetActive(true);
    }
}
