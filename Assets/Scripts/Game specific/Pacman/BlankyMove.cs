using UnityEngine;
using System.Collections;


/**
 * BlankyMove is a class which determines how the red enemy will move
 **/

/// <summary>
/// BlankyMove is a class which determines how the red enemy will move.
/// </summary>
/// 
public class BlankyMove : MonoBehaviour {
	/**
	 * Attributes
	 **/
	/// <summary>
	/// The player's GameObject
	/// </summary>
	GameObject pacman;

	/// <summary>
	/// The target during normal mode
	/// </summary>
	Vector3 blankyTarget;

	/// <summary>
	/// The target during scatter mode
	/// </summary>
	Vector3 blankyScatterTarget = new Vector3(0,0,0);

	/// <summary>
	/// The target once defeated
	/// </summary>
	Vector3 homeTarget = new Vector3(13, 0, -11);

	/// <summary>
	/// True is this enemy is in scatter mode
	/// </summary>
	bool scatterMode = false;

	/// <summary>
	/// The duration of scatter mode
	/// </summary>
	float scatterDelay = 10.0f;

	/// <summary>
	/// The time when the current scatter mode started
	/// </summary>
	float scatterTimer;

	/// <summary>
	/// True if this enemy is in frighten mode
	/// </summary>
	bool frightenMode = false;

	/// <summary>
	/// The duration of the frighten mode
	/// </summary>
	float frightenDelay = 5.0f;

	/// <summary>
	/// The time when the current frighten mode started
	/// </summary>
	float frightenTimer;


	/// <summary>
	/// True if this enemy has been defeated
	/// </summary>
	bool eaten = false;

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
	/// The x coordinate of the tile this enemy is.
	/// </summary>
	int curTileX;

	/// <summary>
	/// The y coordinate of the tile this enemy is
	/// </summary>
	int curTileY;

	/// <summary>
	/// True if this enemy is allowed to move.
	/// </summary>
	bool isMoving = false;

	/// <summary>
	/// The default color of this enemy
	/// </summary>
	Color defaultColor;

	/**
	 * Functions
	 **/

	/// <summary>
	/// True is this enemy is allowed to move.	/// </summary>
	/// <param name="real">True if the enemy is allowed to move.</param>
	/// <param name="tag">The tag of the GameObject.</param>
	/// <returns>void</returns>
	void moving(bool real, string tag){
		if (tag == gameObject.tag){
			isMoving = real;
		}
	}

	/// <summary>
	/// Activates the scatter mode
	/// </summary>
	/// <returns>void</returns>
	void scatter(){
		scatterMode = true;
		scatterTimer = Time.time;
	}

	/// <summary>
	/// Activates the frighten mode
	/// </summary>
	/// <returns>void</returns>
	void frightened(){
		frightenMode = true;
		frightenTimer = Time.time;
		renderer.material.color = Color.blue;
	}


	/*
	 * We check if the tile the player wants to go is a valid tile
	 * If the tile he is supposed to go is a valid tile (its value on the grid is 1), true is returned.
	 * Unlike pacman, the ghosts can't make a u-turn.
	 * If one of the valid direction would cause a ghost to make a u-turn te direction will be invalidated.
	 */
	/// <summary>
	/// Checks if the next tile is valid
	/// </summary>
	/// <returns><c>true</c>, if the tile is valid, <c>false</c> otherwise.</returns>
	/// <param name="next">The next direction.</param>
	bool isValid (Vector3 next){
		Vector3 predicted = transform.position + next;
		int predX = Mathf.RoundToInt(predicted.x);
		int predY = Mathf.RoundToInt(- predicted.z);
		if(pacGrid[predY,predX] >= 1 && next + curDir != Vector3.zero){
			return true;
		}
		return false;
	}


	/* 
	 * We check which direction is valid and store the valid direction in an array
	 * An invalid direction is given as a Vector3.zero.
	 * The array is four spaces long : 
	 *  - The first element is for up
	 *  - The second is for left
	 *  - The third is for down
	 *  - And the fourth is for right
	 */
	/// <summary>
	/// Check which directions will lead to a valid position.
	/// </summary>
	/// <returns>An array of directions.</returns>
	Vector3[] validDir(){
		Vector3[] validDirs = new Vector3[4];
		validDirs[0] = Vector3.zero;
		validDirs[1] = Vector3.zero;
		validDirs[2] = Vector3.zero;
		validDirs[3] = Vector3.zero;
		int i = 0;
		if (isValid(Vector3.forward)){
			validDirs[i] = Vector3.forward;
			++i;
		}
		if (isValid(Vector3.left)){
			validDirs[i] = Vector3.left;
			++i;
		}
		if (isValid(Vector3.back)){
			validDirs[i] = Vector3.back;
			++i;
		}
		if (isValid(Vector3.right)){
			validDirs[i] = Vector3.right;
			++i;
		}
		return validDirs;

	}


	/* After checking the valid directions, we choose the one that will make the ghost go closer to Pacman.
	 * If no direction has already been chosen, the first valid direction is picked.
	 * If there is already a chosen direction, we check if the next valid one is a better solution, if it is */

