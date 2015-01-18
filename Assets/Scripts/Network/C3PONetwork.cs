/**************************************************************************************
 * Defines                                                                            *
 **************************************************************************************/
//#define C3POTeacher
#define C3POStudent

using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
    [SerializeField]
    private C3PONetworkManager ntm;

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

    private string serverIp;
    string customMessage = "192.168.0.19";

    // Used in the discovery
    UdpClient udpClient;
    IPEndPoint receiveIPGroup;
    IPEndPoint broadCastGroup;
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
		/*string path = Application.dataPath;
		System.Diagnostics.Process.Start(path + @"\MasterServer\MasterServer.exe");*/
		
		/* Bind to it */
		MasterServer.port = masterPort;
        MasterServer.ipAddress = "192.168.0.19"; /* We're on the teacher computer so the master server is right there */
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

        Debug.Log(masterHostname);
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
        /*masterHostname = "192.168.0.19";
        return true;*/
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
            /*IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, remotePort);
            receiver = new UdpClient(remotePort);
            receiver.Receive(ref remoteIpEndPoint);
            Debug.Log("huk");
            Debug.Log(remoteIpEndPoint.Address.ToString());
            IPEndPoint e = new IPEndPoint(IPAddress.Any, remotePort);
            UdpClient u = new UdpClient(e);

            UdpState s = new UdpState();
            s.e = e;
            s.u = u;

            u.BeginReceive(new AsyncCallback(ReceiveCallback), s);*/
            return false;
        }
        catch(SocketException e)
        {
            Debug.Log(e);
            return false;
        }
	}

    public void ReceiveMessages()
    {
        // Receive a message and write it to the console.
        IPEndPoint e = new IPEndPoint(IPAddress.Any, remotePort);
        UdpClient u = new UdpClient(e);

        UdpState s = new UdpState();
        s.e = e;
        s.u = u;

        Debug.Log("listening for messages");
        u.BeginReceive(new AsyncCallback(ReceiveCallback), s);

        // Do some work while we wait for a message. For this example,
        // we'll just sleep
    }

    public static bool messageReceived = false;
    public void ReceiveCallback(IAsyncResult ar)
    {
        UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
        IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

        Byte[] receiveBytes = u.EndReceive(ar, ref e);
        string receiveString = Encoding.ASCII.GetString(receiveBytes);

        Debug.Log("Received: {0}" + receiveString);
        messageReceived = true;
    }

    public class UdpState
    {
        public IPEndPoint e;
        public UdpClient u;
    }

    public void StartReceivingIP()
    {
        try
        {
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }

    /*private void ReceiveData(IAsyncResult result)
    {
        receiveIPGroup = new IPEndPoint(IPAddress.Any, remotePort);
        StateObject so = (StateObject)result.AsyncState ;
        Socket s = so.workSocket;
        byte[] received  = receiver.EndReceive(result, ref receiveIPGroup);
        Debug.Log("Begin recieve");
        string receivedString = Encoding.ASCII.GetString(received);
        Debug.Log("Begin recieve" + receivedString);

        receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
    }*/

    private void SendData()
    {
        udpClient.Send(Encoding.ASCII.GetBytes(customMessage), customMessage.Length, broadCastGroup);
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
    void Start()
    {
        if(IS_SERVER)
        {
            udpClient = new UdpClient();
            broadCastGroup = new IPEndPoint(IPAddress.Broadcast, remotePort);

            InvokeRepeating("SendData", 0, 0.1f);
            serverIp = localIPAddress();
        }
        else
        {
            ReceiveMessages();
            receiveIPGroup = new IPEndPoint(IPAddress.Any, remotePort);
            //StartReceivingIP();
        }
	}

    public string getMyIp()
    {
        return localIPAddress();
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

	}

    void OnConnectedToServer()
    {
        EventManager.Raise(EnumEvent.CONNECTIONTOUNITY);
        ntm.tryTologIn();
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        EventManager.Raise(EnumEvent.DISCONNECTFROMUNITY);
    }
	
	/**
	 * If the IP/Hostname is not correct or the Network is down (doom)
	 **/
	void OnFailedToConnectToMasterServer()
	{
		masterHostname = null;
		connectTeacherServer();
        C3PONetworkManager.Instance.onFailedAuth("Cannot find the server.");
	}

    public string localIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
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
                //Network.Connect(hostList[0]);
                Network.Connect(hostList[0]);
                isConnectedToTeacher = true;
			}
		}
	}


}
