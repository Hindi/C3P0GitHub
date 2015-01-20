﻿using UnityEngine;
using System.Collections;

public class PankyMove : MonoBehaviour {
	//We need to know where pacman is
	GameObject pacman;

	//And we need to know where he goes.
	PacMove pacMove;

	//Each ghost has specific target and a scatter target
	Vector3 pankyTarget;
	Vector3 pankyScatterTarget = new Vector3(28,0,2);
	bool scatterMode = true;

	Vector3 curDir = Vector3.right;
	Vector3 nextDir = Vector3.right;
	
	int[,] pacGrid;
	int curTileX;
	int curTileY;
	
	
	
	
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
				else if(Vector3.Distance(transform.position + possibleDir, pankyTarget) > Vector3.Distance(transform.position + possibleDirs[i], pankyTarget)){
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
		rigidbody.position += curDir * 3 * Time.deltaTime;
	}
	
	
	/*
	 * The ghost uses the Blinky behaviour : when chasing Pacman, its target point is the tile where pacman is WITH an offset four tile away in the direction Pacman is moving.
	 * When moving, the ghost is looking a tile ahead.
	 * It checks which direction is available for the next tile.
	 * If only one direction is valid, it will follow it.
	 * If multiples direction are available, it check which direction will make him go closer to its target.
	 * Then he moves*/
	
	void chase(){
		if(scatterMode){
			pankyTarget = pankyScatterTarget;
		}
		else {
			pankyTarget = pacman.transform.position + 4 * pacMove.getCurDir();
		}
		nextDir = getNextDir();
		move();
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
		
		pacman = GameObject.FindGameObjectWithTag("Pacman");
		pacMove = pacman.GetComponent<PacMove>();
		pankyTarget = pacman.transform.position + 4 * pacMove.getCurDir();
		curTileX = Mathf.RoundToInt(transform.position.x);
		curTileY = Mathf.RoundToInt(- transform.position.z);
	}
	
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.Space)){
			scatterMode = !scatterMode;
			//curDir = -curDir;
		}
		chase();
	}
}
