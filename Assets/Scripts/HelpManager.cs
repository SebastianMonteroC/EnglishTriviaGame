using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    [SerializeField] private GameObject helpContainer;
    [SerializeField] private GameObject[] helpPages;
    [SerializeField] private GameObject nextPage;
    [SerializeField] private GameObject previousPage;

    private bool helpActive = false;
    private int currentPage = 0;

    private void ButtonUpdate() {
        if(currentPage == 0) {
            nextPage.SetActive(true);
            previousPage.SetActive(false);
        } else if (currentPage == 4) {
            nextPage.SetActive(false);
            previousPage.SetActive(true);
        } else {
            nextPage.SetActive(true);
            previousPage.SetActive(true);
        }
    }
    
    public void ToggleHelp() {
        SoundManager.Instance.PlaySFX("defaultButton");
        helpActive = !helpActive;
        helpContainer.SetActive(helpActive);
        ButtonUpdate();
    }
    
    public void CloseHelp() {
        currentPage = 0;
        SoundManager.Instance.PlaySFX("backButton");
        helpActive = false;
        foreach(var page in helpPages) {
            page.SetActive(false);
        }
        helpPages[0].SetActive(true);
        helpContainer.SetActive(false);
    }

    public void NextPage() {
        SoundManager.Instance.PlaySFX("openGrade");
        helpPages[currentPage].SetActive(false);
        currentPage++;
        helpPages[currentPage].SetActive(true);
        ButtonUpdate();
    }

    public void PreviousPage() {
        SoundManager.Instance.PlaySFX("closeGrade");
        helpPages[currentPage].SetActive(false);
        currentPage--;
        helpPages[currentPage].SetActive(true);
        ButtonUpdate();
    }
}
