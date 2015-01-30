using UnityEngine;
using System.Collections;

public class Randomizer : MonoBehaviour {

	public GameObject circle;
	float timer;
	// Use this for initialization
	
	void create(){
		GameObject dot = Instantiate (circle, transform.position, transform.rotation)as GameObject;
		dot.transform.parent = transform;
		dot.transform.Translate(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 3f, transform);
		SpriteRenderer sprite = dot.GetComponent<SpriteRenderer>();
		sprite.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}

	void OnEnable(){
		timer = Time.realtimeSinceStartup;
	}

	void Update () {
		if (Time.realtimeSinceStartup - timer > 0.5){
			create ();
			timer = Time.realtimeSinceStartup;
		}
	}

	void OnDisable(){
		BroadcastMessage("terminate",SendMessageOptions.DontRequireReceiver);
	}
}
