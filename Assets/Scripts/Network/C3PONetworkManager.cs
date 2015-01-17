﻿/**************************************************************************************
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
	private Dictionary<string, Client> clientsInfos = null;
    public Dictionary<string, Client> ClientsInfos
    {
        get { return clientsInfos; }
    }

    [SerializeField]
    private LevelLoader levelLoader;
    [SerializeField]
    private StateManager stateManager;

    PlayerData playerDatas;
	
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

    public void tryTologIn(string login, string password)
    {
        this.login = login;
        this.password = password;
        tryTologIn();
    }

    public void tryTologIn()
    {
		if(!tcpNetwork.isConnectedToTeacher)
		{
			isConnectedApp = false;
			throw new System.Exception("Failed to connect to teacher");
		}

        networkView.RPC("clientConnect", RPCMode.Server, login, password);
    }

    public void onFailedAuth(string reason)
    {
        EventManager<string>.Raise(EnumEvent.AUTHFAILED, reason);
    }

    void OnPlayerDisconnected(NetworkPlayer client)
    {
        foreach(KeyValuePair<string, Client> e in ClientsInfos)
        {
            if (e.Value.NetworkPlayer == client)
            {
                ClientsInfos.Remove(e.Key);
                return;
            }
        }
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
            case QuestionManager.QuestionType.qM:

                switch(question.reponses.Count)
                {
                    case 2:
                        networkView.RPC("rcvQuestionRPC2", RPCMode.Others, question.question, question.reponses[0], question.reponses[1]);
                        break;
                    case 3:
                        networkView.RPC("rcvQuestionRPC3", RPCMode.Others, question.question, question.reponses[0], question.reponses[1], question.reponses[2]);
                        break;
                    case 4:
                        networkView.RPC("rcvQuestionRPC4", RPCMode.Others, question.question, question.reponses[0], question.reponses[1], question.reponses[2], question.reponses[3]);
                        break;
                }
                break;
        }
	}
	
	public void sendAnswer(int rep)
	{
		networkView.RPC("rcvAnswerRPCi", RPCMode.Server, privateID, rep);
	}

    public void sendResult(NetworkPlayer netPlayer, string rep, bool b)
    {
        networkView.RPC("rcvResult", netPlayer, rep, b);
    }

    public void loadLevel(string name, int stateEnum)
    {
        networkView.RPC("rpcLoadLevel", RPCMode.Others, name, stateEnum);
    }

    public void sendNotifyWrongLogin(NetworkPlayer netPlayer, string name)
    {
        networkView.RPC("notifyWrongLogin", netPlayer, name);
    }

    public void sendNotifyWrongPassword(NetworkPlayer netPlayer, string name)
    {
        networkView.RPC("notifyWrongPassword", netPlayer, name);
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
        loginInfos.Add("raphael", "jesuisunmotdepasse");
        loginInfos.Add("a", "b");
        loginInfos.Add("b", "b");
        loginInfos.Add("c", "b");


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
        if (playerDatas.checkAuth(login, password, info.sender))
		{
			networkView.RPC("clientSuccessfullyConnected", info.sender, login+System.DateTime.Now);
            Client c = new Client();
            c.Login = login;
            string id = login + System.DateTime.Now;
            c.Id = id;
            c.NetworkPlayer = info.sender;
            clientsInfos.Add(id, c);
		}
	}
	
	[RPC]
	void clientSuccessfullyConnected(string uniqueID)
	{
        Debug.Log("auth succeeded");
		privateID = uniqueID;
		isConnectedApp = true;
        EventManager.Raise(EnumEvent.AUTHSUCCEEDED);
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

    [RPC]
    void rpcLoadLevel(string level, int stateEnum)
    {
        Debug.Log(level + " " + stateEnum);
        levelLoader.loadLevel(level);
        stateManager.changeState((StateEnum)stateEnum);
    }
	
	[RPC] 
	void rcvAnswerRPCi(string uniqueID, int rep)
	{
        if (clientsInfos.ContainsKey(uniqueID))
		{
            Client c = clientsInfos[uniqueID];
            QuestionManager.Instance.rcvAnswer(ref c, rep);
		}
	}

    [RPC]
    void rcvResult(string rep, bool b)
    {
        EventManager<string, bool>.Raise(EnumEvent.QUESTIONRESULT, rep, b);
    }

    [RPC]
    void notifyWrongLogin(string name)
    {
        onFailedAuth("Wrong login");
    }

    [RPC]
    void notifyWrongPassword(string name)
    {
        onFailedAuth("Wrong password");
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
        clientsInfos = new Dictionary<string, Client>();

		fillLoginInfos();
        //EventManager.AddListener(EnumEvent.CONNECTIONTOUNITY, onConnectedToUnity);

        playerDatas =  new PlayerData();
	}
	
	// Update is called once per frame
    void Update()
    {
	}
}
