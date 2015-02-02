using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/********************************************************************************
 * Grid is meant to save the block's position. It checks if we have to delete a *
 * line and does it.                                                            *
 ********************************************************************************/

/// <summary>
/// Grid is meant to save and update almost all the data of the tetris game (score, lines, the block's position...). 
/// </summary>
public class Grid : MonoBehaviour {

	// Grid width and length

    /// <summary>
    /// The number of coloms of the tetris board grid.
    /// </summary>
	public int w ;

    /// <summary>
    /// The number of rows of the tetris board grid.
    /// </summary>
	public int h ;
	
    /// <summary>
    /// Level of the game that defines the falling speed and points multiplicator.
    /// </summary>
    public int level;

    /// <summary>
    /// Number of lines done this party.
    /// </summary>
    public int nbLines;


    /// <summary>
    /// Total score of the player this party.
    /// </summary>
    public int score;

    /// <summary>
    /// Save the bonus score obtained when we fast fall a tetromino.
    /// </summary>
    public int fastFallScore;

    // Display fields

    /// <summary>
    /// The Text that displays score for PC (emplacement depends of the platform)
    /// </summary>
    [SerializeField]
    private Text guiScorePC;



    /// <summary>
    /// The Text that displays the number of lines done for PC (emplacement depends of the platform)
    /// </summary>
    [SerializeField]
    private Text guiLinesPC;


    /// <summary>
    /// The Text that displays the level for PC (emplacement depends of the platform)
    /// </summary>
    [SerializeField]
    private Text guiLevelPC;


    /// <summary>
    /// The Text that displays score for mobile platforms (emplacement depends of the platform)
    /// </summary>
    [SerializeField]
    private Text guiScoreMobile;

    /// <summary>
    /// The Text that displays the number of lines done for mobile platforms (emplacement depends of the platform)
    /// </summary>
    [SerializeField]
    private Text guiLinesMobile;

    /// <summary>
    /// The Text that displays the level for mobile platforms (emplacement depends of the platform)
    /// </summary>
    [SerializeField]
    private Text guiLevelMobile;

    /// <summary>
    /// Those Text are used during update and refers wether to a Text for PC or mobile of the same name.
    /// </summary>
    private Text guiLevel, guiLines, guiScore;

    // Use for easier tests for mobile UI
    /// <summary>
    /// Used for easier tests on mobile platforms
    /// </summary>
    private bool isMobile;

    // Display scaling
    /// <summary>
    /// Floats that represent the resizing ratio for the current resolution.
    /// </summary>
    public float xScale, yScale;

	
    /// <summary>
    /// The grid that stocks all blocks positions.
    /// </summary>
	public Transform[,] grid;

    // The _grid is a singleton
    /// <summary>
    /// _grid is used to make a singleton.
    /// </summary>
    public static Grid _grid;

	// Use this for initialization
    /// <summary>
    /// Called when a Grid element is instantiate.
    /// We use it for initialisation.
    /// </summary>
    /// <returns>void</returns>
	void Start () 
	{			
		// Grid width and length
		w = 10;
		h = 25;


        isMobile = Application.isMobilePlatform;
        
        // Portrait mode can be loaded after this initialisation
        // Thus we make sure that good resolution are taken into account
        float wi, he;
        if (Screen.width > Screen.height) 
        {
            wi = (float)Screen.height;
            he = (float)Screen.width;
        }
        else 
        {
            wi = (float)Screen.width;
            he = (float)Screen.height;
        }

        if (isMobile)
        {
            float temp = wi/he;
            xScale = 1.156f * temp / (2f / 3f);
            yScale = 0.88f;
            guiLevel = guiLevelMobile;
            guiScore = guiScoreMobile;
            guiLines = guiLinesMobile;
        }
        else
        {
            xScale = 1f;
            yScale = 1f;

            guiLevel = guiLevelPC;
            guiScore = guiScorePC;
            guiLines = guiLinesPC;
        }
        initGrid();

		// Initialization of the grid that stocks all blocks positions.
		grid = new Transform[w, h];
		
	}

