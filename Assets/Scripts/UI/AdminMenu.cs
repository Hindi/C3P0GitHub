using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdminMenu : MonoBehaviour {

    [SerializeField]
    private UI ui;

    [SerializeField]
    private InputField clientLoginInput;

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
        C3PONetworkManager.Instance.kickClient(clientLoginInput.text);
    }

    public void kickEveryBody()
    {
        C3PONetworkManager.Instance.kickClient();
    }

    public void resetSpecificPassword()
    {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
