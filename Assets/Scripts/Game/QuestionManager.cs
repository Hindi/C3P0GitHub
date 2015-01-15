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
        public int intRep;
        public string stringRep;
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
	private Dictionary<string, List<AnswerKeeper>> playerAnswers = null;
	private List<QuestionKeeper> oldQuestions = null;

    /**************************************************************************************
	 * Public functions                                                                   *
	 **************************************************************************************/
    public void sendQuestion(string squestion)
    {
        questionBuffer.qType = QuestionType.qO;
		questionBuffer.question = squestion;
		oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
    }

    /**
     * Functions used to send a question to students
     **/
    public void sendQuestion(QuestionKeeper q)
    {
        questionBuffer = new QuestionKeeper(q);
        oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
    }

    public void sendQuestion(string squestion, QuestionKeeper q)
    {
        questionBuffer = new QuestionKeeper(q);
		oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
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

    public void rcvAnswer(string login, string rep)
    {
        AnswerKeeper a = new AnswerKeeper();
        a.question = oldQuestions[oldQuestions.Count - 1];
        a.stringRep = rep;

        if (!playerAnswers.ContainsKey(login))
            playerAnswers.Add(login, new List<AnswerKeeper>());
        playerAnswers[login].Add(a);

        bool b = (a.question.reponses[a.question.bonneReponse] == rep);

        C3PONetworkManager.Instance.sendResult(login, a.question.explication, b);
        EventManager<AnswerKeeper>.Raise(EnumEvent.ANSWERRCV,a);
    }

    public void rcvAnswer(string login, int rep)
    {
        AnswerKeeper a = new AnswerKeeper();
        a.question = oldQuestions[oldQuestions.Count - 1];
        a.intRep = rep;

        if (!playerAnswers.ContainsKey(login))
            playerAnswers.Add(login, new List<AnswerKeeper>());
        playerAnswers[login].Add(a);
        bool b = (a.question.bonneReponse == rep);

        C3PONetworkManager.Instance.sendResult(login, a.question.explication, b);
        EventManager<AnswerKeeper>.Raise(EnumEvent.ANSWERRCV, a);
    }

    /**
     * Functions used to send an answer to the teacher
     **/
    public void sendAnswer(string rep)
    {
        C3PONetworkManager.Instance.sendAnswer(rep);
    }

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
            
		    playerAnswers = new Dictionary<string, List<AnswerKeeper>>();
		    oldQuestions = new List<QuestionKeeper>();
		
		    questionBuffer = new QuestionKeeper();
        }
    }
}
