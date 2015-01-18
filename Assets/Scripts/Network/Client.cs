using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;

[XmlType("Answer")]
public class Answer
{
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

[XmlType("AnswerStats")]
public class AnswerStats
{
    public AnswerStats()
    {
        reponses = new List<Answer>();
    }

    [XmlAttribute]
    public int courseId;
    [XmlArrayItem("ar")]
    public List<Answer> reponses;
}

public class Client
{
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
        AnswerStats stats = new AnswerStats();
        stats.courseId = currentCourseId;

        foreach(QuestionManager.AnswerKeeper a in answers)
        {
            stats.reponses.Add(new Answer(a.question.id, a.rep, a.result, a.answerTime));
        }

        XmlHelpers.SaveToXML<AnswerStats>("Assets/Resources/xml/answers/" + currentCourseId + "/" + login, stats);
    }
}
