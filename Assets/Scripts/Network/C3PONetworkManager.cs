/**************************************************************************************
 * Defines                                                                            *
 **************************************************************************************/
//#define C3POTeacher
#define C3POStudent

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>C3PONetworkManager is the class managing application-level connexion between students and the teacher.
/// It contains the authentication and all the rpcs.
/// This class is built against the singleton model.</summary>
public class C3PONetworkManager : MonoBehaviour {

	/**************************************************************************************
	 * Public Nested Classes & Enums                                                      *
	 **************************************************************************************/

	/**************************************************************************************
	 * Public Attributes                                                                  *
	 **************************************************************************************/
    /// <summary>This class is built against the singleton model.</summary>
    private static C3PONetworkManager instance = null;
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

    /// <summary>The login of the client.</summary>
    public string login;
    /// <summary>The password of the client.</summary>
    public string password;
    /// <summary>The current course id.</summary>
    private int currentCourseId;
    /// <summary>The current game.</summary>
    private EnumGame currentGameEnum;
	
	/** Used by the server only **/
	
	// 

    /// <summary>Dictionnaire contenant un identifiant unique pou chaque client.</summary>
	private Dictionary<string, Client> clientsInfos = null;
    public Dictionary<string, Client> ClientsInfos
    {
        get { return clientsInfos; }
    }

    /// <summary>Referenceto the level loader.</summary>
    [SerializeField]
    private LevelLoader levelLoader;

    /// <summary>Reference to the state manager.</summary>
    [SerializeField]
    private StateManager stateManager;

    /// <summary>Object that manages the players credentials.</summary>
    PlayerCredential playerCredentials;

    /// <summary>Stores the client id.</summary>
	private string privateID = null;

    [SerializeField]
    private ServerMenu serverMenu;

    /// <summary>Functions used to connect to the teacher server application-wise .</summary>
    /// <param name="ip">The ip of the server.</param>
    /// <param name="login">The login of the client.</param>
    /// <param name="password">The password of the client.</param>
    /// <returns>void</returns>
	public void connectToTeacher(string ip, string login, string password)
	{
        this.login = login;
        this.password = password;

        C3PONetwork.Instance.connectTeacherServer(ip);
	}

    /// <summary>Functions used to sign in on the server.</summary>
    /// <param name="login">The login of the client.</param>
    /// <param name="password">The password of the client.</param>
    /// <returns>void</returns>
    public void tryTologIn(string login, string password)
    {
        this.login = login;
        this.password = password;
        tryTologIn();
    }

    /// <summary>Functions used to connect to the teacher server application-wise .</summary>
    /// <returns>void</returns>
    public void tryTologIn()
    {
        if (!C3PONetwork.Instance.IsConnectedToTeacher)
		{
			throw new System.Exception("Failed to connect to teacher");
		}

        networkView.RPC("clientConnect", RPCMode.Server, login, password);
    }

    /// <summary>Called when the authentication failed and broadcast the reason with the event manager.</summary>
    /// <param name="reason">The reason why the authentication failed.</param>
    /// <returns>void</returns>
    public void onFailedAuth(string reason)
    {
        EventManager<string>.Raise(EnumEvent.AUTHFAILED, reason);
    }

    /// <summary>Unity function called when a player is disconnected. When this happens, save the player's stats and clear the lists</summary>
    /// <param name="client">Unity object that contains all the network related infos of the client.</param>
    /// <returns>void</returns>
    void OnPlayerDisconnected(NetworkPlayer client)
    {
        foreach (KeyValuePair<string, Client> e in clientsInfos)
        {
            if (e.Value.NetworkPlayer == client)
            {
                e.Value.saveStats(currentCourseId);
                clientsInfos.Remove(e.Key);
                return;
            }
        }
    }

    /// <summary>Load the client stats for the current course. Also initialise the courseId variable.</summary>
    /// <param name="courseId">The id of the current course.</param>
    /// <returns>void</returns>
    public void loadClientStats(int courseId)
    {
        currentCourseId = courseId;
        foreach (KeyValuePair<string, Client> e in clientsInfos)
        {
            e.Value.loadStats(courseId);
        }
    }

    /// <summary>Save the client game stats for the current game.</summary>
    /// <returns>void</returns>
    public void saveClientsGameStats()
    {
        foreach (KeyValuePair<string, Client> e in clientsInfos)
        {
            e.Value.saveGameStats(currentGameEnum);
        }
    }

    /// <summary>Save the client stats for the current course.</summary>
    /// <returns>void</returns>
    public void saveClientsStats()
    {
        foreach (KeyValuePair<string, Client> e in clientsInfos)
        {
            e.Value.saveStats(currentCourseId);
        }
    }

