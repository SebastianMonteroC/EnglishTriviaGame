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
    
    private string currentWheelPiece;

    //Play box displayed after spinning the wheel
    private GameObject playBox;
    private Text learningAbility;
    private Button playButton;

    //question box displayed after pressing "play"
    private GameObject questionBox;
    private Text questionAbility;
    private Text question;
    private Text timer;
    private float timerValue = 10;
    private bool timerOn;

    private void Start(){
        spinButtonUI.onClick.AddListener(Spin);
        playBox = GameObject.Find("ConfirmPlayBox");
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        learningAbility = GameObject.Find("Theme").GetComponent<Text>();
        
        questionBox = GameObject.Find("QuestionBox");
        questionAbility = GameObject.Find("Theme").GetComponent<Text>();
        timer = GameObject.Find("Timer").GetComponent<Text>();
        question = GameObject.Find("Question").GetComponent<Text>();
        
        GameObject.Find("ListeningImage").GetComponent<Image>().enabled = false;
        GameObject.Find("SpeakingImage").GetComponent<Image>().enabled = false;
        GameObject.Find("ReadingImage").GetComponent<Image>().enabled = false;
        GameObject.Find("WritingImage").GetComponent<Image>().enabled = false;

        playBox.SetActive(false);
        questionBox.SetActive(false);
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
    }

    private void Spin(){
        spinButtonUI.interactable = false;
        spinButtonText.text = "Spinning";

        wheel.OnSpinStart(() =>{
            Debug.Log("Spinning wheel!");
        });

        wheel.OnSpinEnd(wheelPiece => {
            Debug.Log(wheelPiece.Label);
            currentWheelPiece = wheelPiece.Label;
            ShowPlayScreen();
        });
        wheel.Spin();
    }

    private void ShowPlayScreen(){
        Debug.Log("Reaching here");
        playBox.SetActive(true);
        learningAbility.text = currentWheelPiece;
        GameObject.Find(currentWheelPiece + "Image").GetComponent<Image>().enabled = true;
    }

    public void BeginTurn(){
        GameObject.Find(currentWheelPiece + "Image").GetComponent<Image>().enabled = false;
        playBox.SetActive(false);
        questionBox.SetActive(true);
        timer.text = timerValue.ToString();
        StartCoroutine(RunTimer());
    }

    private void FormulateQuestion(){

    }

    private void ChangeTimerDisplay(Color color){
        timer.color = color;
        timer.text = timerValue.ToString();
    }
}

