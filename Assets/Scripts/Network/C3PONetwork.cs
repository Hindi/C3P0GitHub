/**************************************************************************************
 * Defines                                                                            *
 **************************************************************************************/
//#define C3POTeacher
#define C3POStudent

using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

/**
 * C3PONetwork is a class facilitating network communication for the C3PO project.
 * Only basic networking is managed here.
 *
 * Basic operations are:
 * - Create and connecting to the teacher server (which also holds a Unity MasterServer)
 * - Create and connecting to game servers for multi-player
 *
 * This class is built against the singleton model
 **/
public class C3PONetwork : MonoBehaviour {

	/**************************************************************************************
	 * Attributes                                                                         *
	 **************************************************************************************/
	// instance, should be unique
	private static C3PONetwork instance = null;
	public static C3PONetwork Instance
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

    /**
     * 
     **/
    [SerializeField]
    public bool IS_SERVER;

	// The IP/Hostname of the Unity MasterServer to connect to (has to exist on the Teacher's computer)
	private string masterHostname = null;
	// askHostnameGUI allows to know when to ask the user to enter the IP/Hostname of the MasterServer
	private bool askHostnameGUI = false;
	// allows to keep the user input until it's complete
	private string userTextStream = "ici";
	// Can be used to know if the server connected to is the teacher or not.
	public bool isConnectedToTeacher = false;
	// masterPort is the port number on which the MasterServer is set.
	[SerializeField]
	private int masterPort = 23466;
	// gamePort number is the port on which the game server is set.
	[SerializeField]
	private int gamePort = 25000;
	// Server lists received from MasterServer for both Teacher Server and match making
	private HostData[] hostList = null;

    // Used in the discovery
    UdpClient sender;
    UdpClient receiver;
    int remotePort = 19784;

	
	/**************************************************************************************
	 * Public functions                                                                   *
	 **************************************************************************************/
	
	/**
	 * Function to be called on the teacher version of the C3PO program.
	 * Launches a Unity MasterServer locally, and creates the main game server
	 **/
	public void createTeacherServer()
	{
		/* Launch MasterServer */
		string path = Application.dataPath;
		System.Diagnostics.Process.Start(path + @"\MasterServer\MasterServer.exe");
		
		/* Bind to it */
		MasterServer.port = masterPort;
        MasterServer.ipAddress = "127.0.0.1"; /* We're on the teacher computer so the master server is right there */
		Network.InitializeServer(1000,gamePort,false);
		MasterServer.RegisterHost("C3PO", "TeacherServer");
	}

	/**
	 * Connects to the MasterServer to find the teacher's game server and connects to it.
	 * If this.masterHostname is null, calls findMasterHostname to initialize it
	 **/
	public void connectTeacherServer()
	{
		if(!findMasterHostname())
		{
			return;
		}
		
		if(isConnectedToTeacher)
		{
			return;
		}
		
		MasterServer.port = masterPort;
		MasterServer.ipAddress = masterHostname;
		MasterServer.RequestHostList("C3PO");
	}
	
	/**
	 * Joins a multi-player server (p2p).
	 * If none exist, creates one or asks the teacher server to create one (depending on platform)
	 */
	public void joinMultiPlayerGame()
	{
		if(isConnectedToTeacher)
		{
			// disconnect
		}
		// connect to someone else
	}
	
	/**************************************************************************************
	 * Private Utility Functions                                                          *
	 **************************************************************************************/

	/**
	 * Tries to find the IP/Hostname of the MasterServer to connect to later.
	 * @returns if the discovery worked
	 * @modifies private string masterHostname : updates it in case the discovery is successful
	 **/
	private bool findMasterHostname()
	{
		/* 1st step : checks if this.masterHostname works */
		if (masterHostname != null)
		{
			return true;
		}
		/* 2nd step : checks Resources/Network/hostnames.xml */
		/* 3rd step : runs this.discovery() below */
		if (discovery())
		{
			return true;
		}
		/* 4th step : puts this.askHostnameGUI = true to get user input and returns false */
		askHostnameGUI = true;
		return false;
	}
	 
	/**
	 * Function to find by discovery the Teacher Server's IP/Hostname, to connect to it later.
	 *
	 * @returns if the discovery worked
	 * @modifies private string masterHostname : updates it in case the discovery is successful
	 **/
	private bool discovery()
	{
        try
        {
            if (receiver == null)
            {
                receiver = new UdpClient(remotePort);
                receiver.BeginReceive(new System.AsyncCallback(ReceiveData), null);
            }
        }
        catch(SocketException e)
        {
            return false;
        }
		return true;
	}

    private void ReceiveData(System.IAsyncResult result)
    {
        IPEndPoint receiveIPGroup = new IPEndPoint(IPAddress.Any, remotePort);
        byte[] received;
        if (receiver != null)
        {
            received = receiver.EndReceive(result, ref receiveIPGroup);
        }
        else
        {
            return;
        }
        receiver.BeginReceive(new System.AsyncCallback(ReceiveData), null);
        masterHostname = Encoding.ASCII.GetString(received);
    }

    private void SendData()
    {
        string customMessage = "127.0.0.1";
        sender.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length);
    }
	
	/**************************************************************************************
	 * Unity Default Delegates                                                            *
	 **************************************************************************************/

	// Awake is used to make sure there's only one
	void Awake()
	{
		if(instance != null)
		{
			Debug.Log("There should only be one C3PONetwork instance");
			throw new System.Exception("There should be only one active instance of the C3PONetwork component at a time.");
		}
		else
		{
			instance = this;
		}
	}
	
	// Use this for initialization
	void Start () {
        if(IS_SERVER)
        {
            sender = new UdpClient();
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
            sender.Connect(groupEP);

            InvokeRepeating("SendData", 0, 1f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(masterHostname);
	}
	
	/**
	 * Prints a text box in the middle of the screen to get the MasterServer IP/Hostname when it should
	 **/
	void OnGUI()
	{
		float width = Screen.width;
		float height = Screen.height;
		if(askHostnameGUI)
		{
			GUI.Box(new Rect(width/2 - width /3, height / 2 - height / 15,
							 width * 2/3, height *2/ 25),
					"Veuillez entrer l'IP ou le Nom d'hôte donné par le professeur");
			userTextStream = GUI.TextArea(new Rect(width/2 - width /6, height / 2 - height /35,
												width * 2/6, height * 2/50),
											userTextStream, 50);
			if(userTextStream.Length > 0 && userTextStream[userTextStream.Length-1] == '\n')
			{
				masterHostname = userTextStream.Replace(" ", "").Remove(userTextStream.Length -1);
				askHostnameGUI = false;
				connectTeacherServer();
			}
			
		}
	}
	
	/**
	 * Confirms that the server was successfully created.
	 **/
	void OnServerInitialized()
	{
		Debug.Log(Time.time + "Server Initialized");
	}
	
	/**
	 * If the IP/Hostname is not correct or the Network is down (doom)
	 **/
	void OnFailedToConnectToMasterServer()
	{
		masterHostname = null;
		connectTeacherServer();
	}
	
	/**
	 * Receives the host lists for both match making and teacher server
	 **/
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			hostList = MasterServer.PollHostList();
			if(hostList.Length > 0 && hostList[0].gameType == "C3PO")
			{
				Network.Connect(hostList[0]);
				isConnectedToTeacher = true;
				Debug.Log("Connected to teacher");
			}
		}
	}

}
