using UnityEngine;
using System.Collections;


/**
 * AnkyMove is a class which determines how the Blue enemy will move
 **/

/// <summary>
/// AnkyMove is a class which determines how the blue enemy will move.
/// </summary>
/// 
public class AnkyMove : MonoBehaviour {
	/// <summary>
	/// The player's GameObject
	/// </summary>
	GameObject pacman;

	/// <summary>
	/// The red ghost GameObject
	/// </summary>
	GameObject blanky;

	/// <summary>
	/// The player's script
	/// </summary>
	PacMove pacMove;

	/// <summary>
	/// The target during normal mode
	/// </summary>
	Vector3 ankyTarget;

	/// <summary>
	/// The target of the red ghost
	/// </summary>
	Vector3 ankyScatterTarget = new Vector3(28,0,-32);

	/// <summary>
	/// The target once defeated
	/// </summary>
	Vector3 homeTarget = new Vector3(13, 0, -11);

	/// <summary>
	/// True if in scatter
	/// </summary>
	bool scatterMode = false;

	/// <summary>
	/// True if in frighten mode
	/// </summary>
	bool frightenMode = false;

	/// <summary>
	/// True if defeated
	/// </summary>
	bool eaten = false;

	/// <summary>
	/// The current direction
	/// </summary>
	Vector3 curDir = Vector3.right;

	/// <summary>
	/// The next direction
	/// </summary>
	Vector3 nextDir = Vector3.right;

	/// <summary>
	/// The tile representation of the maze.
	/// </summary>
	/// [i, j] is the tile located on the ith row (from top to bottom) and jth column (from left to right).
	/// For the game graphics, a tile i a 1x1 square and and its coordinates are (j, -i)
	/// Each number represents a tile :
	/// - 0 is a dead tile, neither the player nor the enemy AIs can be located in a dead tile at any time.
	/// - 1 or higher is a legal tile, both players and enemies can travel between those tiles.
	int[,] pacGrid;

	/// <summary>
	/// The x coordinate of the tile this enemy is
	/// </summary>
	int curTileX;

	/// <summary>
	/// The y coordinate of the tile this enemy is
	/// </summary>
	int curTileY;

	/// <summary>
	/// True if movement is allowed
	/// </summary>
	bool isMoving = false;

	/// <summary>
	/// True if inside the spawn zone
	/// </summary>
	bool inHouse = true;

	/// <summary>
	/// The default color
	/// </summary>
	Color defaultColor;

	/// <summary>
	/// The halo of the object
	/// </summary>
	Behaviour halo;


	/// <summary>
	/// Allows movement
	/// </summary>
	/// <param name="real">If set to <c>true</c>, movement is allowed.</param>
	/// <param name="tag">The tage of the GameObject.</param>
	void moving(bool real, string tag){
		if (tag == gameObject.tag){
			isMoving = real;
			inHouse = false;
		}
	}

	/// <summary>
	/// Allows movement
	/// </summary>
	/// <param name="res">If set to <c>true</c>, movement is allowed.</param>
	void moving(bool res){
		isMoving = res;
	}

	/// <summary>
	/// Starts scatter mode.
	/// </summary>
	void scatter(bool res){
		scatterMode = res;
	}
	/// <summary>
	/// Starts frighten mode.
	/// </summary>
	void frightened(bool res){
		if(!eaten){
			if (res){
				frightenMode = res;
				renderer.material.color = Color.blue;
				renderer.enabled = true;
				halo.enabled = false;
			}
			else {
				frightenMode = false;
				renderer.material.color = defaultColor;
				renderer.enabled = false;
				halo.enabled = true;
			}
		}
	}
	