    /// <summary>Load the client game stats for the current game. Also initialise the currentGameEnum variable.</summary>
    /// <param name="stateEnum">The enum of the current game.</param>
    /// <returns>void</returns>
    private void loadPlayersGameStats(EnumGame stateEnum)
    {
        currentGameEnum = stateEnum;
        foreach (KeyValuePair<string, Client> e in clientsInfos)
            e.Value.loadGameStats((EnumGame)stateEnum);
    }

    /// <summary>Kick a specific client.</summary>
    /// <param name="login">The login of the client.</param>
    /// <returns>void</returns>
    public void kickClient(string login)
    {
        foreach (KeyValuePair<string, Client> e in clientsInfos)
            if (e.Value.Login == login)
                Network.CloseConnection(e.Value.NetworkPlayer, true);
    }

    /// <summary>Kick all the connected clients.</summary>
    /// <returns>void</returns>
    public void kickClient()
    {
        foreach (KeyValuePair<string, Client> e in clientsInfos)
            Network.CloseConnection(e.Value.NetworkPlayer, true);
    }

    /// <summary>Reset the password of all the connected clients.</summary>
    /// <returns>void</returns>
    public void resetPassword()
    {
        playerCredentials.resetPassword();
    }

    /// <summary>Reset the password of a specific client.</summary>
    /// <param name="login">The login of the client.</param>
    /// <returns>void</returns>
    public void resetPassword(string login)
    {
        playerCredentials.resetPassword(login);
    }
	 
    /// <summary>Functions used to send a question by the server. Calls the correct RPC (we can't overload RPC...)</summary>
    /// <param name="question">The question object to be sent.</param>
    /// <returns>void</returns>
	public void sendQuestion(QuestionManager.QuestionKeeper question)
    {
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
	}

    /// <summary>Functions used to send an answer to the server.</summary>
    /// <param name="rep">The id of the answer.</param>
    /// <returns>void</returns>
	public void sendAnswer(int rep)
	{
		networkView.RPC("rcvAnswerRPCi", RPCMode.Server, privateID, rep);
	}

    /// <summary>Functions used to send a result to the client.</summary>
    /// <param name="netPlayer">Unity object containing the client's info (used by the RPC).</param>
    /// <param name="id">The id ofthe answer.</param>
    /// <param name="rep">The explanation to be displayed for the client.</param>
    /// <returns>void</returns>
    public void sendResult(NetworkPlayer netPlayer, string rep, int id)
    {
        networkView.RPC("rcvResult", netPlayer, rep, id);
    }

    /// <summary>Functions used to broadcast to all the client the order to load a unity scene.</summary>
    /// <param name="name">The name of the scene.</param>
    /// <returns>void</returns>
    public void loadLevel(string name)
    {
        loadPlayersGameStats(IdConverter.levelToGame(name));
        saveClientsStats();
        int questionCount = QuestionManager.Instance.CurrentQuestionNb;
        float goodAnswerRatio = 0;

        foreach(KeyValuePair<string, Client> p in clientsInfos)
        {
            goodAnswerRatio = (float)((float)p.Value.Score / (float)questionCount);
            Debug.Log(goodAnswerRatio);
            networkView.RPC("rpcLoadLevel", p.Value.NetworkPlayer, name, goodAnswerRatio);
        }
    }

    /// <summary>Functions used to broadcast to all the client the order to unlock agame.</summary>
    /// <param name="name">The name of game.</param>
    /// <returns>void</returns>
    public void unlockGame(string name)
    {
        networkView.RPC("rpcUnlockGame", RPCMode.Others, name);
    }


    /// <summary>Functions used to notify the server that a player won or lost his game.</summary>
    /// <param name="b">True if won.</param>
    /// <returns>void</returns>
    public void sendNotifyGameOver(bool b)
    {
        networkView.RPC("notifyGameOver", RPCMode.Server, privateID, (int)currentGameEnum, b);
    }

    /// <summary>Functions used to notify a specific client that the login he uses does not exists.</summary>
    /// <param name="netPlayer">Unity object containing the client's info (used by the RPC).</param>
    /// <param name="name">The login.</param>
    /// <returns>void</returns>
    public void sendNotifyWrongLogin(NetworkPlayer netPlayer, string name)
    {
        networkView.RPC("notifyWrongLogin", netPlayer, name);
    }

    /// <summary>Functions used to notify a specific client that the password he uses is not correct.</summary>
    /// <param name="netPlayer">Unity object containing the client's info (used by the RPC).</param>
    /// <param name="name">The login.</param>
    /// <returns>void</returns>
    public void sendNotifyWrongPassword(NetworkPlayer netPlayer, string name)
    {
        networkView.RPC("notifyWrongPassword", netPlayer, name);
    }

