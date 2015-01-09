/**************************************************************************************
 * Defines                                                                            *
 **************************************************************************************/
//#define C3POTeacher
#define C3POStudent

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * C3PONetworkManager is the class managing application-level connexion between students and the teacher.
 * 
 * This class is built against the singleton model
 **/
public class C3PONetworkManager : MonoBehaviour {

	/**************************************************************************************
	 * Public Nested Classes & Enums                                                      *
	 **************************************************************************************/

	/**************************************************************************************
	 * Public Attributes                                                                  *
	 **************************************************************************************/
	public static C3PONetworkManager Instance
	{
		get {
			if (instance == null)
			{
				throw new System.Exception("There isn't any C3PONetwork instance");	
			}
			else 
				return instance;
		}
		private set {
			instance = value;
		}
	}
	
	public bool isConnectedApp;
	public string login;
    public string password;
	
	
	

	/**************************************************************************************
	 * Private Attributes                                                                 *
	 **************************************************************************************/
	 /** Used by both **/
	// instance, should be unique
	private static C3PONetworkManager instance = null;
	 
	// The instance of C3PONetwork to manage TCP-level connexion
	private C3PONetwork tcpNetwork = null;
	
	/** Used by the server only **/
	// dictionnaire contenant login/mot de passe
	private Dictionary<string, string> loginInfos = null;
	
	// dictionnaire contenant un identifiant unique
	private Dictionary<string, string> playerNetworkInfo = null;
	
	/** Used by the client only **/
	private string privateID = null;
	
	/**************************************************************************************
	 * Public functions                                                                   *
	 **************************************************************************************/
	/** 
	 * Functions used to connect to the teacher server application-wise 
	 **/
	public void connectToTeacher(string login, string password)
	{
        this.login = login;
        this.password = password;

		if (tcpNetwork == null) 
			tcpNetwork = C3PONetwork.Instance;
			
		tcpNetwork.connectTeacherServer();
	}

    public void onConnectedToUnity()
    {
		if(!tcpNetwork.isConnectedToTeacher)
		{
			isConnectedApp = false;
			throw new System.Exception("Failed to connect to teacher");
		}

        networkView.RPC("clientConnect", RPCMode.Server, login, password);
    }
	
	 
	/**
	 * Functions used to send a question by the server 
	 **/
	public void sendQuestion(QuestionManager.QuestionKeeper question)
	{
		switch(question.qType)
        {
            case QuestionManager.QuestionType.qO:
                networkView.RPC("rcvQuestionRPC0", RPCMode.Others, question.question);
                break;
            default:
                switch(question.choicesNb)
                {
                    case 2:
                        networkView.RPC("rcvQuestionRPC2", RPCMode.Others, question.question, question.rep1, question.rep2);
                        break;
                    case 3:
                        networkView.RPC("rcvQuestionRPC2", RPCMode.Others, question.question, question.rep1, question.rep2, question.rep3);
                        break;
                    case 4:
                        networkView.RPC("rcvQuestionRPC2", RPCMode.Others, question.question, question.rep1, question.rep2, question.rep3, question.rep4);
                        break;
                }
                break;
        }
	}
	
	/**
	 * Functions used to send an answer to the server
	 **/
	public void sendAnswer(string rep)
	{
		networkView.RPC("rcvAnswerRPCs", RPCMode.Server, privateID, rep);
	}
	
	public void sendAnswer(int rep)
	{
		networkView.RPC("rcvAnswerRPCi", RPCMode.Server, privateID, rep);
	}
	
	
	
	/**************************************************************************************
	 * Private Utility Functions                                                          *
	 **************************************************************************************/

	/**
	 * Tries to fill the login/password dictionary from XML file
	 * @returns if the filling was successful
	 **/
	private bool fillLoginInfos()
	{
		return false;
	}
	
