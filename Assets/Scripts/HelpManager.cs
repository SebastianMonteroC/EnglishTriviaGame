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
        helpActive = !helpActive;
        helpContainer.SetActive(helpActive);
    }
    
    public void CloseHelp() {
        helpActive = false;
        howToPlay.SetActive(true);
        categoriesHelp.SetActive(false);
        helpContainer.SetActive(false);
    }

    public void CategoriesHelp() {
        howToPlay.SetActive(false);
        categoriesHelp.SetActive(true);
    }

    public void HowToPlayHelp() {
        howToPlay.SetActive(true);
        categoriesHelp.SetActive(false);
    }
}
