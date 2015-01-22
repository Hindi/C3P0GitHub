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

[XmlType("GameStat")]
public class GameStat
{
    public GameStat()
    {

    }

    public GameStat(GameStat g)
    {
        gameId = g.gameId;
        paramId = g.paramId;
        score = g.score;
        nbDefeats = g.nbDefeats;
        nbVictories = g.nbVictories;
    }

    public GameStat(int id, int paramId, int sco, int vct, int def)
    {
        gameId = id;
        this.paramId = paramId;
        score = sco;
        nbDefeats = def;
        nbVictories = vct;
    }

    [XmlAttribute]
    public int gameId;
    [XmlAttribute]
    public int paramId;
    [XmlAttribute]
    public int score;
    [XmlAttribute]
    public int nbVictories;
    [XmlAttribute]
    public int nbDefeats;
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

    private List<GameStat> gameStats;
    public List<GameStat> GameStats
    {
        get { return gameStats; }
    }

    private List<QuestionManager.AnswerKeeper> answers;
    public List<QuestionManager.AnswerKeeper> Answers
    {
        get { return answers; }
    }

    public Client()
    {
        score = 0;
        answers = new List<QuestionManager.AnswerKeeper>();
        gameStats = new List<GameStat>();
        answeredLast = false;
    }

    public Client(Client c)
    {
        answeredLast = c.AnsweredLast;
        answers = c.Answers;
    }

    public void clearStats()
    {
        score = 0;
        answers.Clear();
        gameStats.Clear();
    }

    public QuestionManager.AnswerKeeper lastAnswer()
    {
        if(answers.Count == 0)
        {
            QuestionManager.AnswerKeeper answerKeeper = new QuestionManager.AnswerKeeper();
            answerKeeper.question = new QuestionManager.QuestionKeeper();
            answerKeeper.answerTime = 40;
            answerKeeper.rep = 0;
            answerKeeper.result = false;
            answerKeeper.question.id = 0;

            answers.Add(answerKeeper);
        }
        return answers[answers.Count - 1];
    }

    public void calcScore()
    {
        score = 0;
        foreach (QuestionManager.AnswerKeeper a in answers)
            if (a.result)
                score++;
    }

    public void addGameStat(GameStat g)
    {
        gameStats.Add(g);
    }

    public void addAnswer(QuestionManager.AnswerKeeper a)
    {
        if (answers.Exists(x => x.question.id == a.question.id))
        {
            answers.Remove(answers.Find(x => x.question.id == a.question.id));
            calcScore();
        }
        else if(a.result)
            score++;
        answers.Add(a);
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

    public void saveGameStats(EnumGame gameEnum)
    {
        if(null != gameEnum && gameStats.Count != 0)
            XmlHelpers.SaveToXML<List<GameStat>>("Assets/Resources/Xml/gameStats/" + gameEnum + "/" + login + ".xml", gameStats);
    }

    public void loadGameStats(EnumGame gameEnum)
    {
        Debug.Log("Xml/gameStats/" + gameEnum + "/" + login + ".xml");
        if(null != gameEnum)
        {
            try
            {
                TextAsset statsFile = (TextAsset)UnityEngine.Resources.Load("Xml/gameStats/" + gameEnum + "/" + login);
                gameStats = XmlHelpers.LoadFromTextAsset<GameStat>(statsFile, "ArrayOfGameStat");
                Debug.Log("loaded : " + gameStats.Count);
            }
            catch
            {

            }
        }
    }

    public void saveStats(int currentCourseId)
    {
        if(currentCourseId != 0)
        {
            List<Answer> stats = new List<Answer>();

            foreach (QuestionManager.AnswerKeeper a in answers)
            {
                stats.Add(new Answer(a.question.id, a.rep, a.result, a.answerTime));
            }

            XmlHelpers.SaveToXML<List<Answer>>("Assets/Resources/Xml/answers/" + currentCourseId + "/" + login + ".xml", stats);
        }
    }

    public void loadStats(int currentCourseId)
    {
        if(currentCourseId != 0)
        {
            try
            {
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

                    answers.Add(answerKeeper);
                }
                calcScore();
                C3PONetworkManager.Instance.setScore(networkPlayer, score);
            }
            catch
            {

            }
        }
    }
}