    /// <summary>Functions used to notify a specific client that the login he use is already used.</summary>
    /// <param name="netPlayer">Unity object containing the client's info (used by the RPC).</param>
    /// <param name="name">The login.</param>
    /// <returns>void</returns>
    public void sendNotifyLoginInUse(NetworkPlayer netPlayer, string login)
    {
        networkView.RPC("notifyLoginInUse", netPlayer, login);
    }

    /// <summary>Functions used to update the score of a specific client.</summary>
    /// <param name="netPlayer">Unity object containing the client's info (used by the RPC).</param>
    /// <param name="score">The score.</param>
    /// <returns>void</returns>
    public void setScore(NetworkPlayer netPlayer,  int score)
    {
        networkView.RPC("setScoreRPC", netPlayer, score);
    }

    /// <summary>Functions used to by the client to askfor its score.</summary>
    /// <returns>void</returns>
    public void sendRequestScore()
    {
        networkView.RPC("requestScore", RPCMode.Server, privateID);
    }

    /// <summary>Functions used by the client to send the game stats to the server.</summary>
    /// <param name="gameId">The id of the current game.</param>
    /// <param name="paramId">The id of the current parameter used by the client.</param>
    /// <param name="score">The score the client achieved during this game.</param>
    /// <returns>void</returns>
    public void sendGameStats(int paramId, int score)
    {
        if(Network.isClient)
            networkView.RPC("sendGameStatsRPC", RPCMode.Server, privateID, (int)currentGameEnum, paramId, score);
    }

    /// <summary>Checks weither this login is already used or not.</summary>
    /// <param name="login">The login to be checked.</param>
    /// <returns>bool : True if the login is already in use.</returns>
    private bool loginInUse(string login)
    {
        foreach(KeyValuePair<string, Client> p in clientsInfos)
        {
            if (p.Value.Login == login)
                return true;
        }
        return false;
    }

    /// <summary>RPC : sign in request on the server.</summary>
    /// <param name="login">The login of the client.</param>
    /// <param name="password">The password of the client.</param>
    /// <param name="info">Unity object containing the infos on the client.</param>
    /// <returns>void</returns>
	[RPC]
	void clientConnect(string login, string password, NetworkMessageInfo info)
	{
        if (playerCredentials.checkAuth(login, password, info.sender))
		{
            if (loginInUse(login))
            {
                sendNotifyLoginInUse(info.sender, login);
            }
            else
            {
                networkView.RPC("clientSuccessfullyConnected", info.sender, login + System.DateTime.Now);
                Client c = new Client();
                c.Login = login;
                string id = login + System.DateTime.Now;
                c.Id = id;
                c.NetworkPlayer = info.sender;
                clientsInfos.Add(id, c);
                c.loadStats(currentCourseId);
            }
		}
	}

    /// <summary>RPC : notify the client he is authenticated. Sends back a unique Id to this client.</summary>
    /// <param name="uniqueID">The uniqueID of the client.</param>
    /// <returns>void</returns>
	[RPC]
	void clientSuccessfullyConnected(string uniqueID)
	{
		privateID = uniqueID;
        EventManager.Raise(EnumEvent.AUTHSUCCEEDED);
	}

    /// <summary>RPC : sends a question to the client.</summary>
    /// <param name="question">The question asked.</param>
    /// <param name="rep1">A possible answer.</param>
    /// <param name="rep2">A possible answer.</param>
    /// <returns>void</returns>
	[RPC]
	void rcvQuestionRPC2(string question, string rep1, string rep2)
    {
        QuestionManager.Instance.rcvQuestion(question, rep1, rep2);
	}

    /// <summary>RPC : sends a question to the client.</summary>
    /// <param name="question">The question asked.</param>
    /// <param name="rep1">A possible answer.</param>
    /// <param name="rep2">A possible answer.</param>
    /// <param name="rep3">A possible answer.</param>
    /// <returns>void</returns>
	[RPC]
	void rcvQuestionRPC3(string question, string rep1, string rep2, string rep3)
    {
        QuestionManager.Instance.rcvQuestion(question, rep1, rep2, rep3);
	}

    /// <summary>RPC : sends a question to the client.</summary>
    /// <param name="question">The question asked.</param>
    /// <param name="rep1">A possible answer.</param>
    /// <param name="rep2">A possible answer.</param>
    /// <param name="rep3">A possible answer.</param>
    /// <param name="rep4">A possible answer.</param>
    /// <returns>void</returns>
	[RPC]
	void rcvQuestionRPC4(string question, string rep1, string rep2, string rep3, string rep4)
    {
        QuestionManager.Instance.rcvQuestion(question, rep1, rep2, rep3, rep4);
	}

