using UnityEngine;
using System.Collections;

public class Dodge : MonoBehaviour {
	bool miniGame = true;
	Camera circleCamera;

	// Use this for initialization
	void Start () {
		circleCamera = GameObject.Find("CircleCamera").GetComponent<Camera>();
	}
	
	void Update () {
		if (miniGame){
			if (Input.GetButton("Fire1")){
				RaycastHit hit;
				if (Physics.Raycast(circleCamera.ScreenToWorldPoint(Input.mousePosition - 1f * circleCamera.transform.forward), circleCamera.transform.forward, out hit, 10f)){
					if(hit.collider.tag == "Ball"){
						Debug.Log("Nice Shot!");
						SendMessageUpwards("niceShot", true, SendMessageOptions.DontRequireReceiver);
					}
				}
				else {
					SendMessageUpwards("niceShot", false, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
