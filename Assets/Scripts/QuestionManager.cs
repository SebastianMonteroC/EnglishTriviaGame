using System.Collections;
using System.Collections.Generic;
using System.IO;
using Random = System.Random;
using UnityEngine;


public class QuestionManager {
    public string assetsFolder = Application.streamingAssetsPath + "/Questions/";
    public string listeningAssetsFolder = Application.streamingAssetsPath + "/ListeningFiles/";
    //Speaking
    public string speakingFile = "speaking-{GRADE}-U{X}.json";
    public List<Question> speakingQuestions;
    public List<Question> wrongSpeakingQuestions;

    //Reading
    public string readingFile = "reading-{GRADE}-U{X}.json";
    public List<Question> readingQuestions;
    public List<Question> wrongReadingQuestions;

    //Writing
    public string writingFile = "writing-{GRADE}-U{X}.json";
    public List<Question> writingQuestions;
    public List<Question> wrongWritingQuestions;

    //Listening
    public string listeningFile = "listening-{GRADE}-U{X}.json";
    public List<Question> listeningQuestions;
    public List<Question> wrongListeningQuestions;

    public QuestionManager(string gradeNumber, string unitNumber) {
        wrongReadingQuestions = new List<Question>();
        SetFileNames(gradeNumber, unitNumber);
        LoadQuestions();
    }

    void SetFileNames(string gradeNumber, string unitNumber) {
        this.speakingFile = this.speakingFile.Replace("{GRADE}",gradeNumber);
        this.speakingFile = this.speakingFile.Replace("{X}",unitNumber);
        this.readingFile = this.readingFile.Replace("{GRADE}",gradeNumber);
        this.readingFile = this.readingFile.Replace("{X}",unitNumber);
        this.writingFile = this.writingFile.Replace("{GRADE}",gradeNumber);
        this.writingFile = this.writingFile.Replace("{X}",unitNumber);
        this.listeningFile = this.listeningFile.Replace("{GRADE}",gradeNumber);
        this.listeningFile = this.listeningFile.Replace("{X}",unitNumber);
    }

    void LoadQuestions() {
        this.readingQuestions = LoadFromJSON(this.readingFile);
        this.writingQuestions = LoadFromJSON(this.writingFile);
        this.speakingQuestions = LoadFromJSON(this.speakingFile);
        this.listeningQuestions = LoadFromJSON(this.listeningFile);
    }

    public List<Question> LoadFromJSON(string file) {
        string jsonContent = File.ReadAllText(assetsFolder + file);
        QuestionData questionData = JsonUtility.FromJson<QuestionData>(jsonContent);
        List<Question> questionList = new List<Question>();
        if (questionData != null && questionData.Question != null) {
            questionList = questionData.Question;
        } else {
            Debug.LogError("Failed to parse JSON data or no questions found!");
        }
        if (questionList.Count == 0) {
            Debug.LogError("No questions loaded.");
        }
        return questionList;
    }

    public Question GetReadingQuestion() {
        Random random = new Random();
        Question reading = new Question();
        int randomIndex;
        if (this.readingQuestions.Count > 0) {
            randomIndex = random.Next(0,readingQuestions.Count-1);
            reading = this.readingQuestions[randomIndex];
        } else {
            randomIndex = random.Next(0,wrongReadingQuestions.Count);
            reading = this.wrongReadingQuestions[randomIndex];
        }
        return reading;
    }

     public Question GetWritingQuestion() {
        Random random = new Random();
        Question writing = new Question();
        int randomIndex;
        if (this.writingQuestions.Count > 0) {
            randomIndex = random.Next(0, writingQuestions.Count-1);
            writing = this.writingQuestions[randomIndex];
        } else {
            randomIndex = random.Next(0, wrongWritingQuestions.Count);
            writing = this.wrongWritingQuestions[randomIndex];
        }
        return writing;
    }

    public Question GetSpeakingQuestion() {
        Random random = new Random();
        Question speaking = new Question();
        int randomIndex;
        if (this.speakingQuestions.Count > 0) {
            randomIndex = random.Next(0, speakingQuestions.Count-1);
            speaking = this.speakingQuestions[randomIndex];
        } else {
            randomIndex = random.Next(0, wrongSpeakingQuestions.Count);
            speaking = this.wrongSpeakingQuestions[randomIndex];
        }
        return speaking;
    }

    public Question GetListeningQuestion() {
        Random random = new Random();
        Question listening = new Question();
        int randomIndex;
        if (this.listeningQuestions.Count > 0) {
            randomIndex = random.Next(0, listeningQuestions.Count-1);
            listening = this.listeningQuestions[randomIndex];
        } else {
            randomIndex = random.Next(0, wrongListeningQuestions.Count);
            listening = this.wrongListeningQuestions[randomIndex];
        }
        return listening;
    }

    public void ReadingIncorrectAnswer(Question question) {
        if (!this.wrongReadingQuestions.Exists(item => item.question == question.question)) {
            this.wrongReadingQuestions.Add(question);
        }
    }
}

[System.Serializable]
public class QuestionData
{
    public List<Question> Question;
}

[System.Serializable]
public class Question
{
    public string question;
    public string answer;
    public string path;
}