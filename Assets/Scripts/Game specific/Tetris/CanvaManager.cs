using UnityEngine;
using System.Collections;

public class CanvaManager : MonoBehaviour {

    [SerializeField]
    GameObject canvaPC;

    [SerializeField]
    GameObject canvaMobile;

	// Use this for initialization
	void Start () {
        if(Application.isMobilePlatform)
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
