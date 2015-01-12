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
        public QuestionKeeper() { }
        public QuestionKeeper(QuestionKeeper q)
        {
            qType = q.qType;
            choicesNb = q.choicesNb;
            question = q.question;
            rep1 = q.rep1; rep2 = q.rep2; rep3 = q.rep3; rep4 = q.rep4;
        }
        public QuestionType qType;
        public int choicesNb;
        public string question;
        public string rep1, rep2, rep3, rep4;
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
    public void sendQuestion(QuestionKeeper qBuffer)
    {
        C3PONetworkManager.Instance.sendQuestion(qBuffer);
    }

    public void sendQuestion(string squestion, string rep1, string rep2)
    {
        questionBuffer.qType = QuestionType.qM;
		questionBuffer.choicesNb = 2;
		questionBuffer.question = squestion;
		questionBuffer.rep1 = rep1;
		questionBuffer.rep2 = rep2;
		oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
    }

    public void sendQuestion(string squestion, string rep1, string rep2, string rep3)
    {
        questionBuffer.qType = QuestionType.qM;
		questionBuffer.choicesNb = 3;
		questionBuffer.question = squestion;
		questionBuffer.rep1 = rep1;
		questionBuffer.rep2 = rep2;
		questionBuffer.rep3 = rep3;
		oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
    }

    public void sendQuestion(string squestion, string rep1, string rep2, string rep3, string rep4)
    {
        questionBuffer.qType = QuestionType.qM;
		questionBuffer.choicesNb = 3;
		questionBuffer.question = squestion;
		questionBuffer.rep1 = rep1;
		questionBuffer.rep2 = rep2;
		questionBuffer.rep3 = rep3;
		questionBuffer.rep4 = rep4;
		oldQuestions.Add(new QuestionKeeper(questionBuffer));
        C3PONetworkManager.Instance.sendQuestion(questionBuffer);
    }

    /**
     * Functions called when a question is received by the students
     **/
    public void rcvQuestion(string squestion)
    {
        questionBuffer.qType = QuestionType.qM;
        questionBuffer.choicesNb = 4;
        questionBuffer.question = squestion;

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2)
    {
        questionBuffer.qType = QuestionType.qM;
        questionBuffer.choicesNb = 4;
        questionBuffer.question = squestion;
        questionBuffer.rep1 = rep1;
        questionBuffer.rep2 = rep2;

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2, string rep3)
    {
        questionBuffer.qType = QuestionType.qM;
        questionBuffer.choicesNb = 4;
        questionBuffer.question = squestion;
        questionBuffer.rep1 = rep1;
        questionBuffer.rep2 = rep2;
        questionBuffer.rep3 = rep3;

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    public void rcvQuestion(string squestion, string rep1, string rep2, string rep3, string rep4)
    {
        questionBuffer.qType = QuestionType.qM;
        questionBuffer.choicesNb = 4;
        questionBuffer.question = squestion;
        questionBuffer.rep1 = rep1;
        questionBuffer.rep2 = rep2;
        questionBuffer.rep3 = rep3;
        questionBuffer.rep4 = rep4;

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
