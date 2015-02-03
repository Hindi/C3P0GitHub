using UnityEngine;
using System.Collections;


/// <summary>
/// This class handles the switch for mobile platform UI or PC UI for the tetris.
/// </summary>
public class CanvaManager : MonoBehaviour {

    /// <summary>
    /// Reference to the computer UI
    /// </summary>
    [SerializeField]
    GameObject canvaPC;

    /// <summary>
    /// Reference to the mobile platform UI
    /// </summary>
    [SerializeField]
    GameObject canvaMobile;

    /// <summary>
    /// Use for easier tests for mobile UI
    /// </summary>
    private bool isMobile;

	// Use this for initialization
    /// <summary>
    /// Called when the scene is loading. It activates the right UI depending on the platform.
    /// </summary>
    /// <returns>void</returns>
	void Start () {
        isMobile = Application.isMobilePlatform;
        if(isMobile)
        {
            canvaPC.gameObject.SetActive(false);
            canvaMobile.gameObject.SetActive(true);
        }
        else
        {
            canvaPC.gameObject.SetActive(true);
            canvaMobile.gameObject.SetActive(false);
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
