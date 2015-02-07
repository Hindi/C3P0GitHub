﻿using UnityEngine;
using System.Collections;

public class AnkyMove : MonoBehaviour {
	GameObject pacman;
	GameObject blanky;
	PacMove pacMove;
	Vector3 ankyTarget;
	Vector3 ankyScatterTarget = new Vector3(28,0,-32);
	Vector3 homeTarget = new Vector3(13, 0, -11);
	bool scatterMode = false;
	float scatterDelay = 10.0f;
	float scatterTimer;
	bool frightenMode = false;
	float frightenDelay = 5.0f;
	float frightenTimer;
	bool eaten = false;
	Vector3 curDir = Vector3.right;
	Vector3 nextDir = Vector3.right;
	int[,] pacGrid;
	int curTileX;
	int curTileY;
	bool isMoving = false;
	bool inHouse = true;
	Color defaultColor;
	
	void moving(bool real, string tag){
		if (tag == gameObject.tag){
			isMoving = real;
			inHouse = false;
		}
	}

	void moving(bool res){
		isMoving = res;
	}

	void scatter(){
		scatterMode = true;
		scatterTimer = 0f;
	}
	
	void frightened(){
		if(!eaten){
			frightenMode = true;
			frightenTimer = 0f;
			renderer.material.color = Color.blue;
		}
	}
	
	/*
	 * We check if the tile the player wants to go is a valid tile
	 * If the tile he is supposed to go is a valid tile (its value on the grid is 1), true is returned.
	 * Unlike pacman, the ghosts can't make a u-turn.
	 * If one of the valid direction would cause a ghost to make a u-turn te direction will be invalidated.
	 */
	
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
	 * The ghost uses the Blinky behaviour : when chasing Pacman, its target point is the following : 
	 *  - We must first establish an intermediate offset two tiles in front of Pac-Man in the direction he is moving
	 *  - Now imagine drawing a vector from the center of Blanky's current tile to the center of the offset tile
	 *  - Then double the vector length by extending it out just as far again beyond the offset tile.
	 *  - The tile this new, extended vector points to is Anky's actual target
	 * When moving, the ghost is looking a tile ahead.
	 * It checks which direction is available for the next tile.
	 * If only one direction is valid, it will follow it.
	 * If multiples direction are available, it check which direction will make him go closer to its target.
	 * Then he moves*/
	
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
				EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
			}
		}
	}

	void sentenceTO(string tag){
		if (tag == gameObject.tag){
			if (!frightenMode){
				EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
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
		transform.position = new Vector3(11, 0, -14);
		inHouse = true;
		curDir = Vector3.left;
		nextDir = Vector3.left;
	}

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
		curTileX = Mathf.RoundToInt(transform.position.x);
		curTileY = Mathf.RoundToInt(- transform.position.z);
		defaultColor = renderer.material.color;

		EventManager<bool, string>.AddListener(EnumEvent.MOVING,moving);
		EventManager.AddListener(EnumEvent.SCATTERMODE, scatter);
		EventManager.AddListener(EnumEvent.FRIGHTENED, frightened);
		EventManager<string>.AddListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<string>.AddListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.AddListener(EnumEvent.RESTARTSTATE, onRestartGame);
		EventManager<bool>.AddListener(EnumEvent.MOVING, moving);



	}
	
	void FixedUpdate () {

		if (isMoving && !inHouse){
			chase();
			scatterTimer += Time.deltaTime;
			frightenTimer += Time.deltaTime;
		}
			if (scatterMode && scatterTimer > scatterDelay){
				scatterMode = false;
			}
			if (frightenMode && frightenTimer > frightenDelay){
				frightenMode = false;
				renderer.material.color = defaultColor;
			}
			if (Vector3.Distance(transform.position, homeTarget) < 1f){
				eaten = false;
				collider.enabled = true;
				renderer.enabled = true;
			}
	}

	void OnDestroy(){
		EventManager<bool, string>.RemoveListener(EnumEvent.MOVING,moving);
		EventManager.RemoveListener(EnumEvent.SCATTERMODE, scatter);
		EventManager.RemoveListener(EnumEvent.FRIGHTENED, frightened);
		EventManager<string>.RemoveListener(EnumEvent.SENTENCE_TO, sentenceTO);
		EventManager<string>.RemoveListener(EnumEvent.SENTENCE_WIN, sentenceWin);
		EventManager.RemoveListener(EnumEvent.RESTARTSTATE, onRestartGame);
		EventManager<bool>.RemoveListener(EnumEvent.MOVING, moving);
	}
}
