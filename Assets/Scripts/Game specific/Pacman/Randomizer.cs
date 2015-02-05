using UnityEngine;
using System.Collections;

public class Randomizer : MonoBehaviour {

	public GameObject circle;
	float timer;
	// Use this for initialization
	
	void create(){
		GameObject dot = Instantiate (circle, transform.position, transform.rotation)as GameObject;
		dot.transform.parent = transform;
		dot.transform.Translate(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0.4f, transform);
		SpriteRenderer sprite = dot.GetComponent<SpriteRenderer>();
		sprite.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}

	void OnEnable(){
		timer = Time.realtimeSinceStartup;
	}

	void Update () {
		if (Time.realtimeSinceStartup - timer > 0.1){
			create();
			timer = Time.realtimeSinceStartup;
		}
	}

	void OnDisable(){
		EventManager.Raise(EnumEvent.MINIGAME_TERMINATE);
	}
}
