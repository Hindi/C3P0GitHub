using UnityEngine;
using System.Collections;

public class ExitHouse : MonoBehaviour {

	public float delay = 10.0f;
	float timer;
	Vector3 waypoint1 = new Vector3(13.5f, 0, -14f);
	Vector3 waypoint2 = new Vector3(13.5f, 0, -11f);


	// Use this for initialization
	void Start () {
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - timer > delay){
			if (Mathf.Abs(transform.position.x - waypoint1.x) > 0.1f){
				rigidbody.position = Vector3.Lerp(transform.position, waypoint1, Time.deltaTime);
			}
			else if (Mathf.Abs(transform.position.z - waypoint2.z) > 0.1f){
				rigidbody.position = Vector3.Lerp(transform.position, waypoint2, Time.deltaTime);
			}
			else{
					gameObject.SendMessage("moving", true);
					Destroy(this);
				}
			}
		}
	}