	/// <summary>
	/// Gives the next direction this enemy will go.
	/// </summary>
	/// <returns>The next direction.</returns>
	Vector3 getNextDir(){
		Vector3[] possibleDirs = validDir();
		Vector3 possibleDir = Vector3.zero;
		for(int i = 0; i < possibleDirs.Length; ++i){
			if (possibleDirs[i] != Vector3.zero){
				if (possibleDir == Vector3.zero){
					possibleDir = possibleDirs[i];
				}
				else if(Vector3.Distance(transform.position + possibleDir, blankyTarget) > Vector3.Distance(transform.position + possibleDirs[i], blankyTarget)){
					possibleDir = possibleDirs[i];
				}
			}
		}
		return possibleDir;
	}


	/*
	 * The ghost will only move if it reached the next tile.
	 * The ghost can only change its direction once pet tile but can't make a u-turn
	 */

	/// <summary>
	/// Makes the enemy move
	/// </summary>
	/// <returns>void</returns>
	void move()
	{
		if (Vector3.Distance(new Vector3(curTileX, 0, -curTileY), transform.position) <= 0.9f){

		}
		else {
			curDir = nextDir;
			curTileX = Mathf.RoundToInt(transform.position.x);
			curTileY = Mathf.RoundToInt(- transform.position.z);
		}

		if (frightenMode){
			rigidbody.position += curDir * 1.75f * Time.deltaTime;
		}
		else{
			rigidbody.position += curDir * 3.5f * Time.deltaTime;
		}
	}


	/*
	 * The ghost uses Blinky's behaviour : when chasing Pacman, its target point is pacman.
	 * When moving, the ghost is looking a tile ahead.
	 * It checks which direction is available for the next tile.
	 * If only one direction is valid, it will follow it.
	 * If multiples direction are available, it check which direction will make him go closer to its target.
	 * Then he moves*/

	/// <summary>
	/// Updates the target of the enemy and make it move
	/// </summary>
	/// <returns>void</returns>
	void chase(){

		if (frightenMode){
			int x = (int) Random.Range(0f,27f);
			int y = (int) Random.Range(0f, 30f);
			blankyTarget = new Vector3((float) x, 0, (float) y);
		}
		else if (scatterMode){
			blankyTarget = blankyScatterTarget;
		}
		else {
			blankyTarget = pacman.transform.position;
		}
		if (eaten){
			blankyTarget = homeTarget;
		}
		nextDir = getNextDir();
		move();
	}
	

	void sentenceWin(string tag){
		if (tag == gameObject.tag){
			if (frightenMode){
				eaten = true;
				frightenMode = false;
				renderer.material.color = defaultColor;
				collider.enabled = false;
				renderer.enabled = false;
				EventManager.Raise(EnumEvent.GHOST_EATEN);
			}
			else{
				EventManager.Raise(EnumEvent.GAMEOVER);
			}
		}
	}
	
	void sentenceTO(string tag){
		if (tag == gameObject.tag){
			if (!frightenMode){
				EventManager.Raise(EnumEvent.GAMEOVER);
			}
		}
	}

	void onRestartGame(){
		scatterMode = false;
		frightenMode = false;
		renderer.material.color = defaultColor;
		eaten = false;
		collider.enabled = true;
		renderer.enabled = true;
		rigidbody.position = new Vector3(13, 0, -11);
	}

	/// <summary>
	/// This is where attributes are initialised
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
			{1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1},
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
			{0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0},
			{0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
			{0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
			{0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
			{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1, 0},
			{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		EventManager<bool, string>.AddListener(EnumEvent.MOVING, moving);
		EventManager.AddListener(EnumEvent.SCATTERMODE, scatter);
		EventManager.AddListener(EnumEvent.FRIGHTENED, frightened);
		EventManager<string>.AddListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<string>.AddListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.AddListener(EnumEvent.RESTARTGAME, onRestartGame);


		pacman = GameObject.FindGameObjectWithTag("Pacman");
		blankyTarget = pacman.transform.position;
		curTileX = Mathf.RoundToInt(transform.position.x);
		curTileY = Mathf.RoundToInt(- transform.position.z);
		moving(true, gameObject.tag);
		defaultColor = renderer.material.color;
	}

	/// <summary>
	/// Updates the statuts of each mode as well as the enemy's target.
	/// </summary>
	/// <returns>void</returns>
	void FixedUpdate () {
		if (isMoving){
			chase();
			if (scatterMode && Time.time - scatterTimer > scatterDelay){
				scatterMode = false;
			}
			if (frightenMode && Time.time - frightenTimer > frightenDelay){
				frightenMode = false;
				renderer.material.color = defaultColor;
			}
			if (Vector3.Distance(transform.position, homeTarget) < 1f){
				eaten = false;
				collider.enabled = true;
				renderer.enabled = true;
			}
		}
	}
	void OnDestroy(){
		EventManager<bool, string>.RemoveListener(EnumEvent.MOVING,moving);
		EventManager.RemoveListener(EnumEvent.SCATTERMODE, scatter);
		EventManager.RemoveListener(EnumEvent.FRIGHTENED, frightened);
		EventManager<string>.RemoveListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<string>.RemoveListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.RemoveListener(EnumEvent.RESTARTGAME, onRestartGame);

	}
}
