using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>The menu displayed when the client connects to the server.</summary>
public class ConnectionMenu : MonoBehaviour {

    /// <summary>The canvas containing the ui for the connection (ip, login, pass).</summary>
    [SerializeField]
    private Canvas connectionPrompt;
    /// <summary>The canvas containing the first ui seen by the client.</summary>
    [SerializeField]
    private Canvas connectionWelcome;
    /// <summary>The canvas containing the game list seen in local mode.</summary>
    [SerializeField]
    private Canvas singleGameSelect;

    /// <summary>Reference to the UI class.</summary>
    [SerializeField]
    private UI ui;

    /// <summary>The input where the client writes the ip.</summary>
    [SerializeField]
    private InputField ipLabel;

    /// <summary>The input where the client writes the login.</summary>
    [SerializeField]
    private InputField loginLabel;
    public string LoginLabel
    {
        get { return UI.cleanString(loginLabel.text); }
    }

    /// <summary>The input where the client writes the password.</summary>
    [SerializeField]
    private InputField passwordLabel;

    /// <summary>Text object that notify the client if there is an error.</summary>
    [SerializeField]
    private Text connectionAnswerLabel;

    /// <summary>True if the client is connected on unity.</summary>
    private bool unityConnected;

    /// <summary>True if the client is authenticated.</summary>
    private bool authed;

    /// <summary>True if the server was found automatically.</summary>
    private bool serverFound;

    /// <summary>True if the client is on the connectionPrompt canvas.</summary>
    private bool onForm;

    /// <summary>Called on start. Initialises the variables</summary>
    void Start()
    {
        //ipLabel.Select();
        onForm = false;
        serverFound = false;
        unityConnected = false;
        authed = false;
        EventManager.AddListener(EnumEvent.AUTHSUCCEEDED, onSucceededAuth);
        EventManager<string>.AddListener(EnumEvent.AUTHFAILED, onFailedAuth);
        EventManager<string>.AddListener(EnumEvent.SERVERIPRECEIVED, onServerIpRecieved);
        EventManager.AddListener(EnumEvent.CONNECTIONTOUNITY, onConnectedToUnity);
        EventManager.AddListener(EnumEvent.DISCONNECTFROMUNITY, onDisconnectedFromUnity);
	}

    /// <summary>Called if the server is recieved via udp.</summary>
    /// <param name="ip">The ip broadcasted by the server.</param>
    /// <returns>void</returns>
    void onServerIpRecieved(string ip)
    {
        ipLabel.text = ip;
        if (ipLabel.isFocused)
            switchUiSelect();
    }

    /// <summary>Called if the authentication succeeded.</summary>
    /// <returns>void</returns>
    void onSucceededAuth()
    {
        authed = true;
    }

    /// <summary>Switched the focus between the input fields.</summary>
    /// <returns>void</returns>
    public void switchUiSelect()
    {
        if (onForm)
        {
            if (ipLabel.isFocused)
            {
                loginLabel.Select();
            }
            else if (loginLabel.isFocused)
            {
                passwordLabel.Select();
            }
            else
                ipLabel.Select();
        }
    }

    /// <summary>Called if the authentication failed.</summary>
    /// <param name="reason">The reason why it failed.</param>
    /// <returns>void</returns>
    void onFailedAuth(string reason)
    {
        connectionAnswerLabel.text = reason;
        loginLabel.text = "";
        passwordLabel.text = "";
    }

    /// <summary>Called when the client is connected to the server (not authenticated yet).</summary>
    /// <returns>void</returns>
    void onConnectedToUnity()
    {
        unityConnected = true;
    }

    /// <summary>Called when the client is disconnected from the server.</summary>
    /// <returns>void</returns>
    void onDisconnectedFromUnity()
    {
        unityConnected = false;
        authed = false;
    }

    /// <summary>Called each frame. Check the inputs and update the ip field input.</summary>
    /// <returns>void</returns>
    void Update()
    {
        if (ipLabel.IsActive() && !serverFound && C3PONetwork.Instance.getServerIp() != "")
        {
            onServerIpRecieved(C3PONetwork.Instance.getServerIp());
            serverFound = true;
        }
	}

    /// <summary>Called when the client clicks on the "Connection" button of the Welcome menu (!= connecitonStart).</summary>
    /// <returns>void</returns>
    public void onConnectionClick()
    {
        onForm = true;
        ui.updateCurrentCanvas(connectionPrompt);
    }

    /// <summary>Called when the client click on the "Back" button.</summary>
    /// <returns>void</returns>
    public void onConnectionQuitClick()
    {
        ui.updateCurrentCanvas(connectionWelcome);
        onForm = false;
    }

    /// <summary>Called when the client tries to conenct to the server.</summary>
    /// <returns>void</returns>
    private void connect()
    {
        if (ipLabel.text == "")
        {
            connectionAnswerLabel.text = "IP field can't be empty.";
            return;
        }
        if (loginLabel.text == "")
        {
            connectionAnswerLabel.text = "Login field can't be empty.";
            return;
        }
        if (passwordLabel.text == "")
        {
            connectionAnswerLabel.text = "Password field can't be empty.";
            return;
        }
        if (loginLabel.text != "" && passwordLabel.text != "")
        {
            if (!unityConnected)
                C3PONetworkManager.Instance.connectToTeacher(ipLabel.text, LoginLabel, passwordLabel.text);
            else if (!authed)
                C3PONetworkManager.Instance.tryTologIn(LoginLabel, passwordLabel.text);
            else
                EventManager<string>.Raise(EnumEvent.LOADLEVEL, "QuestionAnswer");
        }
    }

    /// <summary>Called when the client clicks on the "Connection" button from the connection prompt.</summary>
    /// <returns>void</returns>
    public void onConnectionStartClick()
    {
        if (onForm)
        {
            connectionAnswerLabel.text = "";
            try
            {
                connect();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                onConnectionQuitClick();
            }
        }
    }

    /// <summary>Called when the client clicks on the "Single" button.</summary>
    /// <returns>void</returns>
    public void onSingleClick()
    {
        ui.updateCurrentCanvas(singleGameSelect);
        onForm = false;
    }

    /// <summary>Called when the client clicks on the "Exit" button.</summary>
    /// <returns>void</returns>
    public void onExitGameClick()
    {
        Application.Quit();
    }
}
