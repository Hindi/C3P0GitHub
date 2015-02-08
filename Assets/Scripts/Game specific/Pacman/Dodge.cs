using UnityEngine;
using System.Collections;

/// <summary>
/// Dodge is the class that determines if the mini-game is won or lost.
/// </summary>
public class Dodge : MonoBehaviour {

	/// <summary>
	/// True if the mini-game has started
	/// </summary>
	bool miniGame = false;

	/// <summary>
	/// The Camera displayong the mini-game
	/// </summary>
	Camera circleCamera;

	/// <summary>
	/// How long the mini-game has been on
	/// </summary>
	float timer;

	/// <summary>
	/// The layer in which the mini-game is
	/// </summary>
	int layerMask = 1 << 11;

	/// <summary>
	/// The maximum duration of the mini-game
	/// </summary>
	[SerializeField]
	float delay = 5f;

	/// <summary>
	/// Called when the mini-game is started.
	/// </summary>
	void startMiniGame(){
		timer = Time.time;
		miniGame = true;
	}

	/// <summary>
	/// Called when this script is destroyed.
	/// </summary>
	void OnDestroy(){
		EventManager.RemoveListener(EnumEvent.MINIGAME_START, startMiniGame);
	}

	// Use this for initialization
	/// <summary>
	/// Called when the scene is loaded
	/// </summary>
	void Start () {
		circleCamera = GameObject.FindGameObjectWithTag("CircleCamera").GetComponent<Camera>();
		EventManager.AddListener(EnumEvent.MINIGAME_START, startMiniGame);
	}

	/// <summary>
	/// Called each frame
	/// </summary>
	void Update () {
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