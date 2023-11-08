using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    [SerializeField] private GameObject helpContainer;
    [SerializeField] private GameObject howToPlay;
    [SerializeField] private GameObject categoriesHelp;

    private bool helpActive = false;

    
    public void ToggleHelp() {
        SoundManager.Instance.PlaySFX("defaultButton");
        helpActive = !helpActive;
        helpContainer.SetActive(helpActive);
    }
    
    public void CloseHelp() {
        SoundManager.Instance.PlaySFX("backButton");
        helpActive = false;
        howToPlay.SetActive(true);
        categoriesHelp.SetActive(false);
        helpContainer.SetActive(false);
    }

    public void CategoriesHelp() {
        SoundManager.Instance.PlaySFX("openGrade");
        howToPlay.SetActive(false);
        categoriesHelp.SetActive(true);
    }

    public void HowToPlayHelp() {
        SoundManager.Instance.PlaySFX("closeGrade");
        howToPlay.SetActive(true);
        categoriesHelp.SetActive(false);
    }
}
