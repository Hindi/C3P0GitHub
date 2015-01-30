using UnityEngine;
using System.Collections;

public class ScatterCycle : MonoBehaviour {

	float timer;

	// Use this for initialization
	void Start () {
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - timer > 90f){
			BroadcastMessage("scatter",SendMessageOptions.DontRequireReceiver);
			timer = Time.time;
		}
	
	}
}
