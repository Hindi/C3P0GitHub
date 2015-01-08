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

	// Use this for initialization
	void Start () {
        network = C3PONetwork.Instance;
        networkManager = C3PONetworkManager.Instance;
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
            networkManager.connectToTeacher(loginLabel.text, passwordLabel.text);
        }
        catch (System.Exception)
        {
            Debug.Log("Connection failed");
            onConnectionQuitClick();
        }
    }

    public void onSingleClick()
    {
        Debug.Log("huk");
    }
}
