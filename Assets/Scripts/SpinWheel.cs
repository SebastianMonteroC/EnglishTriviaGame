using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using System;

public class SpinWheel : MonoBehaviour
{
    [SerializeField] private Button spinButtonUI;
    [SerializeField] private Text spinButtonText;
    [SerializeField] private PickerWheel wheel;
    
    [SerializeField] private GameObject GameManagerObject;

    private string currentWheelPiece;
    [SerializeField] private GameObject interactionBox;
    [SerializeField] private GameObject powerUpInteractionBox;

    //Interaction Containers
    [SerializeField] private GameObject questionContainer;
    [SerializeField] private GameObject timeupContainer;
    [SerializeField] private GameObject confirmPlayContainer;
    [SerializeField] private GameObject answerContainer;
    [SerializeField] private GameObject winnerContainer;
    [SerializeField] private GameObject timerContainer;
    [SerializeField] private GameObject listeningContainer;
    [SerializeField] private GameObject playAudio;
    [SerializeField] private GameObject stopAudio;

    //Play box displayed after spinning the wheel
    [SerializeField] private Text learningAbility;
    [SerializeField] private Button playButton;

    //question box displayed after pressing "play"
    [SerializeField] private Text questionAbility;
    [SerializeField] private Text question;
    [SerializeField] private Text timer;
    [SerializeField] private GameObject noTimerAnswer;

    //images
    [SerializeField] private Image ListeningImage;
    [SerializeField] private Image SpeakingImage;
    [SerializeField] private Image ReadingImage;
    [SerializeField] private Image WritingImage;

    //timer
    private float timerValue;
    private bool timerOn;

    //Question Loader
    private QuestionManager questionManager;
    private Question currentQuestion;


    //power ups
    [SerializeField] private GameObject powerUpTable;
    [SerializeField] private GameObject powerUp1;
    [SerializeField] private GameObject powerUp2;
    [SerializeField] private GameObject powerUp3;
    [SerializeField] private GameObject NoPowerUpText;
    [SerializeField] private GameObject ChangeQuestionButton;
    [SerializeField] private GameObject RespinButton;
    [SerializeField] private GameObject SabotageContainer;
    [SerializeField] private GameObject SabotageButton1;
    [SerializeField] private GameObject SabotageTeamText1;
    [SerializeField] private GameObject SabotageButton2;
    [SerializeField] private GameObject SabotageTeamText2;
    [SerializeField] private GameObject SabotageButton3;
    [SerializeField] private GameObject SabotageTeamText3;
    private int currentSabotageSlotUsed;

    [SerializeField] private GameObject StealPointContainer;
    [SerializeField] private GameObject StealPointButton1;
    [SerializeField] private GameObject StealPointTeamText1;
    [SerializeField] private GameObject StealPointButton2;
    [SerializeField] private GameObject StealPointTeamText2;
    [SerializeField] private GameObject StealPointButton3;
    [SerializeField] private GameObject StealPointTeamText3;
    [SerializeField] private GameObject NoPoints;
    private int currentStealPointSlotUsed;

    [SerializeField] private GameObject DoublePoints1;
    [SerializeField] private GameObject TriplePoints1;
    [SerializeField] private GameObject StealPoint1;
    [SerializeField] private GameObject Sabotage1;
    [SerializeField] private GameObject Respin1;
    [SerializeField] private GameObject ChangeQuestion1;

    [SerializeField] private GameObject DoublePoints2;
    [SerializeField] private GameObject TriplePoints2;
    [SerializeField] private GameObject StealPoint2;
    [SerializeField] private GameObject Sabotage2;
    [SerializeField] private GameObject Respin2;
    [SerializeField] private GameObject ChangeQuestion2;

    [SerializeField] private GameObject DoublePoints3;
    [SerializeField] private GameObject TriplePoints3;
    [SerializeField] private GameObject StealPoint3;
    [SerializeField] private GameObject Sabotage3;
    [SerializeField] private GameObject Respin3;
    [SerializeField] private GameObject ChangeQuestion3;

    [SerializeField] private GameObject PowerUpText1;
    [SerializeField] private GameObject PowerUpText2;
    [SerializeField] private GameObject PowerUpText3;
    
