using UnityEngine;
using System.Collections;

public class ConnectionMenu : MonoBehaviour {

    [SerializeField]
    private GameObject networkManager;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onConnectionClick()
    {
        Debug.Log("abwabwa");
    }

    public void onSingleClick()
    {
        Debug.Log("huk");
    }
}