	/**
	 * Checks if a login/password combination is correct.
	 * @arg1 login sent by the client
	 * @arg2 raw password sent by the client
	 * @returns if the login infos exist in the database.
	 **/
	private bool checkLog(string login, string password)
	{
		try
		{
			if(Hash(password) == loginInfos[login])
				return true;
			else
				return false;
		}
		catch(KeyNotFoundException)
		{
			return false;
		}
	}
	
	/**
	 * Transforms a password into a custom Hash to check against the stored value
	 * @arg1 a password to be hashed
	 * @returns @arg1 hashed
	 **/
	private string Hash(string password)
	{
		return password;
	}
	
	/**************************************************************************************
	 * Private RPC Functions                                                     	      *
	 **************************************************************************************/
	/** 
	 * Functions used to connect to the teacher server application-wise 
	 **/
	[RPC]
	void clientConnect(string login, string password, NetworkMessageInfo info)
	{
		if(checkLog(login, password))
		{
			networkView.RPC("clientSuccessfullyConnected", info.sender, login+System.DateTime.Now);
			playerNetworkInfo.Add(login+System.DateTime.Now, login);
			
		}
	}
	
	[RPC]
	void clientSuccessfullyConnected(string uniqueID)
	{
		privateID = uniqueID;
		isConnectedApp = true;
        EventManager.Raise(EnumEvent.CONNECTIONESTABLISHED);
	}
	
	/**
	 * Functions used to send a question by the server 
	 **/
	[RPC]
	void rcvQuestionRPC0(string question)
	{
        QuestionManager.Instance.rcvQuestion(question);
	}
	
	[RPC]
	void rcvQuestionRPC2(string question, string rep1, string rep2)
    {
        QuestionManager.Instance.rcvQuestion(question, rep1, rep2);
	}
	
	[RPC]
	void rcvQuestionRPC3(string question, string rep1, string rep2, string rep3)
    {
        QuestionManager.Instance.rcvQuestion(question, rep1, rep2, rep3);
	}
	
	[RPC]
	void rcvQuestionRPC4(string question, string rep1, string rep2, string rep3, string rep4)
    {
        QuestionManager.Instance.rcvQuestion(question, rep1, rep2, rep3, rep4);
	}
	
	/**
	 * Functions used to send an answer to the server
	 **/
	[RPC] 
	void rcvAnswerRPCs(string uniqueID, string rep)
	{
		if(playerNetworkInfo.ContainsKey(uniqueID))
		{
            QuestionManager.Instance.rcvAnswer(playerNetworkInfo[uniqueID], rep);
            giveResult(playerNetworkInfo[uniqueID], "huk", true);
		}
	}
	
	[RPC] 
	void rcvAnswerRPCi(string uniqueID, int rep)
	{
		if(playerNetworkInfo.ContainsKey(uniqueID))
		{
            QuestionManager.Instance.rcvAnswer(playerNetworkInfo[uniqueID], rep);
            giveResult(playerNetworkInfo[uniqueID], "huk", true);
		}
	}

    private void giveResult(string uniqueID, string rep, bool b)
    {
        networkView.RPC("rcvResult", RPCMode.Others, uniqueID, rep, b);
    }

    [RPC]
    void rcvResult(string uniqueID, string rep, bool b)
    {
		if(playerNetworkInfo.ContainsKey(uniqueID))
        {
            EventManager<bool>.Raise(EnumEvent.QUESTIONRESULT, true);
        }
    }
	
	/**************************************************************************************
	 * Unity Default Delegates                                                            *
	 **************************************************************************************/
	void Awake()
	{
		if(instance != null)
		{
			throw new System.Exception("There should be only one active instance of the C3PONetworkManager component at a time.");
		}
		else
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		tcpNetwork = C3PONetwork.Instance;

        loginInfos = new Dictionary<string, string>();
        loginInfos.Add("raphael", "jesuisunmotdepasse");
        loginInfos.Add("a", "b");
		playerNetworkInfo = new Dictionary<string, string>();
		
		fillLoginInfos();

        EventManager.AddListener(EnumEvent.CONNECTIONTOUNITY, onConnectedToUnity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
