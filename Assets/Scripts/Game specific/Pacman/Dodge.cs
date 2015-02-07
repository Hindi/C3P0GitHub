using UnityEngine;
using System.Collections;

public class Dodge : MonoBehaviour {
	bool miniGame = false;
	Camera circleCamera;
	float timer;
	int layerMask = 1 << 11;
	[SerializeField]
	float delay = 5f;

	void startMiniGame(){
		timer = Time.time;
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
		Debug.DrawRay(circleCamera.ScreenToWorldPoint(Input.mousePosition - 0.5f * circleCamera.transform.forward), circleCamera.transform.forward, Color.green);
		if (miniGame){
			if (Time.time - timer > delay){
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