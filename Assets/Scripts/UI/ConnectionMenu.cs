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

    private bool unityConnected = false;

	// Use this for initialization
	void Start () {
        network = C3PONetwork.Instance;
        networkManager = C3PONetworkManager.Instance;
        EventManager.AddListener(EnumEvent.CONNECTIONTOUNITY, onConnectedToUnity);
        EventManager.AddListener(EnumEvent.DISCONNECTFROMUNITY, onDisconnectedFromUnity);
	}

    void onConnectedToUnity()
    {
        loginLabel.text = "";
        passwordLabel.text = "";
        unityConnected = true;
    }

    void onDisconnectedFromUnity()
    {
        unityConnected = false;
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

    public void onConnectionStartClick()
    {
        try
        {
            if (unityConnected)
                networkManager.connectToTeacher(loginLabel.text, passwordLabel.text);
            else
                networkManager.onConnectedToUnity();
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
