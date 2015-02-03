using UnityEngine;
using System.Collections;

public class Destroyable : MonoBehaviour {

	void OnDestroy(){
		SendMessageUpwards("dotEaten", SendMessageOptions.DontRequireReceiver);
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
