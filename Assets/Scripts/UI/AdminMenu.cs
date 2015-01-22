using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdminMenu : MonoBehaviour {

    [SerializeField]
    private UI ui;

    [SerializeField]
    private InputField clientLoginInput;
    public string ClientLoginInput
    {
        get { return UI.cleanString(clientLoginInput.text); }
    }

    private Canvas previousCanvas;

    public void setPreviousCanvas(Canvas c)
    {
        previousCanvas = c;
    }

    public void leaveAdminPanel()
    {
        ui.updateCurrentCanvas(previousCanvas);
    }

    public void kickSpecificClient()
    {
        C3PONetworkManager.Instance.kickClient(ClientLoginInput);
        clientLoginInput.text = "";
    }

    public void kickEveryBody()
    {
        C3PONetworkManager.Instance.kickClient();
    }

    public void resetSpecificPassword()
    {
        C3PONetworkManager.Instance.resetPassword(ClientLoginInput);
        clientLoginInput.text = "";
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
