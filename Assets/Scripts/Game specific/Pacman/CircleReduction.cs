using UnityEngine;
using System.Collections;

public class CircleReduction : MonoBehaviour {
	float timer;


	// Use this for initialization
	void Start () {
		timer = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale -= new Vector3 (0.1f, 0.1f, 0.1f) * Time.unscaledDeltaTime;
		if(Time.realtimeSinceStartup - timer > 10f){
			Destroy(gameObject);
		}
	}

	void terminate(){
		Destroy(gameObject);
	}
}
