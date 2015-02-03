using UnityEngine;
using System.Collections;

public class Dodge : MonoBehaviour {
	bool miniGame = false;
	Camera circleCamera;
	float timer;
	int layerMask = 1 << 10;

	void startMiniGame(){
		timer = Time.realtimeSinceStartup;
		miniGame = true;
	}


	// Use this for initialization
	void Start () {
		circleCamera = GameObject.Find("CircleCamera").GetComponent<Camera>();
	}
	
	void Update () {
		if (miniGame){
			if (Time.realtimeSinceStartup - timer > 5f){
				BroadcastMessage("niceShot", false, SendMessageOptions.DontRequireReceiver);
				miniGame = false;
			}
			if (Input.GetButton("Fire1")){
				RaycastHit hit;
				if (Physics.Raycast(circleCamera.ScreenToWorldPoint(Input.mousePosition - circleCamera.transform.forward), circleCamera.transform.forward, out hit, 10f, layerMask)){
					if(hit.collider.tag == "Circle"){
						BroadcastMessage("niceShot", true, SendMessageOptions.DontRequireReceiver);
						BroadcastMessage("destroyThat", SendMessageOptions.DontRequireReceiver);
						miniGame = false;

					}
					else {
						BroadcastMessage("niceShot", false, SendMessageOptions.DontRequireReceiver);
						miniGame = false;
					}
				}
				else {
					BroadcastMessage("niceShot", false, SendMessageOptions.DontRequireReceiver);
					miniGame = false;
				}
			}
		}
	}
}