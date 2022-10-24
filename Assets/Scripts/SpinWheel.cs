using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //Play box displayed after spinning the wheel
    private Text learningAbility;
    private Button playButton;

    //question box displayed after pressing "play"
    private Text questionAbility;
    private Text question;
    private Text timer;
    private float timerValue = 10f;
    private bool timerOn;

    //Group and score 

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

        timeupContainer.SetActive(false);
        questionContainer.SetActive(false);
        confirmPlayContainer.SetActive(false);
        answerContainer.SetActive(false);

        interactionBox.SetActive(false);
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
        confirmPlayContainer.transform.Find(currentWheelPiece + "Image").gameObject.GetComponent<Image>().enabled = true;;
    }

    public void BeginTurn(){
        GameObject.Find(currentWheelPiece + "Image").GetComponent<Image>().enabled = false;
        confirmPlayContainer.SetActive(false);
        questionContainer.SetActive(true);
        GameObject.Find("QuestionTheme").GetComponent<Text>().text = currentWheelPiece;
        timer.text = timerValue.ToString();
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer(){
        Color color = Color.black;
        while (timerValue > 0){
            yield return new WaitForSeconds(1f);
            timerValue --;
            if(timerValue < 7){
                color = Color.yellow;
            }
            if(timerValue < 4){
                color = Color.red;
            }
            ChangeTimerDisplay(color);
        }
        questionContainer.SetActive(false);
        timeupContainer.SetActive(true);
    }

    private void FormulateQuestion(){

    }

    private void ChangeTimerDisplay(Color color){
        timer.color = color;
        timer.text = timerValue.ToString();
    }

    public void ShowAnswer(){
        timeupContainer.SetActive(false);
        answerContainer.SetActive(true);
        GameObject.Find("RightOrWrong").GetComponent<Text>().text = "Did " + GameManager.teams[GameManager.currentTeamID].teamName + " get it right?";
        timerValue = 10f;
    }

    public void VerifyAnswer(bool answer){
        if(answer){
            GameManagerObject.GetComponent<GameManager>().AddPoint();
        }
        else{
            Debug.Log("Wrong Answer! goofy mf");
        }
        GameManagerObject.GetComponent<GameManager>().TurnChange();
        answerContainer.SetActive(false);
        interactionBox.SetActive(false);
        spinButtonUI.interactable = true;
        spinButtonText.text = "Spin!";
    }
}