	/*
	 * We check if the tile the player wants to go is a valid tile
	 * If the tile he is supposed to go is a valid tile (its value on the grid is 1), true is returned.
	 * Unlike pacman, the ghosts can't make a u-turn.
	 * If one of the valid direction would cause a ghost to make a u-turn te direction will be invalidated.
	 */
	/// <summary>
	/// Check if next is a valid direction.
	/// </summary>
	/// <returns><c>true</c>, if next is a valid direction, <c>false</c> otherwise.</returns>
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
	/// Gives the possible directions.
	/// </summary>
	/// <returns>The array of possible directions.</returns>
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
	/// Get the next possible direction.
	/// </summary>
	/// <returns>The next dir.</returns>
	Vector3 getNextDir(){
		Vector3[] possibleDirs = validDir();
		Vector3 possibleDir = Vector3.zero;
		for(int i = 0; i < possibleDirs.Length; ++i){
			if (possibleDirs[i] != Vector3.zero){
				if (possibleDir == Vector3.zero){
					possibleDir = possibleDirs[i];
				}
				else if(Vector3.Distance(transform.position + possibleDir, ankyTarget) > Vector3.Distance(transform.position + possibleDirs[i], ankyTarget)){
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
	/// Makes the blue ghost moving
	/// </summary>
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
			transform.position += curDir * 1.5f * Time.deltaTime;
		}
		else {
			transform.position += curDir * 3 * Time.deltaTime;
		}
	}
	
	
	/*
	 * The ghost uses the Inky behaviour : when chasing Pacman, its target point is the following : 
	 *  - We must first establish an intermediate offset two tiles in front of Pac-Man in the direction he is moving
	 *  - Now imagine drawing a vector from the center of Blanky's current tile to the center of the offset tile
	 *  - Then double the vector length by extending it out just as far again beyond the offset tile.
	 *  - The tile this new, extended vector points to is Anky's actual target
	 * When moving, the ghost is looking a tile ahead.
	 * It checks which direction is available for the next tile.
	 * If only one direction is valid, it will follow it.
	 * If multiples direction are available, it check which direction will make him go closer to its target.
	 * Then he moves*/
	/// <summary>
	/// Updates the ankyTarget
	/// </summary>
	void chase(){

		if (frightenMode){
			int x = (int) Random.Range(0f,27f);
			int y = (int) Random.Range(0f, 30f);
			ankyTarget = new Vector3((float) x, 0, (float) y);
		}
		else if (scatterMode){
			ankyTarget = ankyScatterTarget;
		}
		else {
			ankyTarget = (pacman.transform.position + 2 * pacMove.getCurDir()) + (pacman.transform.position + 2 * pacMove.getCurDir() - blanky.transform.position);
		}
		if (eaten){
			ankyTarget = homeTarget;
		}
		nextDir = getNextDir();
		move();
	}


	/// <summary>
	/// Called when the player wins the mini-game and this ghost is his encounter
	/// </summary>
	/// <param name="obj">The gameObject of the encounter.</param>
	void sentenceWin(GameObject obj){
		if (frightenMode){
			renderer.enabled = true;
		}
		else {
			renderer.enabled = false;
			halo.enabled = true;
		}
		if (obj == gameObject){
			if (frightenMode){
				eaten = true;
				frightenMode = false;
				renderer.material.color = defaultColor;
				collider.enabled = false;
				renderer.enabled = false;
				halo.enabled = false;
				EventManager.Raise(EnumEvent.GHOST_EATEN);
			}
			else{
				EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
			}
		}
	}
	
	/// <summary>
	/// Called when the time is over and this ghost is his encounter
	/// </summary>
	/// <param name="obj">The gameObject of the encounter.</param>
	void sentenceTO(GameObject obj){
		if (frightenMode){
			renderer.enabled = true;
		}
		else {
			renderer.enabled = false;
			halo.enabled = true;
		};
		if (obj == gameObject){
			if (!frightenMode){
				EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
			}
		}
	}

	/// <summary>
	/// Called when the player loses the mini-game and this ghost is his encounter
	/// </summary>
	/// <param name="obj">The gameObject of the encounter.</param>
	void sentenceLost(GameObject obj){
		if (frightenMode){
			renderer.enabled = true;
		}
		else {
			frightenMode = false;
			renderer.material.color = defaultColor;
			renderer.enabled = false;
			halo.enabled = true;
		}
	}

	/// <summary>
	/// Called when the mini-game starts
	/// </summary>
	void onStartMiniGame(){
		renderer.enabled = false;
		halo.enabled = true;
	}

	/// <summary>
	/// Called when the game restarts
	/// </summary>
	void onRestartGame(){
		scatterMode = false;
		frightenMode = false;
		renderer.material.color = defaultColor;
		eaten = false;
		collider.enabled = true;
		renderer.enabled = false;
		halo.enabled = true;
		transform.position = new Vector3(11, 0, -14);
		inHouse = true;
		curDir = Vector3.left;
		nextDir = Vector3.left;
	}

	/// <summary>
	/// Called when the scene loads
	/// </summary>
	void Start () {
		
		/*
		 * The following grid represents the tile map of the level
		 * [i, j] is the tile located on the ith row (from top to bottom) and jth column (from left to right).
		 * For the game graphics, a tile i a 1x1 square and and its coordinates are [j, -i]
		 * Each number represents a tile :
		 * - 0 is a dead tile, neither the player nor the enemy AIs can be located in a dead tile at any time.
		 * - 1 is a legal tile, both players and enemies can travel between those tiles
		 */
		
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
		pacman = GameObject.FindGameObjectWithTag("Player");
		blanky = GameObject.FindGameObjectWithTag("Blanky");
		pacMove = pacman.GetComponent<PacMove>();
		ankyTarget = (pacman.transform.position + 2 * pacMove.getCurDir()) + (pacman.transform.position + 2 * pacMove.getCurDir() - blanky.transform.position);
		halo = GetComponent("Halo") as Behaviour;
		curTileX = Mathf.RoundToInt(transform.position.x);
		curTileY = Mathf.RoundToInt(- transform.position.z);
		defaultColor = renderer.material.color;

		EventManager<bool, string>.AddListener(EnumEvent.MOVING, moving);
		EventManager<bool>.AddListener(EnumEvent.SCATTERMODE, scatter);
		EventManager<bool>.AddListener(EnumEvent.FRIGHTENED, frightened);
		EventManager<GameObject>.AddListener(EnumEvent.SENTENCE_LOST, sentenceLost);
		EventManager<GameObject>.AddListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<GameObject>.AddListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.AddListener(EnumEvent.RESTARTSTATE, onRestartGame);
		EventManager<bool>.AddListener(EnumEvent.MOVING, moving);
		EventManager.AddListener(EnumEvent.MINIGAME_START, onStartMiniGame);
	}

	/// <summary>
	/// Caled after a fixed amount of time
	/// </summary>
	void FixedUpdate () {
		if (isMoving && !inHouse){
			chase();
			if (isMoving){
			}
		}
		if (Vector3.Distance(transform.position, homeTarget) < 1f){
			if(eaten){
				eaten = false;
				renderer.material.color = defaultColor;
				collider.enabled = true;
				renderer.enabled = false;
				halo.enabled = true;
				frightened(false);
			}
		}
	}

	/// <summary>
	/// Called when this scrip is destroyed
	/// </summary>
	void OnDestroy(){
			EventManager<bool, string>.RemoveListener(EnumEvent.MOVING,moving);
			EventManager<bool>.RemoveListener(EnumEvent.SCATTERMODE, scatter);
			EventManager<bool>.RemoveListener(EnumEvent.FRIGHTENED, frightened);
			EventManager<GameObject>.RemoveListener(EnumEvent.SENTENCE_LOST, sentenceLost);
			EventManager<GameObject>.RemoveListener(EnumEvent.SENTENCE_TO, sentenceTO);
			EventManager<GameObject>.RemoveListener(EnumEvent.SENTENCE_WIN, sentenceWin);
			EventManager.RemoveListener(EnumEvent.RESTARTSTATE, onRestartGame);
			EventManager<bool>.RemoveListener(EnumEvent.MOVING, moving);
			EventManager.RemoveListener(EnumEvent.MINIGAME_START, onStartMiniGame);
	}
}
