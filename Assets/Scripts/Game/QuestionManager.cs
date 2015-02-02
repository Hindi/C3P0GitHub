using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// Manages all of the questions and answers.
/// </summary>
public class QuestionManager {

    /**************************************************************************************
     * Public Nested Classes & Enums                                                      *
     **************************************************************************************/


    /// <summary>
    /// Innher class that represents the question. It contains all of its datas.
    /// </summary>
    [XmlType("QuestionKeeper")]
    public class QuestionKeeper
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuestionKeeper()
        {
            reponses = new List<string>();
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
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
        /// <summary>
        /// The id of the question, also its position in the list for the current course.
        /// </summary>
        public int id;

        /// <summary>The string that'll be displayed client side.</summary>
        public string question;


        /// <summary>The possible responses.</summary>
        [XmlArrayItem("r")]
        public List<string> reponses;

        /// <summary>The correct answer.</summary>
        public int bonneReponse;

        /// <summary>The explanation.</summary>
        public string explication;
    }

    /// <summary>TThis class represents an answer sent by a client.</summary>
    [XmlType("AnswerKeeper")]
    public class AnswerKeeper
    {
        /// <summary>The question related to this answer.</summary>
        [XmlIgnore]
        public QuestionKeeper question;

        /// <summary>The id of the response the client sent.</summary>
        public int rep;

        /// <summary>The response of the client is either true or false.</summary>
        public bool result;

        /// <summary>The time it took for the client to answer (999 if he didn't answer in time).</summary>
        public float answerTime;
    }

	/**************************************************************************************
	 * Public Attributes                                                                  *
	 **************************************************************************************/
    /// <summary>This class is a singleton, get its instance with this function.</summary>
    /// <returns>The instance of QuestionManager.</returns>
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
    /// <summary>The instance is stored here.</summary>
    private static QuestionManager instance = null;

    /// <summary>The previously asked question.</summary>
    private List<QuestionKeeper> oldQuestions = null;

    /// <summary>If a question is sent, we wait for the answers.</summary>
    private bool waitForAnswers = false;

    /// <summary>The time we're waiting for answers.</summary>
    private float questionSendTime;

    /// <summary>The list of all the question to be asked, loaded from XML.</summary>
    private List<QuestionManager.QuestionKeeper> questionList;

    /// <summary>The current course.</summary>
    private int courseId;

    /// <summary>The id of the current question.</summary>
    private int currentQuestionNb = 0;

    /// <summary>True if the question were loaded.</summary>
    private bool xmlLoaded = false;

    /**************************************************************************************
	 * Public functions                                                                   *
	 **************************************************************************************/

    /// <summary>This function sends the question to the clients.</summary>
    /// <returns>void</returns>
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

    /// <summary>This function checks if the time allowed for the quesiton is over.</summary>
    /// <returns>bool : True if the time passed.</returns>
    public bool isQuestionTimeOver()
    {
        return (xmlLoaded && currentQuestionNb == questionList.Count);
    }

    /// <summary>This function access the question string displayed on clients.</summary>
    /// <returns>string</returns>
    public string getQuestionTxt()
    {
        return questionList[currentQuestionNb].question;
    }

    /// <summary>This function sets the previous question as current.</summary>
    /// <returns>void</returns>
    public void goToPreviousQuestion()
    {
        if (currentQuestionNb > 0)
            currentQuestionNb--;
    }

    /// <summary>This function sets the next question as current.</summary>
    /// <returns>Bool :false if the current qestion is the last of the list.</returns>
    public bool goToNextQuestion()
    {
        if (currentQuestionNb < questionList.Count - 1)
        {
            currentQuestionNb++;
            return true;
        }
        return false;
    }

    /// <summary>Load the questions for the current course from the xml file and add them to the questionList.</summary>
    /// <param name="id">The id of the current course</param>
    /// <returns>void</returns>
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

    /// <summary>Clears the question list.</summary>
    /// <returns>void</returns>
    public void unloadXml()
    {
        questionList.Clear();
    }

    /// <summary>Clears the question buffer.</summary>
    /// <returns>void</returns>
    private void reset()
    {
        questionBuffer.reponses.Clear();
    }

    /// <summary>Triggers the event that displays the question menu on the client.</summary>
    /// <param name="squestion">The string displayed, the actual question.</param>
    /// <param name="rep1">A possible response.</param>
    /// <param name="rep2">A possible response.</param>
    /// <returns>void</returns>
    public void rcvQuestion(string squestion, string rep1, string rep2)
    {
        reset();
        questionBuffer.question = squestion;
        questionBuffer.reponses.Add(rep1);
        questionBuffer.reponses.Add(rep2);

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    /// <summary>Triggers the event that displays the question menu on the client.</summary>
    /// <param name="squestion">The string displayed, the actual question.</param>
    /// <param name="rep1">A possible response.</param>
    /// <param name="rep2">A possible response.</param>
    /// <param name="rep3">A possible response.</param>
    /// <returns>void</returns>
    public void rcvQuestion(string squestion, string rep1, string rep2, string rep3)
    {
        reset();
        questionBuffer.question = squestion;
        questionBuffer.reponses.Add(rep1);
        questionBuffer.reponses.Add(rep2);
        questionBuffer.reponses.Add(rep3);

        EventManager<QuestionKeeper>.Raise(EnumEvent.QUESTIONRCV, questionBuffer);
    }

    /// <summary>Triggers the event that displays the question menu on the client.</summary>
    /// <param name="squestion">The string displayed, the actual question.</param>
    /// <param name="rep1">A possible response.</param>
    /// <param name="rep2">A possible response.</param>
    /// <param name="rep3">A possible response.</param>
    /// <param name="rep4">A possible response.</param>
    /// <returns>void</returns>
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

    /// <summary>Called when a client send an answer.</summary>
    /// <param name="c">The client that sent the answer.</param>
    /// <param name="rep">The id of the response.</param>
    /// <returns>void</returns>
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

    /// <summary>Called by the client to send an answer to the server.</summary>
    /// <param name="rep">The id of the response.</param>
    /// <returns>void</returns>
    public void sendAnswer(int rep)
    {
        C3PONetworkManager.Instance.sendAnswer(rep);
    }

    /**************************************************************************************
	 * Public Constructor                                                                 *
	 **************************************************************************************/
    /// <summary>Public constructor.</summary>
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

    /// <summary>Called by the server to send the result to the client.</summary>
    /// <returns>void</returns>
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

    /// <summary>Check the answers of the clients for the last question.</summary>
    /// <returns>void</returns>
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

    /// <summary>Called each frame, as soon as the quesiton time is over, check the answers ans send the results.</summary>
    /// <returns>void</returns>
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
