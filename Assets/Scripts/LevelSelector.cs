using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private GameObject unitSelection;
    private bool isUnitSelectionActive = false;

    void Start() {
        unitSelection = GameObject.Find("UnitSelectionBox");
        unitSelection.SetActive(false);
    }

    public void BackToMenu() {
        GameManager.teams.Clear();
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleUnitSelection(int grade) {
        if(isUnitSelectionActive && int.Parse(GameManager.grade) != grade) {
            GameObject oldUnitArrow = GameObject.Find(GameManager.grade.ToString() + "thGradeArrow");
            oldUnitArrow.transform.eulerAngles = new Vector3(
                oldUnitArrow.transform.eulerAngles.x,
                oldUnitArrow.transform.eulerAngles.y,
                270
            );
        } else {
            isUnitSelectionActive = !isUnitSelectionActive;
        }
        
        GameManager.grade = grade.ToString();
        GameObject arrow = GameObject.Find(grade.ToString() + "thGradeArrow");
        if(isUnitSelectionActive) {
            unitSelection.SetActive(isUnitSelectionActive);
            int[] colors = GetColor(grade);
            SetUnitButtonColors(colors[0], colors[1], colors[2]);
            arrow.transform.eulerAngles = new Vector3(
                arrow.transform.eulerAngles.x,
                arrow.transform.eulerAngles.y,
                90
            );
        } else {
            arrow.transform.eulerAngles = new Vector3(
                arrow.transform.eulerAngles.x,
                arrow.transform.eulerAngles.y,
                270
            );
            GameManager.grade = "";
        }
        unitSelection.SetActive(isUnitSelectionActive);
    }

    private int[] GetColor (int grade) {
        switch(grade) {
            case 7:
                return new int[] {60, 132, 62};
            case 8:
                return new int[] {47, 141, 131};
            case 9:
                return new int[] {58, 77, 173};
            case 10:
                return new int[] {132, 60, 101};
            case 11:
                return new int[] {128, 39, 42};
        }
        return new int[] {255, 255, 255};
    }

    private void SetUnitButtonColors(int red, int green, int blue) {
        for(int i = 1; i < 7; i++ ) {
            GameObject.Find("Unit" + i + "Button").GetComponent<Image>().color = new Color32((byte)red, (byte)green, (byte)blue, 255);
        }
    }

    public void UnitSelected(int unit) {
        GameManager.unit = unit.ToString();
        SceneManager.LoadScene("TeamPicker");
    }
}
