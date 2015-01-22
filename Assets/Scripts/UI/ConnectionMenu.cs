using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectionMenu : MonoBehaviour {

    [SerializeField]
    private C3PONetwork network;
    private C3PONetworkManager networkManager;

    [SerializeField]
    private Canvas connectionPrompt;
    [SerializeField]
    private Canvas connectionWelcome;

    [SerializeField]
    private UI ui;

    [SerializeField]
    private InputField ipLabel;
    [SerializeField]
    private InputField loginLabel;
    public string LoginLabel
    {
        get { return UI.cleanString(loginLabel.text); }
    }
    [SerializeField]
    private InputField passwordLabel;
    [SerializeField]
    private Text connectionAnswerLabel;

    private bool unityConnected;
    private bool authed;
    private bool serverFound;
    private bool onForm;

	// Use this for initialization
    void Start()
    {
        ipLabel.Select();
        onForm = false;
        serverFound = false;
        unityConnected = false;
        authed = false;
        network = C3PONetwork.Instance;
        networkManager = C3PONetworkManager.Instance;
        EventManager.AddListener(EnumEvent.AUTHSUCCEEDED, onSucceededAuth);
        EventManager<string>.AddListener(EnumEvent.AUTHFAILED, onFailedAuth);
        EventManager<string>.AddListener(EnumEvent.SERVERIPRECEIVED, onServerIpRecieved);
        EventManager.AddListener(EnumEvent.CONNECTIONTOUNITY, onConnectedToUnity);
        EventManager.AddListener(EnumEvent.DISCONNECTFROMUNITY, onDisconnectedFromUnity);
	}

    void onServerIpRecieved(string ip)
    {
        ipLabel.text = ip;
        if (ipLabel.isFocused)
            switchUiSelect();
    }

    void onSucceededAuth()
    {
        authed = true;
    }

    void switchUiSelect()
    {
        if (ipLabel.isFocused)
            loginLabel.Select();
        else if (loginLabel.isFocused)
            passwordLabel.Select();
        else
            ipLabel.Select();
    }

    void onFailedAuth(string reason)
    {
        connectionAnswerLabel.text = reason;
        loginLabel.text = "";
        passwordLabel.text = "";
    }
    
    void onConnectedToUnity()
    {
        unityConnected = true;
    }

    void onDisconnectedFromUnity()
    {
        unityConnected = false;
        authed = false;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (ipLabel.IsActive() && !serverFound && C3PONetwork.Instance.getServerIp() != "")
        {
            onServerIpRecieved(C3PONetwork.Instance.getServerIp());
            serverFound = true;
        }
        if (onForm)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                onConnectionStartClick();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                switchUiSelect();
            }
        }
	}

    public void onConnectionClick()
    {
        onForm = true;
        ui.updateCurrentCanvas(connectionPrompt);
    }

    public void onConnectionQuitClick()
    {
        ui.updateCurrentCanvas(connectionWelcome);
        onForm = false;
    }

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
                networkManager.connectToTeacher(ipLabel.text, LoginLabel, passwordLabel.text);
            else if (!authed)
                networkManager.tryTologIn(LoginLabel, passwordLabel.text);
        }
    }

    public void onConnectionStartClick()
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

    public void onSingleClick()
    {
        Debug.Log("huk");
    }
}
