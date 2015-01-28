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
            id = q.id;
            question = q.question;
            reponses = new List<string>();
            bonneReponse = q.bonneReponse;
            explication = q.explication;
            foreach (string s in q.reponses)
                reponses.Add(s);
        }
        public int id;
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

    public string getQuestionTxt()
    {
        return questionList[currentQuestionNb].question;
    }

    public void goToPreviousQuestion()
    {
        if (currentQuestionNb > 0)
            currentQuestionNb--;
    }

    public void goToNextQuestion()
    {
        if (currentQuestionNb < questionList.Count - 1)
            currentQuestionNb++;
    }

    public void loadXml(int id)
    {
        courseId = id;
        TextAsset questionFile;
        questionFile = (TextAsset)UnityEngine.Resources.Load("xml/cours" + id);
        questionList = XmlHelpers.LoadFromTextAsset<QuestionManager.QuestionKeeper>(questionFile);

        for (int i = 0; i < questionList.Count; ++i)
            questionList[i].id = i;

        currentQuestionNb = 0;
        xmlLoaded = true;
    }

    private void reset()
    {
        questionBuffer.reponses.Clear();
    }

    public void rcvQuestion(string squestion, string rep1, string rep2)
    {
        reset();
        questionBuffer.question = squestion;
        questionBuffer.reponses.Add(rep1);
        questionBuffer.reponses.Add(rep2);

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2, string rep3)
    {
        reset();
        questionBuffer.question = squestion;
        questionBuffer.reponses.Add(rep1);
        questionBuffer.reponses.Add(rep2);
        questionBuffer.reponses.Add(rep3);

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2, string rep3, string rep4)
    {
        reset();
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
        c.addAnswer(a, courseId);
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
        int i = 0;
        int questionId = 0;
        string explication = oldQuestions[(oldQuestions.Count - 1)].explication;

        int[] answers = {0,0,0,0,0};

        foreach (KeyValuePair<string, Client> e in C3PONetworkManager.Instance.ClientsInfos)
        {
            i = e.Value.lastAnswer().question.bonneReponse;
            C3PONetworkManager.Instance.sendResult(e.Value.NetworkPlayer, explication, i);
            C3PONetworkManager.Instance.setScore(e.Value.NetworkPlayer, e.Value.Score);
            questionId = e.Value.lastAnswer().question.id;
            
            //Count for stats
            if(e.Value.lastAnswer().answerTime == 999)
                answers[0]++;
            else
                answers[e.Value.lastAnswer().rep]++;
        }

        HtmlHelpers.createAnswerStatPage("Course " + courseId + " Question " + questionId, answers[1], answers[2], answers[3], answers[4], answers[0]);
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
                a.answerTime = 999;
                a.result = false;

                e.Value.addAnswer(a, courseId);
                Debug.Log("Didn't answer, creating default answer");
            }
            e.Value.AnsweredLast = false;
        }
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
