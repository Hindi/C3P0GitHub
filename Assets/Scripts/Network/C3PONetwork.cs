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
/// <summary>
/// C3PONetwork is a class facilitating network communication for the C3PO project.
/// Only basic networking is managed here.
///  This class is built against the singleton model</summary>
public class C3PONetwork : MonoBehaviour {

	/**************************************************************************************
	 * Attributes                                                                         *
	 **************************************************************************************/

    /// <summary>C3PO is a singleton;</summary>
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


    /// <summary>Set to true or false in Unity before compiling.</summary>
    [SerializeField]
    public bool IS_SERVER;

    // <summary>Can be used to know if the server connected to is the teacher or not.</summary>
	private bool isConnectedToTeacher = false;
    public bool IsConnectedToTeacher
    {
        get { return isConnectedToTeacher; }
        private set { }
    }

    // <summary>MasterPort is the port number on which the MasterServer is set.</summary>
	[SerializeField]
	private int masterPort = 23466;

    // <summary>GamePort number is the port on which the game server is set.</summary>
	[SerializeField]
	private int gamePort = 25000;
    
    // <summary>Server lists received from MasterServer for both Teacher Server and match making.</summary>
	private HostData[] hostList = null;

    // <summary>Object that does the detection of the game server.</summary>
    ConnectionManager ipReceiver;

    // <summary>Inner class made for the detection of the server.</summary>
    public class ConnectionManager
    {
        // <summary>Object that broadcasts the ip.</summary>
        public Sender ipSender;

        // <summary>The udp socket.</summary>
        private UdpClient udp;

        // <summary>True if it's the server.</summary>
        private bool isServer;

        // <summary>Constructor.</summary>
        /// <param name="server">True if it's the server.</param>
        public ConnectionManager(bool server)
        {
            isServer = server;
            udp = new UdpClient(15000);
            ipSender = new Sender(udp);
        }

        // <summary>CStore the server ip as a string.</summary>
        private string serverIp = "";
        public string ServerIp
        {
            get { return serverIp; }
            set { serverIp = value; }
        }

        // <summary>True if the ip has been recieved.</summary>
        private bool received = false;
        public bool Received
        {
            get { return received; }
            set { received = value; }
        }

        // <summary>Starts the coroutine that listen for udp broadcast.</summary>
        public void StartListening()
        {
            this.udp.BeginReceive(Receive, new object());
        }

        // <summary>The function used by the coroutine to receive the server's ip.</summary>
        private void Receive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 15000);
                byte[] bytes = udp.EndReceive(ar, ref ip);
                string message = Encoding.ASCII.GetString(bytes);
                if (!isServer)
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

    // <summary>Inner class that boradcast the server's ip on the local network.</summary>
    public class Sender
    {
        // <summary>Stores the server IP as a string.</summary>
        private string serverIp;
        public string ServerIp
        {
            get { return serverIp; }
            set { serverIp = value; }
        }

        // <summary>The udp socket.</summary>
        UdpClient udp;

        // <summary>Stores the server IP as an IP object.</summary>
        IPEndPoint ip;

        // <summary>Constructor.</summary>
        /// <param name="udp">Udp socket used to broadcast on local network.</param>
        public Sender(UdpClient udp)
        {
            this.udp = udp;
            serverIp = localIPAddress();
            ip = new IPEndPoint(IPAddress.Broadcast, 15000);
        }

        // <summary>Finds and returns the local ipAdress.</summary>
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

        // <summary>Broadcast the server's ip.</summary>
        public void sendIp(string ip)
        {
            broadCastSomething("C3PO " + ip);
        }

        // <summary>Broadcast a string on the network.</summary>
        public void broadCastSomething(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            udp.Send(bytes, bytes.Length, ip);
        }
    }
	
	/**************************************************************************************
	 * Public functions                                                                   *
	 **************************************************************************************/
	
    /// <summary>Function to be called on the teacher version of the C3PO program.
    /// Launch and initialize the  master server and register the server.</summary>
    /// <returns>void</returns>
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

    /// <summary>Connects to the MasterServer to find the teacher's game server and connects to it.</summary>
    /// <param name="ip">The ip of the server.</param>
    /// <returns>void</returns>
	public void connectTeacherServer(string ip)
	{		
		MasterServer.port = masterPort;
		MasterServer.ipAddress = ip;
		MasterServer.RequestHostList("C3PO");
	}
	
	/**************************************************************************************
	 * Private Utility Functions                                                          *
	 **************************************************************************************/
	
	/**************************************************************************************
	 * Unity Default Delegates                                                            *
	 **************************************************************************************/

    /// <summary>Awake is used to make sure there's only one.</summary>
    /// <param name="ip">The ip of the server.</param>
    /// <returns>void</returns>
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

    /// <summary>Called when the client recieves the server's ip and stores it.</summary>
    /// <param name="ip">The ip of the server.</param>
    /// <returns>void</returns>
    void onServerIpRecieved(string ip)
    {
        ipReceiver.ServerIp = ip;
    }

    /// <summary>Uses the ipSender to broadcast the local ip address.</summary>
    /// <returns>void</returns>
    void sendIp()
    {
        ipReceiver.ipSender.sendIp(getMyIp());
    }


    /// <summary>This is where we differenciate server and client. The first broadcast the ip and the other tries to catch it.</summary>
    /// <returns>void</returns>
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

    /// <summary>Get the ip address of the machine.</summary>
    /// <returns>string : the ip address</returns>
    public string getMyIp()
    {
        return ipReceiver.ipSender.ServerIp;
    }

    /// <summary>Get the ip address of the server.</summary>
    /// <returns>void</returns>
    public string getServerIp()
    {
        if (ipReceiver.ServerIp != null)
            return ipReceiver.ServerIp;
        else
            return "";
    }

    /// <summary>Called when connected to server, tries to login.</summary>
    /// <returns>void</returns>
    void OnConnectedToServer()
    {
        EventManager.Raise(EnumEvent.CONNECTIONTOUNITY);
        C3PONetworkManager.Instance.tryTologIn();
    }

    /// <summary>Called when the client is disconnected from the server. Broadcast the info with events.</summary>
    /// <param name="info">Unity object that contains the disconnection info.</param>
    /// <returns>void</returns>
    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        EventManager.Raise(EnumEvent.DISCONNECTFROMUNITY);
        isConnectedToTeacher = false;
    }
	
    /// <summary>If the IP/Hostname is not correct or the Network is down notice the client.</summary>
    /// <returns>void</returns>
	void OnFailedToConnectToMasterServer()
	{
        C3PONetworkManager.Instance.onFailedAuth("Cannot find the server.");
	}
	
    /// <summary>Called when the master server sends the host list. Connects to the server that name is "C3PO"</summary>
    /// <param name="msEvent">Unity object that contains the list of the servers connected to the master server.</param>
    /// <returns>void</returns>
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
            hostList = MasterServer.PollHostList();
			if(hostList.Length > 0 && hostList[0].gameType == "C3PO")
			{
                Network.Connect(hostList[0]);
                isConnectedToTeacher = true;
			}
		}
	}


}
