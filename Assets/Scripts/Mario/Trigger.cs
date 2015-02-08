using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    public GameObject root;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            root.SendMessage("trigger", collider);
        }
    }
}
