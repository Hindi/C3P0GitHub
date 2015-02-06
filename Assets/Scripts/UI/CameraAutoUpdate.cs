using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraAutoUpdate : MonoBehaviour {

    [SerializeField]
    private GameObject androidInfo, paramPanel;
    private Canvas androidInfoCanvas, paramPanelCanvas, thisCanvas;

	// Use this for initialization
	void Start () {
        if(androidInfo != null)
            androidInfoCanvas = androidInfo.GetComponent<Canvas>();
        if(paramPanel != null)
            paramPanelCanvas = paramPanel.GetComponent<Canvas>();
        thisCanvas = GetComponent<Canvas>();

        Camera mainCamera = (GameObject.FindGameObjectWithTag("MainCamera")).GetComponent<Camera>();
        if (androidInfoCanvas != null)
        {
            androidInfoCanvas.worldCamera = mainCamera;
        }
        if (paramPanelCanvas != null)
        {
            paramPanelCanvas.worldCamera = mainCamera;
        }
        if (thisCanvas != null)
        {
            thisCanvas.worldCamera = mainCamera;
        }
	}
	
	// Update is called once per frame
    void Update()
    {
        Camera mainCamera = (GameObject.FindGameObjectWithTag("MainCamera")).GetComponent<Camera>();
        if (androidInfoCanvas != null)
        {
            androidInfoCanvas.worldCamera = mainCamera;
        }
        if (paramPanelCanvas != null)
        {
            paramPanelCanvas.worldCamera = mainCamera;
        }
        if (thisCanvas != null)
        {
            thisCanvas.worldCamera = mainCamera;
        }
	}
}
