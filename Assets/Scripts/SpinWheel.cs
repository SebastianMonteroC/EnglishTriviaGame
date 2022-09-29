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

    private void Start(){
        SpinButtonUI.onClick.AddListener(Spin);
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
        });
        Wheel.Spin();
    }
}
