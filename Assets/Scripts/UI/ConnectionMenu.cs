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
    private InputField loginLabel;
    [SerializeField]
    private InputField passwordLabel;
    [SerializeField]
    private Text connectionAnswerLabel;

    private bool unityConnected;
    private bool authed;

	// Use this for initialization
	void Start () {
        unityConnected = false;
        authed = false;
        network = C3PONetwork.Instance;
        networkManager = C3PONetworkManager.Instance;
        EventManager.AddListener(EnumEvent.AUTHSUCCEEDED, onSucceededAuth);
        EventManager<string>.AddListener(EnumEvent.AUTHFAILED, onFailedAuth);
        EventManager.AddListener(EnumEvent.CONNECTIONTOUNITY, onConnectedToUnity);
        EventManager.AddListener(EnumEvent.DISCONNECTFROMUNITY, onDisconnectedFromUnity);
	}

    void onSucceededAuth()
    {
        authed = true;
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
	void Update () {
	
	}

    public void onConnectionClick()
    {
        ui.updateCurrentCanvas(connectionPrompt);
    }

    public void onConnectionQuitClick()
    {
        ui.updateCurrentCanvas(connectionWelcome);
    }

    private void connect()
    {
        if (loginLabel.text != "" && passwordLabel.text != "")
        {
            if (!unityConnected)
                networkManager.connectToTeacher(loginLabel.text, passwordLabel.text);
            else if (!authed)
                networkManager.tryTologIn(loginLabel.text, passwordLabel.text);
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
