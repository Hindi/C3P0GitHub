using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>The administration menu used on the server.</summary>
public class AdminMenu : MonoBehaviour {

    /// <summary>Reference to the UI class.</summary>
    [SerializeField]
    private UI ui;

    /// <summary>The input used to write the login concerned by the action.</summary>
    [SerializeField]
    private InputField clientLoginInput;
    public string ClientLoginInput
    {
        get { return UI.cleanString(clientLoginInput.text); }
    }

    /// <summary>The active canvas before opening the admin panel.</summary>
    private Canvas previousCanvas;

    /// <summary>Save the previous canvas.</summary>
    public void setPreviousCanvas(Canvas c)
    {
        previousCanvas = c;
    }

    /// <summary>Close the admin panel and display the previous canvas.</summary>
    /// <returns>void</returns>
    public void leaveAdminPanel()
    {
        ui.updateCurrentCanvas(previousCanvas);
    }

    /// <summary>Apply the kick action on the specified client.</summary>
    /// <returns>void</returns>
    public void kickSpecificClient()
    {
        C3PONetworkManager.Instance.kickClient(ClientLoginInput);
        clientLoginInput.text = "";
    }

    /// <summary>Apply the kick action on every connected client.</summary>
    /// <returns>void</returns>
    public void kickEveryBody()
    {
        C3PONetworkManager.Instance.kickClient();
    }

    /// <summary>Reset the password of a specific client.</summary>
    /// <returns>void</returns>
    public void resetSpecificPassword()
    {
        C3PONetworkManager.Instance.resetPassword(ClientLoginInput);
        clientLoginInput.text = "";
    }
}
