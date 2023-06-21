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

    //Play box displayed after spinning the wheel
    private Text learningAbility;
    private Button playButton;

    //question box displayed after pressing "play"
    private Text questionAbility;
    private Text question;
    private Text timer;
    private float timerValue = 10f;
    private bool timerOn;

    //Question Loader
    private QuestionManager questionManager;
    Question currentQuestion;

    private void Start(){
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
        questionAbility = GameObject.Find("Theme").GetComponent<Text>();
        timer = GameObject.Find("Timer").GetComponent<Text>();
        question = GameObject.Find("Question").GetComponent<Text>();

        timeupContainer = GameObject.Find("TimeUpContainer");

        answerContainer = GameObject.Find("AnswerContainer");

        winnerContainer = GameObject.Find("WinnerContainer");

        timeupContainer.SetActive(false);
        questionContainer.SetActive(false);
        confirmPlayContainer.SetActive(false);
        answerContainer.SetActive(false);
        winnerContainer.SetActive(false);

        interactionBox.SetActive(false);

        questionManager = new QuestionManager(GameManager.grade, GameManager.unit);
    }

    private void Spin(){
        spinButtonUI.interactable = false;
        spinButtonText.text = "Spinning";

        wheel.OnSpinEnd(wheelPiece => {
            currentWheelPiece = wheelPiece.Label;
            Debug.Log(wheelPiece.Label);
            ShowPlayScreen();
        });
        wheel.Spin();
    }

    private void ShowPlayScreen(){
        interactionBox.SetActive(true);
        confirmPlayContainer.SetActive(true);
        learningAbility.text = currentWheelPiece;
        confirmPlayContainer.transform.Find(currentWheelPiece + "Image").gameObject.GetComponent<Image>().enabled = true;
    }

    public void BeginTurn(){
        this.currentQuestion = GetQuestion(currentWheelPiece);
        GameObject.Find(currentWheelPiece + "Image").GetComponent<Image>().enabled = false;
        confirmPlayContainer.SetActive(false);
        questionContainer.SetActive(true);
        GameObject.Find("QuestionTheme").GetComponent<Text>().text = currentWheelPiece;
        Text question = GameObject.Find("Question").GetComponent<Text>();
        question.resizeTextForBestFit = true;
        question.text = this.currentQuestion.question;
        timer.text = timerValue.ToString();
        StartCoroutine(RunTimer());
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
        while (timerValue > 0){
            yield return new WaitForSeconds(1f);
            timerValue --;
            if(timerValue < 7){
                color = Color.green;
            }
            if(timerValue < 4){
                color = Color.red;
            }
            ChangeTimerDisplay(color);
        }
        questionContainer.SetActive(false);
        timeupContainer.SetActive(true);
    }

    private void ChangeTimerDisplay(Color color){
        timer.color = color;
        timer.text = timerValue.ToString();
    }

    public void ShowAnswer(){
        timeupContainer.SetActive(false);
        answerContainer.SetActive(true);
        Text answer = GameObject.Find("Answer").GetComponent<Text>();
        answer.resizeTextForBestFit = true;
        answer.text = this.currentQuestion.answer;
        GameObject.Find("RightOrWrong").GetComponent<Text>().text = "Did " + GameManager.teams[GameManager.currentTeamID].teamName + " get it right?";
        timerValue = 10f;
    }

    public void VerifyAnswer(bool answer){
        if(answer){
            GameManagerObject.GetComponent<GameManager>().AddPoint();
        }
        else{
            Debug.Log("Wrong Answer! goofer!");
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
            Debug.Log("Winner winner chicken dinner");
        }
    }

    private void ShowWinnerScreen(){
        winnerContainer.SetActive(true);
        GameObject.Find("WinnerTitle").GetComponent<Text>().text = GameManager.teams[GameManager.currentTeamID].teamName + " wins!";
    }

    public void BackToMainMenu(){
        winnerContainer.SetActive(false);
        SceneManager.LoadScene("TeamPicker");
    }
}
