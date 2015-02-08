using UnityEngine;
using System.Collections;

public class ExitHouse : MonoBehaviour {

	/// <summary>
	/// How long the object have to wait
	/// </summary>
	[SerializeField]
	private float delay = 10.0f;

	/// <summary>
	/// How long the object has waited
	/// </summary>
	float timer;

	/// <summary>
	/// The position of the first waypoint
	/// </summary>
	Vector3 waypoint1 = new Vector3(13.5f, 0, -14f);

	/// <summary>
	/// The position of the second waypont
	/// </summary>
	Vector3 waypoint2 = new Vector3(13.5f, 0, -11f);

	/// <summary>
	/// True if it's a new game
	/// </summary>
	bool newGame;

	/// <summary>
	/// Called when the player restarts the game
	/// </summary>
	void restart(){
		newGame = true;
		timer = Time.time;
	}
	
	/// <summary>
	/// Called when the scene is loaded
	/// </summary>
	void Start () {
		timer = Time.time;
		newGame = true;
		EventManager.AddListener(EnumEvent.RESTARTGAME, restart);
	}
	
	/// <summary>
	/// Called at each frame
	/// </summary>
	void Update () {
		if (Time.time - timer > delay && newGame){
			if (Mathf.Abs(transform.position.x - waypoint1.x) > 0.1f){
				transform.position = Vector3.Lerp(transform.position, waypoint1, Time.deltaTime);
			}
			else if (Mathf.Abs(transform.position.z - waypoint2.z) > 0.1f){
				transform.position = Vector3.Lerp(transform.position, waypoint2, Time.deltaTime);
			}
			else{
				EventManager<bool, string>.Raise(EnumEvent.MOVING, true, gameObject.tag);
				newGame = false;
				}
			}
		}
	}
