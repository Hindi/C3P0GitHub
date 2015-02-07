using UnityEngine;
using System.Collections;

public class CircleReduction : MonoBehaviour {
	float timer;

	void OnDestroy(){
		EventManager.RemoveListener(EnumEvent.MINIGAME_TERMINATE, terminate);
	}

	// Use this for initialization
	void Start () {
		timer = Time.time;
		EventManager.AddListener(EnumEvent.MINIGAME_TERMINATE, terminate);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale -= new Vector3 (.5f, .5f, 0.5f) * Time.deltaTime;
		if(Time.time - timer > 2){
			Destroy(gameObject);
		}
	}

	void terminate(){
		Destroy(gameObject);
	}
}