    private List<GameObject> AllPowerUps;

    private void Start(){
        AllPowerUps = InsertPowerUps();
        timerValue = GameManager.time;
        spinButtonUI.onClick.AddListener(Spin);
                
        ListeningImage.enabled = false;
        SpeakingImage.enabled = false;
        ReadingImage.enabled = false;
        WritingImage.enabled = false;

        if(GameManager.grade != "" && GameManager.unit != "") {
            questionManager = new QuestionManager(GameManager.grade, GameManager.unit);
        } else {
            questionManager = new QuestionManager(GameManager.customQuestionBank);
        }
        
        LoadListeningAudios();
        GameManagerObject.GetComponent<GameManager>().StartGamePowerUps();
        LoadTeamPowerUps();
    }

    private void OutOfQuestions() {
        int index;
        var pickerWheel = GameObject.Find("PickerWheel").GetComponent<PickerWheel>();
        if(this.questionManager.readingQuestions.Count == 0 && this.questionManager.wrongReadingQuestions.Count == 0) {
            index = Array.FindIndex(pickerWheel.wheelPieces, x => x.Label == "Reading");
            pickerWheel.wheelPieces[index].Chance = 0;
        }
        if(this.questionManager.writingQuestions.Count == 0 && this.questionManager.wrongWritingQuestions.Count == 0) {
            index = Array.FindIndex(pickerWheel.wheelPieces, x => x.Label == "Writing");
            pickerWheel.wheelPieces[index].Chance = 0;
        }
        if(this.questionManager.speakingQuestions.Count == 0 && this.questionManager.wrongSpeakingQuestions.Count == 0) {
            index = Array.FindIndex(pickerWheel.wheelPieces, x => x.Label == "Speaking");
            pickerWheel.wheelPieces[index].Chance = 0;
        }
        if(this.questionManager.listeningQuestions.Count == 0 && this.questionManager.wrongListeningQuestions.Count == 0) {
            index = Array.FindIndex(pickerWheel.wheelPieces, x => x.Label == "Listening");
            pickerWheel.wheelPieces[index].Chance = 0;
        }
    }

