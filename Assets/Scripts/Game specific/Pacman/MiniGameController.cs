using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {
	GameObject pocman;

	Randomizer rand;

	Vector3 startingPosition;
	Vector3 startingForward;
	Vector3 targetPosition;

	bool isTarget = false;
	bool gotHit = false;
	bool miniGame = false;
	bool gameOver = false;


	void niceShot(){
		isTarget = false;
		gotHit = false;
		miniGame = false;
	}

	void onGameOver(){
		gameOver = true;
	}

	void onRestart(){
		gameOver = false;
	}

	// Use this for initialization
	void Start () {
		pocman = GameObject.FindGameObjectWithTag("Player");
		rand = GetComponentInChildren<Randomizer>();
		startingPosition = transform.position;
		startingForward = transform.position + 25 * transform.forward;
		targetPosition = startingPosition;
		EventManager<GameObject>.AddListener(EnumEvent.ENCOUNTER, moveTo);
		EventManager.AddListener(EnumEvent.MINIGAME_LOST, niceShot);
		EventManager.AddListener(EnumEvent.MINIGAME_WIN, niceShot);
		EventManager.AddListener(EnumEvent.MINIGAME_TO, niceShot);
		EventManager.AddListener (EnumEvent.RESTARTSTATE, onRestart);
		EventManager.AddListener(EnumEvent.GAMEOVER, onGameOver);
	}


	public void moveTo(GameObject ghost){
		if (!gotHit){
			targetPosition = ghost.transform.position;
			isTarget = true;
			gotHit = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if (isTarget){
			EventManager<bool>.Raise(EnumEvent.MOVING, false);
			transform.LookAt(Vector3.Lerp(transform.position + transform.forward, targetPosition, Time.unscaledDeltaTime));
			if(Vector3.Distance(transform.position, pocman.transform.position) > 0.5f){
				transform.position = Vector3.Lerp(transform.position, pocman.transform.position, 2 * Time.unscaledDeltaTime);
			}
			else {
				if (!miniGame){
					rand.enabled = true;
					EventManager.Raise(EnumEvent.MINIGAME_START);
					miniGame = true;
				}
			}
		}
		else {
			rand.enabled = false;
			targetPosition = startingPosition;
			transform.LookAt(Vector3.Lerp(transform.position + transform.forward, startingForward, Time.unscaledDeltaTime));
			transform.position = Vector3.Lerp(transform.position, startingPosition, 2 * Time.unscaledDeltaTime);

			if (Vector3.Distance(transform.position, startingPosition) < 0.5f){
				EventManager<bool>.Raise(EnumEvent.MOVING,true);
			}
		}	
	}

	void OnDestroy(){
		EventManager<GameObject>.RemoveListener(EnumEvent.ENCOUNTER, moveTo);
		EventManager.RemoveListener(EnumEvent.MINIGAME_LOST, niceShot);
		EventManager.RemoveListener(EnumEvent.MINIGAME_WIN, niceShot);
		EventManager.RemoveListener(EnumEvent.MINIGAME_TO, niceShot);
		EventManager.RemoveListener(EnumEvent.RESTARTSTATE, onRestart);
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
	}
}