    /// <summary>RPC : order the client to load a unity scene.</summary>
    /// <param name="question">The question asked.</param>
    /// <param name="level">The level name.</param>
    /// <param name="goodAnswerRatio">The ratio that dtermines how the student succeded during thequestion / answer phase.</param>
    /// <returns>void</returns>
    [RPC]
    void rpcLoadLevel(string level, float goodAnswerRatio)
    {
        QuestionManager.Instance.AnswerRatio = goodAnswerRatio;
        currentGameEnum = IdConverter.levelToGame(level);
        levelLoader.loadLevel(level);
    }

    /// <summary>RPC : unlock a game for the client.</summary>
    /// <param name="name">The game name.</param>
    /// <returns>void</returns>
    [RPC]
    void rpcUnlockGame(string name)
    {
        EventManager<string>.Raise(EnumEvent.ADDGAME, name);
    }

    /// <summary>RPC : answer a response to the server.</summary>
    /// <param name="uniqueID">The client unique ID.</param>
    /// <param name="rep">The id of the answer.</param>
    /// <returns>void</returns>
	[RPC] 
	void rcvAnswerRPCi(string uniqueID, int rep)
	{
        if (clientsInfos.ContainsKey(uniqueID))
		{
            Client c = clientsInfos[uniqueID];
            QuestionManager.Instance.rcvAnswer(ref c, rep);
		}
	}

    /// <summary>RPC : give a result to the client.</summary>
    /// <param name="rep">The explanation.</param>
    /// <param name="i">The id of the correct answer.</param>
    /// <returns>void</returns>
    [RPC]
    void rcvResult(string rep, int i)
    {
        EventManager<string, int>.Raise(EnumEvent.QUESTIONRESULT, rep, i);
    }

    /// <summary>RPC : notify the client he used a wrong login.</summary>
    /// <param name="login">The login of the client.</param>
    /// <returns>void</returns>
    [RPC]
    void notifyWrongLogin(string login)
    {
        onFailedAuth("Wrong login : " + login);
    }

    /// <summary>RPC : notify the client he used a wrong password.</summary>
    /// <param name="pass">The pass of the client. Not used for now.</param>
    /// <returns>void</returns>
    [RPC]
    void notifyWrongPassword(string pass)
    {
        onFailedAuth("Wrong password.");
    }

    /// <summary>RPC : notify the client his login is already in use.</summary>
    /// <param name="login">The login of the client.</param>
    /// <returns>void</returns>
    [RPC]
    void notifyLoginInUse(string login)
    {
        onFailedAuth("Login " + login + " already in use.");
    }

    /// <summary>Functions used to update the score of a specific client.</summary>
    /// <param name="score">The score.</param>
    /// <returns>void</returns>
    [RPC]
    void setScoreRPC(int score)
    {
        EventManager<int>.Raise(EnumEvent.SCOREUPDATEQA, score);
    }


    /// <summary>Functions used to by the client to askfor its score (Q/A).</summary>
    /// <param name="id">The unique id of the client.</param>
    /// <returns>void</returns>
    [RPC]
    void requestScore(string id)
    {
        if(clientsInfos.ContainsKey(id))
            setScore(clientsInfos[id].NetworkPlayer, clientsInfos[id].Score);
    }

    /// <summary>Notify the server that the player won or lost his game.</summary>
    /// <param name="uniqueID">The uniqueID of the client.</param>
    /// <param name="gameId">The id of the current game.</param>
    /// <param name="b">True if won.</param>
    /// <returns>void</returns>
    [RPC]
    void notifyGameOver(string uniqueID, int gameId, bool b)
    {
        clientsInfos[uniqueID].saveGameStats((EnumGame)gameId);
    }

    /// <summary>Functions used by the client client to send the game stats to the server.</summary>
    /// <param name="gameId">The id of the current game.</param>
    /// <param name="paramId">The id of the current parameter used by the client.</param>
    /// <param name="score">The score the client achieved during this game.</param>
    /// <returns>void</returns>
    [RPC]
    void sendGameStatsRPC(string uniqueID, int gameId, int paramId, int score)
    {
        if (currentGameEnum == (EnumGame)gameId)
        {
            GameStat g = new GameStat(gameId, paramId, score, 0, 0);
            clientsInfos[uniqueID].addGameStat(g);
            serverMenu.addScore(score, paramId, clientsInfos[uniqueID].Login);
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
        if(C3PONetwork.Instance.IS_SERVER)
        {
            clientsInfos = new Dictionary<string, Client>();
            playerCredentials = new PlayerCredential();
        }
        currentCourseId = 0;
	}
	
	// Update is called once per frame
    void Update()
    {
	}
}
