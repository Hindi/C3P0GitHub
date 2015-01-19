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

    ConnectionManager ipReceiver;

    string customMessage = "192.168.0.19";

    int remotePort = 19784;

    public class ConnectionManager
    {
        public Sender ipSender;

        public ConnectionManager()
        {
            ipSender = new Sender();
        }

        private string serverIp = "";
        public string ServerIp
        {
            get { return serverIp; }
            set { serverIp = value; }
        }

        private bool received = false;
        public bool Received
        {
            get { return received; }
            set { received = value; }
        }

        private readonly UdpClient udp = new UdpClient(15000);
        public void StartListening()
        {
            this.udp.BeginReceive(Receive, new object());
        }
        private void Receive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 15000);
                byte[] bytes = udp.EndReceive(ar, ref ip);
                string message = Encoding.ASCII.GetString(bytes);
                if (message == "C3PO request ip")
                    ipSender.sendIp();
                else if (message.Split(' ')[0] == "C3PO")
                {
                    serverIp = message.Split(' ')[1];
                    received = true;
                }
                else
                    StartListening();
            }
            catch (Exception ex)
            {
                Debug.Log("EndReceive failed : " + ex);
            }
        }
    }

    public class Sender
    {
        private string serverIp;
        public string ServerIp
        {
            get { return serverIp; }
            set { serverIp = value; }
        }

        private string localIPAddress()
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

        public void sendIpRequest()
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 15000);
            byte[] bytes = Encoding.ASCII.GetBytes("C3PO request ip");
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }

        public void sendIp()
        {
            serverIp = localIPAddress();
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 15000);
            byte[] bytes = Encoding.ASCII.GetBytes("C3PO 192.168.0.19");
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }
    }
	
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
        masterHostname = "192.168.0.19";
        return true;
		/* 1st step : checks if this.masterHostname works */
		if (masterHostname != null)
		{
			return true;
		}
		/* 4th step : puts this.askHostnameGUI = true to get user input and returns false */
		askHostnameGUI = true;
		return false;
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

    void onServerIpRecieved(string ip)
    {
        ipReceiver.ServerIp = ip;
    }
	
	// Use this for initialization
    void Start()
    {
        ipReceiver = new ConnectionManager();
        if(IS_SERVER)
        {
            ipReceiver.ipSender.sendIp();
        }
        else
        {
            ipReceiver.ipSender.sendIpRequest();
            ipReceiver.StartListening();
            EventManager<string>.AddListener(EnumEvent.SERVERIPRECEIVED, onServerIpRecieved);
        }
	}

    public string getMyIp()
    {
        return ipReceiver.ipSender.ServerIp;
    }

    public string getServerIp()
    {
        return ipReceiver.ServerIp;
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
