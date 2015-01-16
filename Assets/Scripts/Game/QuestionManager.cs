using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class QuestionManager {

    /**************************************************************************************
     * Public Nested Classes & Enums                                                      *
     **************************************************************************************/
    [XmlType("QuestionKeeper")]
    public class QuestionKeeper
    {
        public QuestionKeeper()
        {
            reponses = new List<string>();
        }
        public QuestionKeeper(QuestionKeeper q)
        {
            qType = q.qType;
            question = q.question;
            reponses = new List<string>();
            bonneReponse = q.bonneReponse;
            explication = q.explication;
            foreach (string s in q.reponses)
                reponses.Add(s);
        }
        public QuestionType qType;
        public string question;
        [XmlArrayItem("r")]
        public List<string> reponses;
        public int bonneReponse;
        public string explication;
    }

    [XmlType("AnswerKeeper")]
    public class AnswerKeeper
    {
        [XmlIgnore]
        public QuestionKeeper question;
        public int rep;
        public bool result;
        public float answerTime;
    }

    public enum QuestionType
    {
        qM, // Multiple choice question
        qO  // Open choice question
    }

	/**************************************************************************************
	 * Public Attributes                                                                  *
	 **************************************************************************************/
    public static QuestionManager Instance
	{
		get {
			if (instance == null)
			{
				instance = new QuestionManager();
                instance.questionList = new List<QuestionManager.QuestionKeeper>();
                return instance;
			}
			else 
				return instance;
		}
		private set {
			instance = value;
		}
	}
	private QuestionKeeper questionBuffer;

    /**************************************************************************************
	 * Private Attributes                                                                 *
	 **************************************************************************************/
    private static QuestionManager instance = null;

    // dictionnaire contenant toutes les réponses à toutes les questions de tout le monde
    private List<QuestionKeeper> oldQuestions = null;
    private bool waitForAnswers = false;
    private float questionSendTime;
    private List<QuestionManager.QuestionKeeper> questionList;
    private int courseId;
    private int currentQuestionNb = 0;
    private bool xmlLoaded = false;

    /**************************************************************************************
	 * Public functions                                                                   *
	 **************************************************************************************/
   /* public void sendQuestion(string squestion)
    {
        questionBuffer.qType = QuestionType.qO;
		questionBuffer.question = squestion;
		oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
    }

    public void sendQuestion(string squestion, QuestionKeeper q)
    {
        questionBuffer = new QuestionKeeper(q);
        oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
    }*/

    /**
     * Functions used to send a question to students
     **/
    public void sendQuestion()
    {
        if (!waitForAnswers)
        {
            waitForAnswers = true;
            questionSendTime = Time.time;
            questionBuffer = new QuestionKeeper(questionList[currentQuestionNb]);
            oldQuestions.Add(new QuestionKeeper(questionBuffer));
            C3PONetworkManager.Instance.sendQuestion(questionBuffer);
            currentQuestionNb++;
        }
    }

    public bool isQuestionTimeOver()
    {
        return (xmlLoaded && currentQuestionNb == questionList.Count);
    }

    public void loadXml(int id)
    {
        courseId = id;
        TextAsset questionFile;
        questionFile = (TextAsset)UnityEngine.Resources.Load("xml/cours" + id);
        questionList = XmlHelpers.LoadFromTextAsset<QuestionManager.QuestionKeeper>(questionFile);
        currentQuestionNb = 0;
        xmlLoaded = true;
    }

    private void reset()
    {
        questionBuffer.reponses.Clear();
    }

    /**
     * Functions called when a question is received by the students
     **/
    public void rcvQuestion(string squestion)
    {
        reset();
        questionBuffer.qType = QuestionType.qO;
        questionBuffer.question = squestion;

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2)
    {
        reset();
        questionBuffer.qType = QuestionType.qM;
        questionBuffer.question = squestion;
        questionBuffer.reponses.Add(rep1);
        questionBuffer.reponses.Add(rep2);

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2, string rep3)
    {
        reset();
        questionBuffer.qType = QuestionType.qM;
        questionBuffer.question = squestion;
        questionBuffer.reponses.Add(rep1);
        questionBuffer.reponses.Add(rep2);
        questionBuffer.reponses.Add(rep3);

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2, string rep3, string rep4)
    {
        reset();
        questionBuffer.qType = QuestionType.qM;
        questionBuffer.question = squestion;
        questionBuffer.reponses.Add(rep1);
        questionBuffer.reponses.Add(rep2);
        questionBuffer.reponses.Add(rep3);
        questionBuffer.reponses.Add(rep4);

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    /*public void rcvAnswer(string login, string rep)
    {
        AnswerKeeper a = new AnswerKeeper();
        a.question = oldQuestions[oldQuestions.Count - 1];
        a.stringRep = rep;

        addPlayerAnswer(login, a);

        bool b = (a.question.reponses[a.question.bonneReponse] == rep);

        C3PONetworkManager.Instance.sendResult(a.question.explication, b);
    }*/

    public void rcvAnswer(ref Client c, int rep)
    {
        AnswerKeeper a = new AnswerKeeper();
        a.question = oldQuestions[oldQuestions.Count - 1];
        a.rep = rep;
        a.answerTime = Time.time - questionSendTime;
        a.result = (a.question.bonneReponse == rep);
        c.AnsweredLast = true;
        c.Answers.Add(a);
    }

    /**
     * Functions used to send an answer to the teacher
     **/
    /*public void sendAnswer(string rep)
    {
        C3PONetworkManager.Instance.sendAnswer(rep);
    }*/

    public void sendAnswer(int rep)
    {
        C3PONetworkManager.Instance.sendAnswer(rep);
    }

    /**************************************************************************************
	 * Public Constructor                                                                 *
	 **************************************************************************************/
    public QuestionManager()
    {
        if(instance != null)
        {
            throw new System.Exception("Cannot create a new instance of QuestionManager because there's one");
        }
        else
        {
            instance = this;

		    oldQuestions = new List<QuestionKeeper>();
		
		    questionBuffer = new QuestionKeeper();
        }
    }

    void sendResults()
    {
        bool b;
        string explication;
        foreach (KeyValuePair<string, Client> e in C3PONetworkManager.Instance.ClientsInfos)
        {
            b = e.Value.lastQuestionResult();
            explication = e.Value.lastAnswerExplication();
            C3PONetworkManager.Instance.sendResult(e.Value.NetworkPlayer, explication, b);
        }
    }

    void checkClientsAnswers()
    {
        foreach (KeyValuePair<string, Client> e in C3PONetworkManager.Instance.ClientsInfos)
        {
            if (!e.Value.AnsweredLast)
            {
                AnswerKeeper a = new AnswerKeeper();
                a.question = oldQuestions[oldQuestions.Count - 1];
                a.rep = a.question.bonneReponse + 1;
                a.answerTime = 40;
                a.result = false;

                e.Value.Answers.Add(a);
            }
            e.Value.AnsweredLast = false;
        }
    }

    void addDefaultAnswerToClient(ref Client c)
    {
    }

    public void update()
    {
        if(waitForAnswers)
        {
            if(Time.time - questionSendTime > 5)
            {
                checkClientsAnswers();
                sendResults();
                waitForAnswers = false;
            }
        }
    }
}
