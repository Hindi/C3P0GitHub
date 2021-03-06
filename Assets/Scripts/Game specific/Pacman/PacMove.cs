﻿	using UnityEngine;
using System.Collections;
/**
 * PacMove is a class which determines how the player will move.
 **/
/// <summary>
/// PacMove is a class which determines how the player will move.
/// </summary>
public class PacMove : MonoBehaviour {

	/**
	 * Attributes
	 **/

	/// <summary>
	/// The current direction.
	/// </summary>
	Vector3 curDir = Vector3.right;

	/// <summary>
	/// The next direction.
	/// </summary>
	Vector3 nextDir = Vector3.right;

	/// <summary>
	/// The tile representation of the maze.
	/// [i, j] is the tile located on the ith row (from top to bottom) and jth column (from left to right).
	/// For the game graphics, a tile i a 1x1 square and and its coordinates are (j, -i)
	/// Each number represents a tile :
	/// - 0 is a dead tile, neither the player nor the enemy AIs can be located in a dead tile at any time.
	/// - 1 or higher is a legal tile, both players and enemies can travel between those tiles.
	/// </summary>
	int[,] pacGrid;

	/// <summary>
	/// The object the player collides with.
	/// </summary>
	GameObject obj;

	/// <summary>
	/// True is the player is allowed to move
	/// </summary>
	bool isMoving;


	/**
	 * Funtions
	 **/

	/// <summary> Restart the game </summary>
	/// <returns>void</returns>
	void restart(){
		rigidbody.position = new Vector3(13, 0, -23);
	}

	/// <summary>
	/// Allows the player to move or not
	/// </summary>
	/// <param name="res">True if the player is allowed to move.</param>
	/// <returns>void</returns>
	void moving(bool res){
		isMoving = res;
	}

	/// <summary>
	/// How long the power up lasted.
	/// </summary>
	float powerTimer;
	

	/// <summary>
	/// Gets the current direction the player is heading.
	/// </summary>
	/// <returns>The current direction the player is heading.</returns>
	public Vector3 getCurDir(){
		return curDir;
	}

	/// <summary>
	/// Check if the next position the player will go is within bounds
	/// </summary>
	/// <returns><c>true</c>, if the next position is within bounds, <c>false</c> otherwise.</returns>
	/// <param name="next">The direction the player will head towards.</param>
	bool isValid (Vector3 next){
		Vector3 predicted = transform.position + next;
		int predX = Mathf.RoundToInt(predicted.x);
		int predY = Mathf.RoundToInt(- predicted.z);
		if(pacGrid[predY,predX] >= 1){
			return true;
		}
		return false;
	}


	/// <summary>
	/// Makes the player move towards the current direction
	/// </summary>
	/// <returns>void</returns>
	void move()
	{
		rigidbody.MovePosition(rigidbody.position + curDir * 4 * Time.deltaTime);
		curDir = nextDir;
	}

	/// <summary>
	/// Called when the player loses the mini-game
	/// </summary>
	void onMiniGameLost(){
			EventManager<GameObject>.Raise(EnumEvent.SENTENCE_LOST, obj);
	}

	/// <summary>
	/// Called when the player wins the mini-game
	/// </summary>
	void onMiniGameWin(){
		EventManager<GameObject>.Raise(EnumEvent.SENTENCE_WIN, obj);
	}

	/// <summary>
	/// Called when the player takes to much time in the mini-game
	/// </summary>
	void onMiniGameTO(){
		EventManager<GameObject>.Raise(EnumEvent.SENTENCE_TO, obj);
	}

	/// <summary>
	/// Called when the player restarts the game
	/// </summary>
	void onRestartGame(){
		rigidbody.position = new Vector3 (13f, 0, -23f);
		curDir = Vector3.right;
		nextDir = Vector3.right;
	}

	/// <summary>
	/// Caled when a power up is piked up
	/// </summary>
	void onPowerUp(bool res){
		if(res){
			(GetComponent("Halo") as Behaviour).enabled = true;
		}
		else{
			(GetComponent("Halo") as Behaviour).enabled = false;
		}
	}

	/// <summary>
	/// Called when the scene is loaded
	/// </summary>
	/// <returns>void</returns>
	void Start () {
		pacGrid =new int[31,28]{
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
			{0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0},
			{0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
			{0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1, 0},
			{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		isMoving = true;
		EventManager.AddListener(EnumEvent.MINIGAME_LOST, onMiniGameLost);
		EventManager.AddListener(EnumEvent.MINIGAME_WIN, onMiniGameWin);
		EventManager.AddListener(EnumEvent.MINIGAME_TO, onMiniGameTO);
		EventManager.AddListener(EnumEvent.RESTARTSTATE, onRestartGame);
		EventManager<bool>.AddListener(EnumEvent.MOVING, moving);
		EventManager<bool>.AddListener(EnumEvent.FRIGHTENED, onPowerUp);

	}

	/// <summary>
	/// Called when the Event UP is raised
	/// </summary>
	public void goUp(){
		if (isValid(Vector3.forward)){
			nextDir = Vector3.forward;
		}
		else{
			nextDir = curDir;
		}
	}

	/// <summary>
	/// Called when the Event DOWN is raised
	/// </summary>
	public void goDown(){
		if (isValid(Vector3.back)){
			nextDir = Vector3.back;
		}
		else{
			nextDir = curDir;
		}
	}

	/// <summary>
	/// Called when the Event LEFT is raised
	/// </summary>
	public void goLeft(){
		if (isValid(Vector3.left)){
			nextDir = Vector3.left;
		}
		else{
			nextDir = curDir;
		}
	}

	/// <summary>
	/// Called when the Event RIGHT is raised
	/// </summary>
	public void goRight(){
		if (isValid(Vector3.right)){
			nextDir = Vector3.right;
		}
		else{
			nextDir = curDir;
		}
	}

	/// <summary>
	/// This is where the player can try to change its direction for another valid direction
	/// </summary>
	/// <returns>void</returns>
	void FixedUpdate () {
		if(isMoving){
			powerTimer += Time.deltaTime;
			move ();
		}
		else{
			rigidbody.velocity = Vector3.zero;
		}
	}
	

	/// <summary>
	/// Called whenever the player hits a special object.
	/// </summary>
	/// <param name="collider">The Collider of the object the player collides with.</param>
	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Ball"){
			Destroy(collider.gameObject);
		}
		else{
			obj = collider.gameObject;
			EventManager<GameObject>.Raise(EnumEvent.ENCOUNTER, collider.gameObject);
		}
	}

	/// <summary>
	/// Called when this script is destroyed
	/// </summary>
	void OnDestroy(){
		EventManager.RemoveListener(EnumEvent.MINIGAME_LOST, onMiniGameLost);
		EventManager.RemoveListener(EnumEvent.MINIGAME_WIN, onMiniGameWin);
		EventManager.RemoveListener(EnumEvent.MINIGAME_TO, onMiniGameTO);
		EventManager.RemoveListener(EnumEvent.RESTARTSTATE, onRestartGame);
		EventManager<bool>.RemoveListener(EnumEvent.MOVING, moving);
		EventManager<bool>.RemoveListener(EnumEvent.FRIGHTENED, onPowerUp);

	}
}