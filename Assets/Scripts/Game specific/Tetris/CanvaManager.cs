using UnityEngine;
using System.Collections;

public class CanvaManager : MonoBehaviour {

    [SerializeField]
    GameObject canvaPC;

    [SerializeField]
    GameObject canvaMobile;

    // Use for easier tests for mobile UI
    private bool isMobile;

	// Use this for initialization
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