    private void Spin() {
        InteractablePowerUps(false);
        OutOfQuestions();
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
        if(GameManager.teams[GameManager.currentTeamId].powerUps.Contains("Re-Spin")) {
            RespinButton.SetActive(true);
        }
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
        GameObject.Find("QuestionTheme").GetComponent<Text>().text = currentWheelPiece;
        Text question = GameObject.Find("Question").GetComponent<Text>();
        question.resizeTextForBestFit = true;
        question.text = this.currentQuestion.question;
        timer.text = timerValue.ToString();
        if(GameManager.teams[GameManager.currentTeamId].powerUps.Contains("Change Question")) {
            ChangeQuestionButton.SetActive(true);
        }
        
        if(currentWheelPiece == "Listening") {
            listeningContainer.SetActive(true);
            playAudio.SetActive(true);
            timerContainer.SetActive(false);
            noTimerAnswer.SetActive(true);
        } else {
            listeningContainer.SetActive(false);
            playAudio.SetActive(false);
            timerContainer.SetActive(true);
            noTimerAnswer.SetActive(false);
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
        ChangeTimerDisplay(color);
        SoundManager.Instance.PlaySFX("timerTick");
        while (timerValue > 0){
            yield return new WaitForSeconds(1f);
            timerValue --;
            if(timerValue < 5){
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
        StopAudio();
        questionContainer.SetActive(false);
        timeupContainer.SetActive (true);
    }

    public void ShowAnswer(){
        SoundManager.Instance.PlaySFX("defaultButton");
        timeupContainer.SetActive(false);
        answerContainer.SetActive(true);
        Text answer = GameObject.Find("Answer").GetComponent<Text>();
        answer.resizeTextForBestFit = true;
        answer.text = this.currentQuestion.answer;
        GameObject.Find("RightOrWrong").GetComponent<Text>().text = "Did " + GameManager.teams[GameManager.currentTeamId].teamName + " get it right?";
        timerValue = GameManager.time;
    }

    public void VerifyAnswer(bool answer){
        if(answer){
            SoundManager.Instance.PlaySFX("rightAnswer");
            GameManagerObject.GetComponent<GameManager>().AddPoint();
        }
        else{
            SoundManager.Instance.PlaySFX("wrongAnswer");
            this.questionManager.ReadingIncorrectAnswer(this.currentQuestion, currentWheelPiece);
            if(GameManager.sabotageActive) {
                GameManagerObject.GetComponent<GameManager>().RemovePoint();
            }
        }

        answerContainer.SetActive(false);
        if(!GameManager.winner){
            TeamTurnChange();
            LoadTeamPowerUps();
        }
        else{
            ShowWinnerScreen();
        }
        InteractablePowerUps(true);
    }

    private void TeamTurnChange() {
        GameManagerObject.GetComponent<GameManager>().TurnChange();
        interactionBox.SetActive(false);
        spinButtonUI.interactable = true;
        spinButtonText.text = "Spin!";
    }

    private void LoadTeamPowerUps() {
        ResetPowerUpBox();
        List<string> powerUps = GameManagerObject.GetComponent<GameManager>().GetCurrentTeamsPowerUps();
        if(powerUps.Count == 0) {
            NoPowerUpText.SetActive(true);
            NoPowerUpText.GetComponent<Text>().text = GameManager.teams[GameManager.currentTeamId].teamName + " has no power ups!";
            powerUpTable.SetActive(false);
        } else {
            NoPowerUpText.SetActive(false);
            powerUpTable.SetActive(true);
            UpdatePowerUpBox(powerUps);
        }
    }

    private void ShowWinnerScreen(){
        winnerContainer.SetActive(true);
        GameObject.Find("WinnerTitle").GetComponent<Text>().text = GameManager.teams[GameManager.currentTeamId].teamName + " wins!";
    }

    public void BackToMainMenu(){
        SoundManager.Instance.PlaySFX("backButton");
        winnerContainer.SetActive(false);
        SceneManager.LoadScene("TeamPicker");
    }

    public void PlayAudio(){
        stopAudio.SetActive(true);
        playAudio.SetActive(false);
        SoundManager.Instance.PlayListeningAudio(currentQuestion.path);
    }

    public void StopAudio(){
        stopAudio.SetActive(false);
        playAudio.SetActive(true);
        SoundManager.Instance.StopSFX();
    }

    public void LoadListeningAudios() {
        if(GameManager.grade != "" && GameManager.unit != ""){
            string fileBase = "listening-{GRADE}-U{X}-{#}";
            fileBase = fileBase.Replace("{GRADE}", GameManager.grade);
            fileBase = fileBase.Replace("{X}", GameManager.unit);
        } else {
            StartCoroutine(SoundManager.Instance.LoadCustomAudio(questionManager.listeningQuestions));
        }
        
    }

    public void DoublePoints(int slot) {
        GameManagerObject.GetComponent<GameManager>().doublePointsActive = true;
        GameManager.teams[GameManager.currentTeamId].powerUps.Remove("Double Points");
        GameObject.Find("DoublePoints" + slot.ToString()).SetActive(false);
        GameObject.Find("PowerUpText" + slot.ToString()).SetActive(false);
    }

    public void TriplePoints(int slot) {
        GameManagerObject.GetComponent<GameManager>().triplePointsActive = true;
        GameManager.teams[GameManager.currentTeamId].powerUps.Remove("Triple Points");
        GameObject.Find("TriplePoints" + slot.ToString()).SetActive(false);
        GameObject.Find("PowerUpText" + slot.ToString()).SetActive(false);
    }

    public void ChangeQuestion(int slot) {
        ChangeQuestionButton.SetActive(false);
        Question previousQuestion = this.currentQuestion;
        this.currentQuestion = GetQuestion(currentWheelPiece);
        switch (currentWheelPiece) {
            case "Listening":
                this.questionManager.listeningQuestions.Add(previousQuestion);
            break;
            case "Reading":
                this.questionManager.readingQuestions.Add(previousQuestion);
            break;
            case "Writing":
                this.questionManager.writingQuestions.Add(previousQuestion);
            break;
            case "Speaking":
                this.questionManager.speakingQuestions.Add(previousQuestion);
            break; 
        }
        Text question = GameObject.Find("Question").GetComponent<Text>();
        question.resizeTextForBestFit = true;
        question.text = this.currentQuestion.question;
        timerValue = GameManager.time;
        timer.text = timerValue.ToString();
    }

    public void RespinWheel() {
        interactionBox.SetActive(false);
        RespinButton.SetActive(false);
        confirmPlayContainer.SetActive(false);
        confirmPlayContainer.transform.Find(currentWheelPiece + "Image").gameObject.GetComponent<Image>().enabled = false;
        GameManager.teams[GameManager.currentTeamId].powerUps.Remove("Re-Spin");
        Spin();
    }

    private void ResetPowerUpBox() {
        foreach(var powerUpObject in AllPowerUps) {
            powerUpObject.SetActive(false);
        }
    }

    public void CloseSabotageContainer() {
        powerUpInteractionBox.SetActive(false);
        SabotageContainer.SetActive(false);
        SabotageButton1.SetActive(false);
        SabotageButton2.SetActive(false);
        SabotageButton3.SetActive(false);
        SabotageTeamText1.SetActive(false);
        SabotageTeamText2.SetActive(false);
        SabotageTeamText3.SetActive(false);
    }

    public void OpenSabotageContainer(int slot) {
        currentSabotageSlotUsed = slot;
        powerUpInteractionBox.SetActive(true);
        SabotageContainer.SetActive(true);
        int sabotageButton = 1;
        foreach(var team in GameManager.teams) {
            Debug.Log("Sabotage: " + team.teamName);
            if(team.teamName != GameManager.teams[GameManager.currentTeamId].teamName) {
                switch (sabotageButton) {
                    case 1:
                        SabotageButton1.SetActive(true);
                        SabotageTeamText1.SetActive(true);
                        SabotageTeamText1.GetComponent<Text>().text = team.teamName;
                    break;
                    case 2:
                        SabotageButton2.SetActive(true);
                        SabotageTeamText2.SetActive(true);
                        SabotageTeamText2.GetComponent<Text>().text = team.teamName;
                    break;
                    case 3:
                        SabotageButton3.SetActive(true);
                        SabotageTeamText3.SetActive(true);
                        SabotageTeamText3.GetComponent<Text>().text = team.teamName;
                    break;
                }
                sabotageButton++;
            }
        }
    }

    public void SabotageTeam(int id) {
        switch (id) {
            case 1:
                GameManagerObject.GetComponent<GameManager>().AddSabotage(SabotageTeamText1.GetComponent<Text>().text);
            break;
            case 2:
                GameManagerObject.GetComponent<GameManager>().AddSabotage(SabotageTeamText2.GetComponent<Text>().text);
            break;
            case 3:
                GameManagerObject.GetComponent<GameManager>().AddSabotage(SabotageTeamText3.GetComponent<Text>().text);
            break;
        }
        GameObject.Find("Sabotage" + currentSabotageSlotUsed).SetActive(false);
        GameObject.Find("PowerUpText" + currentSabotageSlotUsed).SetActive(false);
        currentSabotageSlotUsed = 0;
        GameManager.teams[GameManager.currentTeamId].powerUps.Remove("Sabotage");
        CloseSabotageContainer();
    }

    public void CloseStealPointContainer() {
        powerUpInteractionBox.SetActive(false);
        StealPointContainer.SetActive(false);
        StealPointButton1.SetActive(false);
        StealPointButton2.SetActive(false);
        StealPointButton3.SetActive(false);
        StealPointTeamText1.SetActive(false);
        StealPointTeamText2.SetActive(false);
        StealPointTeamText3.SetActive(false);
        NoPoints.SetActive(false);
    }

    public void OpenStealPointContainer(int slot) {
        currentStealPointSlotUsed = slot;
        powerUpInteractionBox.SetActive(true);
        StealPointContainer.SetActive(true);
        int StealPointButton = 1;
        bool noPoints = true;
        foreach(var team in GameManager.teams) {
            if(team.teamName != GameManager.teams[GameManager.currentTeamId].teamName && team.score > 0) {
                noPoints = true;
                switch (StealPointButton) {
                    case 1:
                        StealPointButton1.SetActive(true);
                        StealPointTeamText1.SetActive(true);
                        StealPointTeamText1.GetComponent<Text>().text = team.teamName;
                    break;
                    case 2:
                        StealPointButton2.SetActive(true);
                        StealPointTeamText2.SetActive(true);
                        StealPointTeamText2.GetComponent<Text>().text = team.teamName;
                    break;
                    case 3:
                        StealPointButton3.SetActive(true);
                        StealPointTeamText3.SetActive(true);
                        StealPointTeamText3.GetComponent<Text>().text = team.teamName;
                    break;
                }
                StealPointButton++;
            }
        }
        if(!noPoints) {
            NoPoints.SetActive(true);
        }
        
    }

    public void StealPoint(int id) {
        switch (id) {
            case 1:
                GameManagerObject.GetComponent<GameManager>().StealPoint(StealPointTeamText1.GetComponent<Text>().text);
            break;
            case 2:
                GameManagerObject.GetComponent<GameManager>().StealPoint(StealPointTeamText2.GetComponent<Text>().text);
            break;
            case 3:
                GameManagerObject.GetComponent<GameManager>().StealPoint(StealPointTeamText3.GetComponent<Text>().text);
            break;
        }
        GameObject.Find("StealPoint" + currentStealPointSlotUsed).SetActive(false);
        GameObject.Find("PowerUpText" + currentStealPointSlotUsed).SetActive(false);
        currentStealPointSlotUsed = 0;
        GameManager.teams[GameManager.currentTeamId].powerUps.Remove("Steal Point");
        CloseStealPointContainer();
    }

    private List<GameObject> InsertPowerUps() {
        List<GameObject> PowerUps = new List<GameObject>();
        PowerUps.AddRange(new GameObject[21] {
            DoublePoints1, TriplePoints1, StealPoint1, Sabotage1, Respin1, ChangeQuestion1,
            DoublePoints2, TriplePoints2, StealPoint2, Sabotage2, Respin2, ChangeQuestion2,
            DoublePoints3, TriplePoints3, StealPoint3, Sabotage3, Respin3, ChangeQuestion3,
            PowerUpText1, PowerUpText2, PowerUpText3
        });
        return PowerUps;
    }

    private void InteractablePowerUps(bool interactable) {
        Sabotage1.GetComponent<Button>().interactable = interactable;
        Sabotage2.GetComponent<Button>().interactable = interactable;
        Sabotage3.GetComponent<Button>().interactable = interactable;
        StealPoint1.GetComponent<Button>().interactable = interactable;
        StealPoint2.GetComponent<Button>().interactable = interactable;
        StealPoint3.GetComponent<Button>().interactable = interactable;
    }

    //refactor function to avoid code repeating / make it cleaner
    //another function that takes the name of the power up? this can be done all in one.
    private void UpdatePowerUpBox(List<string> powerUps) {
        int currentPowerUp = 1;
        foreach(var powerUp in powerUps) {
            Debug.Log("Powerup: " + powerUp);
            switch (powerUp) {
                case "Double Points":
                    if(currentPowerUp == 1) {
                        powerUp1.SetActive(true);
                        DoublePoints1.SetActive(true);
                        PowerUpText1.SetActive(true);
                        PowerUpText1.GetComponent<Text>().text = "Double Points";
                        currentPowerUp++;
                    } else {
                        if(currentPowerUp == 2) {
                            powerUp2.SetActive(true);
                            DoublePoints2.SetActive(true);
                            PowerUpText2.SetActive(true);
                            PowerUpText2.GetComponent<Text>().text = "Double Points";
                            currentPowerUp++;
                        } else {
                            DoublePoints3.SetActive(true);
                            powerUp3.SetActive(true);
                            DoublePoints3.SetActive(true);
                            PowerUpText3.SetActive(true);
                            PowerUpText3.GetComponent<Text>().text = "Double Points";
                            currentPowerUp++;
                        }
                    }
                break;
                case "Triple Points":
                    if(currentPowerUp == 1) {
                        powerUp1.SetActive(true);
                        TriplePoints1.SetActive(true);
                        PowerUpText1.SetActive(true);
                        PowerUpText1.GetComponent<Text>().text = "Triple Points";
                        currentPowerUp++;
                    } else {
                        if(currentPowerUp == 2) {
                            powerUp2.SetActive(true);
                            TriplePoints2.SetActive(true);
                            PowerUpText2.SetActive(true);
                            PowerUpText2.GetComponent<Text>().text = "Triple Points";
                            currentPowerUp++;
                        } else {
                            powerUp3.SetActive(true);
                            TriplePoints3.SetActive(true);
                            PowerUpText3.SetActive(true);
                            PowerUpText3.GetComponent<Text>().text = "Triple Points";
                            currentPowerUp++;
                        }
                    }
                break;
                case "Sabotage":
                    if(currentPowerUp == 1) {
                        powerUp1.SetActive(true);
                        Sabotage1.SetActive(true);
                        PowerUpText1.SetActive(true);
                        PowerUpText1.GetComponent<Text>().text = "Sabotage";
                        currentPowerUp++;
                    } else {
                        if(currentPowerUp == 2) {
                            powerUp2.SetActive(true);
                            Sabotage2.SetActive(true);
                            PowerUpText2.SetActive(true);
                            PowerUpText2.GetComponent<Text>().text = "Sabotage";
                            currentPowerUp++;
                        } else {
                            powerUp3.SetActive(true);
                            Sabotage3.SetActive(true);
                            PowerUpText3.SetActive(true);
                            PowerUpText3.GetComponent<Text>().text = "Sabotage";
                            currentPowerUp++;
                        }
                    }
                break;
                case "Steal Point":
                    if(currentPowerUp == 1) {
                        powerUp1.SetActive(true);
                        StealPoint1.SetActive(true);
                        PowerUpText1.SetActive(true);
                        PowerUpText1.GetComponent<Text>().text = "Steal Point";
                        currentPowerUp++;
                    } else {
                        if(currentPowerUp == 2) {
                            powerUp2.SetActive(true);
                            StealPoint2.SetActive(true);
                            PowerUpText2.SetActive(true);
                            PowerUpText2.GetComponent<Text>().text = "Steal Point";
                            currentPowerUp++;
                        } else {
                            powerUp3.SetActive(true);
                            StealPoint3.SetActive(true);
                            PowerUpText3.SetActive(true);
                            PowerUpText3.GetComponent<Text>().text = "Steal Point";
                            currentPowerUp++;
                        }
                    }
                break;
                case "Change Question":
                    if(currentPowerUp == 1) {
                        powerUp1.SetActive(true);
                        ChangeQuestion1.SetActive(true);
                        PowerUpText1.SetActive(true);
                        PowerUpText1.GetComponent<Text>().text = "Change Question";
                        currentPowerUp++;
                    } else {
                        if(currentPowerUp == 2) {
                            powerUp2.SetActive(true);
                            ChangeQuestion2.SetActive(true);
                            PowerUpText2.SetActive(true);
                            PowerUpText2.GetComponent<Text>().text = "Change Question";
                            currentPowerUp++;
                        } else {
                            powerUp3.SetActive(true);
                            ChangeQuestion3.SetActive(true);
                            PowerUpText3.SetActive(true);
                            PowerUpText3.GetComponent<Text>().text = "Change Question";
                            currentPowerUp++;
                        }
                    }
                break;
                case "Re-Spin":
                    if(currentPowerUp == 1) {
                        powerUp1.SetActive(true);
                        Respin1.SetActive(true);
                        PowerUpText1.SetActive(true);
                        PowerUpText1.GetComponent<Text>().text = "Re-Spin";
                        currentPowerUp++;
                    } else {
                        if(currentPowerUp == 2) {
                            powerUp2.SetActive(true);
                            Respin2.SetActive(true);
                            PowerUpText2.SetActive(true);
                            PowerUpText2.GetComponent<Text>().text = "Re-Spin";
                            currentPowerUp++;
                        } else {
                            powerUp3.SetActive(true);
                            Respin3.SetActive(true);
                            PowerUpText3.SetActive(true);
                            PowerUpText3.GetComponent<Text>().text = "Re-Spin";
                            currentPowerUp++;
                        }
                    }
                break;
        }
        }
        if(GameManager.teams[GameManager.currentTeamId].powerUps.Count > 0) {
            powerUp1.SetActive(true);
            powerUp2.SetActive(true);
            powerUp3.SetActive(true);
        }
    }
}