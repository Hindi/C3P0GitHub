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

	// The IP/Hostname of the Unity MasterServer to connect to (has to exist on the Teacher's computer)
	private string masterHostname = null;
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

    int remotePort = 19784;

    public class ConnectionManager
    {
        public Sender ipSender;
        private UdpClient udp;
        private bool isServer;

        public ConnectionManager(bool server)
        {
            isServer = server;
            udp = new UdpClient(15000);
            ipSender = new Sender(udp);
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
                if (isServer)
                {
                }
                else
                {
                    if (message.Split(' ')[0] == "C3PO")
                    {
                        serverIp = message.Split(' ')[1];
                        received = true;
                    }
                    else
                        StartListening();
                }
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

        UdpClient udp;
        IPEndPoint ip;

        public Sender(UdpClient udp)
        {
            this.udp = udp;
            serverIp = localIPAddress();
            ip = new IPEndPoint(IPAddress.Broadcast, 15000);
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
            broadCastSomething("request ip");
        }

        public void sendIp(string ip)
        {
            broadCastSomething("C3PO " + ip);
        }

        public void broadCastSomething(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            udp.Send(bytes, bytes.Length, ip);
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
        MasterServer.ipAddress = getMyIp(); /* We're on the teacher computer so the master server is right there */
		Network.InitializeServer(1000,gamePort,false);
		MasterServer.RegisterHost("C3PO", "TeacherServer");
	}

	/**
	 * Connects to the MasterServer to find the teacher's game server and connects to it.
	 * If this.masterHostname is null, calls findMasterHostname to initialize it
	 **/
	public void connectTeacherServer(string ip)
	{		
		MasterServer.port = masterPort;
		MasterServer.ipAddress = ip;
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

    void sendIp()
    {
        ipReceiver.ipSender.sendIp(getMyIp());
    }
	
	// Use this for initialization
    void Start()
    {
        if(IS_SERVER)
        {
            ipReceiver = new ConnectionManager(true);
            InvokeRepeating("sendIp", 0, 1);
        }
        else
        {
            ipReceiver = new ConnectionManager(false);
            EventManager<string>.AddListener(EnumEvent.SERVERIPRECEIVED, onServerIpRecieved);
            ipReceiver.StartListening();
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
	 * Confirms that the server was successfully created.
	 **/
	void OnServerInitialized()
	{

	}

    void OnConnectedToServer()
    {
        EventManager.Raise(EnumEvent.CONNECTIONTOUNITY);
        C3PONetworkManager.Instance.tryTologIn();
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
