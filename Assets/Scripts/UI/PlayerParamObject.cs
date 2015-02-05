using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerParamObject : MonoBehaviour {

    [SerializeField]
    private GameObject androidInfo, paramPanel;
    private Canvas androidInfoCanvas, paramPanelCanvas;

	// Use this for initialization
	void Start () {
        if(androidInfo != null)
            androidInfoCanvas = androidInfo.GetComponent<Canvas>();
        if(paramPanel != null)
            paramPanelCanvas = paramPanel.GetComponent<Canvas>();
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
	}
}
