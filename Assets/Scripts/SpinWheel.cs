using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{
    [SerializeField] private Button SpinButtonUI;
    [SerializeField] private Text SpinButtonText;
    [SerializeField] private PickerWheel Wheel;
    
    private string CurrentWheelPiece;

    //Play box displayed after spinning the wheel
    private GameObject PlayBox;
    private Text LearningAbility;
    private Button PlayButton;

    private void Start(){
        SpinButtonUI.onClick.AddListener(Spin);
        PlayBox = GameObject.Find("ConfirmPlayBox");

        LearningAbility = GameObject.Find("Theme").GetComponent<Text>();

        GameObject.Find("ListeningImage").GetComponent<Image>().enabled = false;;
        GameObject.Find("SpeakingImage").GetComponent<Image>().enabled = false;;
        GameObject.Find("ReadingImage").GetComponent<Image>().enabled = false;;
        GameObject.Find("WritingImage").GetComponent<Image>().enabled = false;;

        PlayButton = GameObject.Find("PlayButton").GetComponent<Button>();
        PlayBox.SetActive(false);
    }

    private void Spin(){
        SpinButtonUI.interactable = false;
        SpinButtonText.text = "Spinning";

        Wheel.OnSpinStart(() =>{
            Debug.Log("Spinning Wheel!");
        });

        Wheel.OnSpinEnd(wheelPiece => {
            SpinButtonUI.interactable = true;
            SpinButtonText.text = "Spin!";
            Debug.Log(wheelPiece.Label);
            CurrentWheelPiece = wheelPiece.Label;
            ShowPlayScreen();
        });
        Wheel.Spin();
    }

    private void ShowPlayScreen(){
        Debug.Log("Reaching here");
        PlayBox.SetActive(true);
        LearningAbility.text = CurrentWheelPiece;
        GameObject.Find(CurrentWheelPiece + "Image").GetComponent<Image>().enabled = true;
        
    }

    public void BeginTurn(){
        GameObject.Find(CurrentWheelPiece + "Image").GetComponent<Image>().enabled = false;
        PlayBox.SetActive(false);
    }
}
