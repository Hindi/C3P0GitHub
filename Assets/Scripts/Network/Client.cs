using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Client {
    public Client()
    {
        answers = new List<QuestionManager.AnswerKeeper>();
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

    public List<QuestionManager.AnswerKeeper> answers;
    public List<QuestionManager.AnswerKeeper> Answers
    {
        get { return answers; }
    }

    public QuestionManager.AnswerKeeper lastAnswer()
    {
        Debug.Log(answers.Count);
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
}
