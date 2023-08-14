using System.Collections;
using System.Collections.Generic;
using System.IO;
using Random = System.Random;
using UnityEngine;


public class QuestionManager {
    public string assetsFolder = Application.streamingAssetsPath + "/Questions/";
    public string listeningAssetsFolder = Application.streamingAssetsPath + "/ListeningFiles/";
    public string customsFolder = Application.persistentDataPath + "/";
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
        wrongSpeakingQuestions = new List<Question>();
        wrongWritingQuestions = new List<Question>();
        wrongListeningQuestions = new List<Question>();
        SetFileNames(gradeNumber, unitNumber);
        LoadQuestions(assetsFolder);
    }
    
    public QuestionManager(string customQuestionBank) {
        wrongReadingQuestions = new List<Question>();
        wrongSpeakingQuestions = new List<Question>();
        wrongWritingQuestions = new List<Question>();
        wrongListeningQuestions = new List<Question>();
        SetFileNamesCustom(customQuestionBank);
        LoadQuestions(customsFolder);
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

    void SetFileNamesCustom(string customQuestionBank) {
        this.speakingFile = customQuestionBank + "_speaking.json";
        this.readingFile = customQuestionBank + "_reading.json";
        this.writingFile = customQuestionBank + "_writing.json";
        this.listeningFile = customQuestionBank + "_listening.json";
    }

    void LoadQuestions(string path) {
        this.readingQuestions = LoadFromJSON(path + this.readingFile);
        this.writingQuestions = LoadFromJSON(path + this.writingFile);
        this.speakingQuestions = LoadFromJSON(path + this.speakingFile);
        this.listeningQuestions = LoadFromJSON(path + this.listeningFile);
    }

    public List<Question> LoadFromJSON(string file) {
        string jsonContent = File.ReadAllText(file);
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
        Debug.Log("Getting question...");
        Random random = new Random();
        Question reading = new Question();
        int randomIndex;
        if (this.readingQuestions.Count > 0) {
            Debug.Log("count more than 1");
            randomIndex = random.Next(0, readingQuestions.Count);
            reading = this.readingQuestions[randomIndex];
            readingQuestions.Remove(reading);
        } else {
            randomIndex = random.Next(0, wrongReadingQuestions.Count);
            reading = this.wrongReadingQuestions[randomIndex];
        }
        return reading;
    }

     public Question GetWritingQuestion() {
        Random random = new Random();
        Question writing = new Question();
        int randomIndex;
        if (this.writingQuestions.Count > 0) {
            Debug.Log("count more than 1");
            randomIndex = random.Next(0, writingQuestions.Count);
            writing = this.writingQuestions[randomIndex];
            writingQuestions.Remove(writing);
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
            Debug.Log("count more than 1");
            randomIndex = random.Next(0, speakingQuestions.Count);
            speaking = this.speakingQuestions[randomIndex];
            speakingQuestions.Remove(speaking);
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
            randomIndex = random.Next(0, listeningQuestions.Count);
            listening = this.listeningQuestions[randomIndex];
            listeningQuestions.Remove(listening);
        } else {
            randomIndex = random.Next(0, wrongListeningQuestions.Count);
            listening = this.wrongListeningQuestions[randomIndex];
        }
        return listening;
    }

    public void ReadingIncorrectAnswer(Question question, string category) {
        switch (category) {
            case "Reading":
                if (!this.wrongReadingQuestions.Exists(item => item.question == question.question)) {
                    this.wrongReadingQuestions.Add(question);
                }
            break;
            case "Speaking":
                if (!this.wrongSpeakingQuestions.Exists(item => item.question == question.question)) {
                    this.wrongSpeakingQuestions.Add(question);
                }
            break;
            case "Writing":
                if (!this.wrongWritingQuestions.Exists(item => item.question == question.question)) {
                        this.wrongWritingQuestions.Add(question);
                }
            break;
            case "Listening":
                if (!this.wrongListeningQuestions.Exists(item => item.question == question.question)) {
                        this.wrongListeningQuestions.Add(question);
                }
            break;
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