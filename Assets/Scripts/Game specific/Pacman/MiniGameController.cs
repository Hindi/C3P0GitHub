using UnityEngine;
using System.Collections;

public class MiniGameController : MonoBehaviour {

	/// <summary>
	/// The player's GameObject
	/// </summary>
	GameObject pocman;

	/// <summary>
	/// The random number generator
	/// </summary>
	Randomizer rand;

	/// <summary>
	/// The position after the scene is being loaded
	/// </summary>
	Vector3 startingPosition;

	/// <summary>
	/// The forward vector of the object after the scene is being loaded
	/// </summary>
	Vector3 startingForward;

	/// <summary>
	/// The position of the target
	/// </summary>
	Vector3 targetPosition;

	/// <summary>
	/// True if a target has been hit
	/// </summary>
	bool isTarget = false;

	/// <summary>
	/// The color of the target
	/// </summary>
	Color col;

	/// <summary>
	/// True if the player encountered an enemy or a bonus
	/// </summary>
	bool gotHit = false;

	/// <summary>
	/// True is the mini-game has started
	/// </summary>
	bool miniGame = false;

	/// <summary>
	/// True if the game is over
	/// </summary>
	bool gameOver = false;

	bool frightenMode = false;

	float frightenDelay = 5f;

	float frightenTimer = 0f;

	bool isGhost = false;


	/// <summary>
	/// Called when the mini-game is over
	/// </summary>
	void niceShot(){
		isTarget = false;
		gotHit = false;
		miniGame = false;
		isGhost = false;
	}

	/// <summary>
	/// Called when the game is over
	/// </summary>
	void onGameOver(){
		gameOver = true;
	}

	/// <summary>
	/// Called when the player restart the game
	/// </summary>
	void onRestart(){
		gameOver = false;
	}

	void frightened(){
		frightenMode = true;
		frightenTimer = Time.time;
	}

	/// <summary>
	/// Called when the scene is loaded
	/// </summary>
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
		EventManager.AddListener(EnumEvent.FRIGHTENED, frightened);
	}

	/// <summary>
	/// Makes the object move toward the ghost
	/// </summary>
	/// <param name="ghost">The object the player encounters.</param>
	public void moveTo(GameObject ghost){
		if (!gotHit){
			if (ghost.tag != "Energizer"){
				isGhost = true;
			}
			targetPosition = ghost.transform.position;
			col = ghost.renderer.material.color;
			isTarget = true;
			gotHit = true;
			rand.setColor(col);
		}
	}

	/// Called at each frame
	void Update () {
		if (isGhost && frightenMode){
			EventManager.Raise(EnumEvent.MINIGAME_WIN);
			}
		else if (isTarget){
			EventManager<bool>.Raise(EnumEvent.MOVING, false);
			transform.LookAt(targetPosition);
			if(Vector3.Distance(transform.position, pocman.transform.position) > 0.5f){
				transform.position = Vector3.Lerp(transform.position, pocman.transform.position, 2 * Time.unscaledDeltaTime);
			}
			else {
				if (!miniGame){
					EventManager.Raise(EnumEvent.MINIGAME_START);
					rand.enabled = true;
					miniGame = true;
				}
			}
			rand.setColor(col);
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
		if (Time.time - frightenTimer > frightenDelay){
			frightenMode = false;
		}
	}

	/// <summary>
	/// Called when this script is destroyed
	/// </summary>
	void OnDestroy(){
		EventManager<GameObject>.RemoveListener(EnumEvent.ENCOUNTER, moveTo);
		EventManager.RemoveListener(EnumEvent.MINIGAME_LOST, niceShot);
		EventManager.RemoveListener(EnumEvent.MINIGAME_WIN, niceShot);
		EventManager.RemoveListener(EnumEvent.MINIGAME_TO, niceShot);
		EventManager.RemoveListener(EnumEvent.RESTARTSTATE, onRestart);
		EventManager.RemoveListener(EnumEvent.GAMEOVER, onGameOver);
		EventManager.RemoveListener(EnumEvent.FRIGHTENED, frightened);

	}
}