    /// <summary>
    /// Called to initialise score, the number of lines done and the level.
    /// </summary>
    /// <returns>void</returns>
    private void initGrid()
    {
        fastFallScore = 0;
        level = 3;
        score = 0;
        nbLines = 0;
    }
	
	// Update is called once per frame
    /// <summary>
    /// Update is called once per frame. 
    /// We update all the Text we display (score, number of  lines and level).
    /// </summary>
    /// <returns>void</returns>
	void Update () 
    {
        // Score update
        guiScore.text = score.ToString();
        guiLines.text = "Lines : \n" + nbLines;
        guiLevel.text = "Level : \n " + level; 
	}
	


	
	
	// A helper function that is used to round positions (cause rotations might
	// create some small errors that could stack
    /// <summary>
    /// A helper function that is used to round positions (cause rotations might
    /// create some small errors that could stack. 
    /// </summary>
    /// <param name="v">The position (Vector 2) we want to round.</param>
    /// <returns>Vector2</returns>
	public Vector2 roundVec2(Vector2 v) 
	{
		return new Vector2(Mathf.Round(v.x),
						   Mathf.Round(v.y));
	}
	
	
	// A function that checks if the tetrominos is inside the borders
    /// <summary>
    /// Called at every movement of a tetromino to determine if it is inside the borders.
    /// It checks if the tetrominos is inside the borders. 
    /// </summary>
    /// <param name="pos">The position we check</param>
    /// <returns>boolean
    /// True if the tetromino is still inside the board boarders
    /// False otherwise.
    /// </returns>
	public bool isInsideBorders(Vector2 pos)
	{
		return ((int)pos.x >= 0 && 
				(int)pos.x < w &&
				(int)pos.y >= 0 &&
                (int)pos.y < h);
	}
	
    /// <summary>
    /// Called when we want to delete a row.
    /// Mostly when the row is full and has to be deleted.
    /// </summary>
    /// <param name="y">The number of the row we want to delete.
    /// 0 is the bottom of the grid.
    /// </param>
    /// <returns>void</returns>
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
	
    /// <summary>
    /// Called on restart
    /// </summary>
    /// <returns>void</returns>
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
	
    /// <summary>
    /// Called when a row has been deleted. Decrease the row number y by one.
    /// </summary>
    /// <param name="y">Number of the row from which we want to decrease</param>
    /// <returns>void</returns>
	public void decreaseRow(int y)
	{
		for (int x = 0; x < w; x++)
		{
			if(grid[x,y] != null)
			{
				// Update the block position
				grid[x,y].position += new Vector3(0, -1 * yScale, 0);

				// Update the grid by moving down the one at pos x,y
				grid[x, y-1] = grid[x, y];
				grid[x,y] = null;		
			}
		}
	}

    /// <summary>
    /// When a row is deleted we have to move down all the blocks above by one.
    /// </summary>
    /// <param name="p">We decrease all the row above p by one (p included)</param>
	public void decreaseRowsAbove(int p)
	{
		for (int y = p; y < h; y++)
		{
			decreaseRow(y);
		}
	}
	
	// Function that returns true if the row y is full and false otherwise
    /// <summary>
    /// Called to check if the row number y is full.
    /// </summary>
    /// <param name="y">Number of the row we want to check</param>
    /// <returns>boolean
    /// true if the row is full
    /// false otherwise.</returns>
	public bool isRowFull(int y)
	{
		for (int x = 0; x < w; x++)
        {
			if (grid[x, y] == null)
				return false;
		}
		return true;
	}

	/// <summary>
	/// Function called when a tetromino has been placed at height y 
    /// (his highest part is at height y). We delete the full rows he might 
	/// have completed (so we only have to check the 4 rows below) .
    /// If some are deleted we move down the rows above.
    /// </summary>
    /// <param name="y">Highest row from where we check if we have to delete this row.</param>
    /// <returns>void</returns>
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
	
	
    /// <summary>
    /// Called just before Start(). Here we handle the singleton aspect.
    /// </summary>
    /// <returns>void</returns>
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
