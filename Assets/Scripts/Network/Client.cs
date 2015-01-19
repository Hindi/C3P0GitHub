using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;

[XmlType("Answer")]
public class Answer
{
    public Answer()
    {

    }

    public Answer(Answer a)
    {
        questionId = a.questionId;
        response = a.response;
        result = a.result;
        answerTime = a.answerTime;
    }

    public Answer(int id, int rep, bool res, float time)
    {
        questionId = id;
        response = rep;
        result = res;
        answerTime = time;
    }

    [XmlAttribute]
    public int questionId;
    [XmlAttribute]
    public int response;
    [XmlAttribute]
    public bool result;
    [XmlAttribute]
    public float answerTime;
}

public class Client
{
    private int score;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    private string login;
    public string Login
    {
        get { return login; }
        set { login = value; }
    }

    private string id;
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    private NetworkPlayer networkPlayer;
    public NetworkPlayer NetworkPlayer
    {
        get { return networkPlayer; }
        set { networkPlayer = value; }
    }

    private bool answeredLast;
    public bool AnsweredLast
    {
        get { return answeredLast; }
        set { answeredLast = value; }
    }

    public List<QuestionManager.AnswerKeeper> answers;
    public List<QuestionManager.AnswerKeeper> Answers
    {
        get { return answers; }
    }

    public Client()
    {
        score = 0;
        answers = new List<QuestionManager.AnswerKeeper>();
        answeredLast = false;
    }

    public Client(Client c)
    {
        answeredLast = c.AnsweredLast;
        answers = c.Answers;
    }

    public QuestionManager.AnswerKeeper lastAnswer()
    {
        return answers[answers.Count - 1];
    }

    public void addAnswer(QuestionManager.AnswerKeeper a)
    {
        if (answers.Exists(x => x.question.id == a.question.id))
        {
            answers.Remove(answers.Find(x => x.question.id == a.question.id));
        }
        answers.Add(a);
        if(a.result)
            score++;
    }

    public string lastAnswerExplication()
    {
        return lastAnswer().question.explication;
    }

    public bool lastQuestionResult()
    {
        return lastAnswer().result;
    }

    public bool questionResult(QuestionManager.QuestionKeeper q)
    {
        foreach(QuestionManager.AnswerKeeper a in answers)
        {
            if (a.question.question == q.question)
                return a.result;
        }
        return true;
    }

    public void saveStats(int currentCourseId)
    {
        List<Answer> stats = new List<Answer>();

        foreach(QuestionManager.AnswerKeeper a in answers)
        {
            stats.Add(new Answer(a.question.id, a.rep, a.result, a.answerTime));
        }

        XmlHelpers.SaveToXML<List<Answer>>("Assets/Resources/Xml/answers/" + currentCourseId + "/" + login + ".xml", stats);
    }

    public void loadStats(int currentCourseId)
    {
        try
        {
            score = 0;
            TextAsset statsFile = (TextAsset)UnityEngine.Resources.Load("xml/answers/" + currentCourseId + "/" + login);
            List<Answer> stats = XmlHelpers.LoadFromTextAsset<Answer>(statsFile, "ArrayOfAnswer");
            QuestionManager.AnswerKeeper answerKeeper;
            foreach (Answer a in stats)
            {
                answerKeeper = new QuestionManager.AnswerKeeper();
                answerKeeper.question = new QuestionManager.QuestionKeeper();
                answerKeeper.answerTime = a.answerTime;
                answerKeeper.rep = a.response;
                answerKeeper.result = a.result;
                answerKeeper.question.id = a.questionId;

                Debug.Log(a.questionId + " "  + a.result);

                if (a.result)
                    score++;
                answers.Add(answerKeeper);
            }
            C3PONetworkManager.Instance.setScore(networkPlayer, score);
        }
        catch
        {

        }
    }
}
