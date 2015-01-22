using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/********************************************************************************
 * Grid is meant to save the block's position. It checks if we have to delete a *
 * line and does it.                                                            *
 ********************************************************************************/
public class Grid : MonoBehaviour {
	
    // TO DO remove singleton aspect no need for that

	// Grid width and length
	public int w ;
	public int h ;
	
    // Level that defines the falling speed
    public int level;

    // Number of lines done
    public int nbLines;

    // Score
    public int score;
    public int fastFallScore;

    // Display
    [SerializeField]
    private Text guiScore;

    [SerializeField]
    private Text guiLines;

    [SerializeField]
    private Text guiLevel;

	// The grid that stocks all blocks positions.
	public Transform[,] grid;

    // The _grid is a singleton
    public static Grid _grid;

	// Use this for initialization
	void Start () 
	{			
		// Grid width and length
		w = 10;
		h = 25;

        initGrid();

		// Initialization of the grid that stocks all blocks positions.
		grid = new Transform[w, h];
		
	}

    private void initGrid()
    {
        fastFallScore = 0;
        level = 10;
        score = 0;
        nbLines = 0;
    }
	
	// Update is called once per frame
	void Update () {
            guiScore.text = score.ToString();
            guiLines.text = "Lines : \n" + nbLines;
            guiLevel.text = "Level : \n " + level;       
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
				(int)pos.y >= 0 &&
                (int)pos.y < h);
	}
	
	// Function that deletes a row;
	public void deleteRow(int y)
	{
		for (int x = 0; x < w; x++)
		{
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
		}
	}
	
    public void gameRestart()
    {
        for (int i = 0; i < _grid.h; i++)
        {
            deleteRow(i);
        }

        initGrid();
        Destroy(Tetromino.foreSeenTetromino.gameObject);
        Destroy(Tetromino.fallingTetromino.gameObject);
        // We set them to null manually because destroy (probably) isn't call at the current frame
        Tetromino.fallingTetromino = null;
        Tetromino.foreSeenTetromino = null;

        Spawner._spawner.spawnNext();
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
        score += fastFallScore;
        fastFallScore = 0;
        switch (nbDeleted)
        {
            case 1: 
                score += 40 * (level + 1);
                break;
            case 2: 
                score += 100 * (level + 1);
                break;
            case 3: 
                score += 300 * (level + 1);
                break;
            case 4: 
                score += 1200 * (level + 1);
                break;
            default: break;
        }
        nbLines += nbDeleted;
        // We use this for the level if we implement a way to start at lvl N instead of 0
        level =Mathf.Max (nbLines / 10, level);
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
