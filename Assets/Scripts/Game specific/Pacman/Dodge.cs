using UnityEngine;
using System.Collections;

public class Dodge : MonoBehaviour {
	bool miniGame = false;
	Camera circleCamera;
	float timer;
	int layerMask = 1 << 11;

	void startMiniGame(){
		timer = Time.realtimeSinceStartup;
		miniGame = true;
	}

	void OnDestroy(){
		EventManager.RemoveListener(EnumEvent.MINIGAME_START, startMiniGame);
	}

	// Use this for initialization
	void Start () {
		circleCamera = GameObject.FindGameObjectWithTag("CircleCamera").GetComponent<Camera>();
		EventManager.AddListener(EnumEvent.MINIGAME_START, startMiniGame);
	}
	
	void Update () {
		if (miniGame){
			if (Time.realtimeSinceStartup - timer > 5f){
				EventManager.Raise(EnumEvent.MINIGAME_TO);
				miniGame = false;
			}
			if (Input.GetButton("Fire1")){
				RaycastHit hit;
				if (Physics.Raycast(circleCamera.ScreenToWorldPoint(Input.mousePosition - 0.5f * circleCamera.transform.forward), circleCamera.transform.forward, out hit, 5f, layerMask)){
					if(hit.collider.tag == "Circle"){
						EventManager.Raise(EnumEvent.MINIGAME_WIN);
						miniGame = false;
					}
					else{
						EventManager.Raise(EnumEvent.MINIGAME_LOST);
						miniGame = false;
					}
				}
				else{
					EventManager.Raise(EnumEvent.MINIGAME_LOST);
					miniGame = false;
				}
			}
		}
	}
}