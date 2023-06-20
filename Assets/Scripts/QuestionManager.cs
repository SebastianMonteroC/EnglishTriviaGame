using System.Collections;
using System.Collections.Generic;
using System.IO;
using Random = System.Random;
using UnityEngine;

public class QuestionManager {
    public string assetsFolder = Application.dataPath + "/Questions/";
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
        SetFileNames(gradeNumber, unitNumber);
        LoadQuestions();
    }

    void SetFileNames(string gradeNumber, string unitNumber) {
        Debug.Log(gradeNumber + " " + unitNumber);
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
        List<Question> questions = JsonUtility.FromJson<List<Question>>(jsonContent);
        foreach(var q in questions)
        {
            Debug.Log(q.question);
        }
        return questions;
    }

    public Question GetReadingQuestion() {
        Random random = new Random();
        Question reading = new Question();
        int randomIndex;
        if (this.readingQuestions.Count > 0) {
            randomIndex = random.Next(readingQuestions.Count);
            reading = this.readingQuestions[randomIndex];
        } else {
            randomIndex = random.Next(wrongReadingQuestions.Count);
            reading = this.wrongReadingQuestions[randomIndex];
        }
        return reading;
    }

    public void ReadingIncorrectAnswer(Question question) {
        if (!this.wrongReadingQuestions.Exists(item => item.question == question.question)) {
            this.wrongReadingQuestions.Add(question);
        }
    }
}

[System.Serializable]
public class Question
{
    public string question;
}