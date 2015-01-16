﻿using UnityEngine;
using System.Collections;


/********************************************************************************
 * Grid is meant to save the block's position. It checks if we have to delete a *
 * line and does it.                                                            *
 ********************************************************************************/
public class Grid : MonoBehaviour {
		
	// Grid width and length
	public int w ;
	public int h ;
	
	// The grid that stocks all blocks positions.
	public Transform[,] grid;

    // The _grid is a singleton
    public static Grid _grid;

	// Use this for initialization
	void Start () 
	{			
		// Grid width and length
		w = 10;
		h = 20;
		
		// Initialization of the grid that stocks all blocks positions.
		grid = new Transform[w, h];
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	


	
	
	// A helper function that is used to round positions (cause rotations might
	// create some small errors that could stack
	public Vector2 roundVec2(Vector2 v) 
	{
		return new Vector2(Mathf.Round(v.x),
						   Mathf.Round(v.y));
	}
	
	
	// A function that checks if the tetrominos is inside the borders
	public bool isInsideBorders(Vector2 pos)
	{
		return ((int)pos.x >= 0 && 
				(int)pos.x < w &&
				(int)pos.y >= 0);
	}
	
	// Function that deletes a row;
	public void deleteRow(int y)
	{
		for (int x = 0; x < w; x++)
		{
			Destroy(grid[x, y].gameObject);
			grid[x,y] = null;
		}
	}
	
	
	// This function decreases the row at height y by nbDecrease 
	public void decreaseRow(int y)
	{
		for (int x = 0; x < w; x++)
		{
			if(grid[x,y] != null)
			{
				// Update the block position
				grid[x,y].position += new Vector3(0,-1,0);

				// Update the grid by moving down the one at pos x,y
				grid[x, y-1] = grid[x, y];
				grid[x,y] = null;		
			}
		}
	}

	// When rows are deleted we have to move down all the blocks above by the number
	// of deleted rows
	public void decreaseRowsAbove(int p)
	{
		for (int y = p; y < h; y++)
		{
			decreaseRow(y);
		}
	}
	
	// Function that returns true if the row y is full and false otherwise
	public bool isRowFull(int y)
	{
		for (int x = 0; x < w; x++)
        {
			if (grid[x, y] == null)
				return false;
		}
		return true;
	}
	
	// Function called when a tetromino has been placed at height y 
    // (his highest part is at height y). We delete the full rows he might 
	// have completed (so we only have to check the 4 rows below) 
    // If some are deleted we move down the rows above
	public void deleteFullRowsFrom(int y)
	{
		int nbDeleted = 0;
		for (int i = y; (i > y-4) && (i >= 0); i--)
		{
			if (isRowFull(i))
			{
				deleteRow(i);
                decreaseRowsAbove(i + 1);
				nbDeleted++;
			}
		}
        // if(nbDeleted != 0)
		// TO DO Augmenter les points correspondants
	}
	
	
	void Awake()
	{
		if (_grid != null)
		{
			Debug.Log("There should only be one Grid instance");
		}
		else 
		{
			_grid = this;
		}
	}
}
