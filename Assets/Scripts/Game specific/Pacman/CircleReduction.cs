using UnityEngine;
using System.Collections;

/**
 * CircleReduction is the class that controls the reduction of the dots that appear during the mini-game
 * */
/// <summary>
/// CircleReduction is the class that controls the reduction of the dots that appear during the mini-game
/// </summary>
public class CircleReduction : MonoBehaviour {
	/// <summary>
	/// How long the dot has stayed on the screen
	/// </summary>
	float timer;

	/// <summary>
	/// Called when the script is destroyed.
	/// </summary>
	void OnDestroy(){
		EventManager.RemoveListener(EnumEvent.MINIGAME_TERMINATE, terminate);
	}

	/// <summary>
	/// Called when the scene is loaded.
	/// </summary>
	void Start () {
		timer = Time.time;
		EventManager.AddListener(EnumEvent.MINIGAME_TERMINATE, terminate);
	}
	
	/// <summary>
	/// Called at each frame
	/// </summary>
	void Update () {
		transform.localScale -= new Vector3 (1f, 1f, 1f) * Time.deltaTime;
		if(Time.time - timer > 1){
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Called when the script recieves the event MINIGAME_TERMINATE
	/// </summary>
	void terminate(){
		Destroy(gameObject);
	}
}
