using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		SendMessageUpwards("dotEaten");
		Destroy(gameObject);
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
