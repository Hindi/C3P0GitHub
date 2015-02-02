using UnityEngine;
using System.Collections;
/*********************************************************************************** 
 * This class handles Tetrominos (foreseen, movements...)                          *
 ***********************************************************************************/


/// <summary>
/// Class that handles Tetrominos (scale, position, movements).
/// </summary>
public class Tetromino : MonoBehaviour {
    /// <summary>
    /// Time since last fall.
    /// </summary>
	private float timeSinceLastFall;
	
    /// <summary>
	/// Time between two "natural" falls of the tetromino.
    /// </summary>
	private float fallingSpeed;

    /// <summary>
    /// "Timers" not to move left, right, or down at every frames when we hold the key down
    /// A movement every movingRate frames.
    /// </summary>
    private float moveRightTimer, moveLeftTimer, moveDownTimer, movingRate, movingDownRate;
	
	/// <summary>
    /// Place where the tetromino will spawn on the board (depends of the platform).
	/// </summary>
	private Vector3 spawnPosition;

    /// <summary>
    /// Use for easier tests for mobile UI.
    /// </summary>
    private bool isMobile;
    
	/// <summary>
	/// Reference to the falling tetromino.
	/// </summary>
	public static Tetromino fallingTetromino = null ;

    /// <summary>
    /// Reference to the foreseen tetromino.
    /// </summary>
	public static Tetromino foreSeenTetromino = null ;


    /// <summary>
    /// A boolean to prevent Tetromino O (square) to rotate.
    /// </summary>
    [SerializeField]
    private bool canRotate;



	// Use this for initialization
    /// <summary>
    /// Called when a Tetromino is created and initializes it (falling speed, movement rate, scale, ...).
    /// </summary>
    /// <returns>void</returns>
	void Start () {		
		// Timer to make a tetromino falls also set on creation because the first one is never foreseen.
		timeSinceLastFall = Time.time;
		
        // Falling speed, increases with the lvl
		fallingSpeed = 1.0f / (Grid._grid.level + 1);
		

        // movingRate initialization
        // Thus we can go left or right 12 times per second
        movingRate = 1.0f / 12;
        // Thus we can do a fast fall that will go down 30 times/sec
        movingDownRate = 1.0f / 30;

        // Timers initialization
        moveDownTimer = Time.time;
        moveRightTimer = Time.time;
        moveLeftTimer = Time.time;

        isMobile = Application.isMobilePlatform;
		
        // Initialisation for scaling (not 1 if mobile)
        // Initialisation for spawnPosition
        if (isMobile)
        {
            // Place where a tetromino will spawn
            spawnPosition = new Vector3((-4.5f + 4f) * Grid._grid.xScale, 19f * Grid._grid.yScale, 0);
        }
        else
        {
            // Place where a tetromino will spawn
            spawnPosition = new Vector3(5f * Grid._grid.xScale, 19f * Grid._grid.yScale, 0);
        }



        

		// This block handles wether the tetromino is falling in the board or is 
		// just foreseen
		
		// First case should happen only when the game starts
		if (fallingTetromino == null && foreSeenTetromino == null)
		{
			fallingTetromino = this;
			fallingTetromino.transform.position = spawnPosition;


            // Size scaling differs if it's for mobile or computer and if it's foreseen or falling
            transform.localScale = new Vector3(Grid._grid.xScale, Grid._grid.yScale, 1);
			
			// We create the foreseen tetromino
			Spawner._spawner.spawnNext();
		}
		else if (foreSeenTetromino == null)
		{
			foreSeenTetromino = this;
            if (isMobile)
                transform.localScale = new Vector3(0.4f * Grid._grid.xScale, 0.6f * Grid._grid.yScale, 1f);//foreSeenScaleMobile;
            else
                transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		}
	}
	
	// Update is called once per frame
    /// <summary>
    /// Handles automatic drop and automatic delete when the tetromino doesn't have any child left.
    /// </summary>
    /// <returns>void</returns>
	void Update () 
	{
        // Automatic move down handled here
		if (this == fallingTetromino)
        {
            if ((Time.time - timeSinceLastFall) >= fallingSpeed)
            {
                goDown();
                timeSinceLastFall = Time.time;
            }
		}
        // When a tetromino doesn't have any child anymore he is deleted
        if (transform.childCount == 0)
            Destroy(this.gameObject);
          
	}
	


	/*******************************************************************************
	 *                          Movement functions                                 *
	 *******************************************************************************/
    /// <summary>
    /// Move the tetromino to the left if allowed.
    /// Note : called only for the falling tetromino.
    /// </summary>
    /// <returns>void</returns>
	public void moveLeft()
	{
        // First we check if we can move left this frame
        if (Time.time - moveLeftTimer >= movingRate)
        {
            // We move the tetromino to the left
            transform.position += new Vector3(-1*Grid._grid.xScale, 0, 0);

            // See if it's still a valid position
            if (!isValidGridPos())
                // Its not valid. revert.
                transform.position += new Vector3(1 * Grid._grid.xScale, 0, 0);
            moveLeftTimer = Time.time;
        }
		
	}

    /// <summary>
    /// Move the tetromino to the right if allowed.
    /// Note : called only for the falling tetromino.
    /// </summary>
    /// <returns>void</returns>
    public void moveRight()
	{
        // Check if we can move to the right this frame
        if (Time.time - moveRightTimer >= movingRate)
        {
            // We move the tetromino to the right
            transform.position += new Vector3(1 * Grid._grid.xScale, 0, 0);

            // See if it's still a valid position
            if (!isValidGridPos())
                // Its not valid. revert.
                transform.position += new Vector3(-1 * Grid._grid.xScale, 0, 0);

            // We set the timer 
            moveRightTimer = Time.time;
        }
	}


    // goDown handles grid updates if the tetromino is set
    /// <summary>
    /// Move the tetromino down by one if allowed. If the movement can't be done. The tetromino is set
    /// and the next one begins to fall (rows are deleted if necessary and grid is updated).
    /// Note : called only for the falling tetromino.
    /// </summary>
    /// <returns>void</returns>
    public void goDown()
    {
		// We move the tetromino to the bottom
        transform.position += new Vector3(0, -1 * Grid._grid.yScale, 0);
        
		// See if it's still a valid position
		if (!isValidGridPos())
		{
			// Its not valid. revert.
            transform.position += new Vector3(0, 1 * Grid._grid.yScale, 0);
			// The tetromino is set and can't move anymore
			// So we update the grid and delete rows if they are full (done in updateGrid)
			updateGrid();
			// Then the next tetromino appears
			nextFalling();
		}
	}

    /// <summary>
    /// Called when the player activates the fast fall (pressing down arrow on computer for example)
    /// </summary>
    /// <returns>void</returns>
    public void moveDown()
    {
        // Check if we can move down this frame (thanks to input)
        if (Time.time - moveDownTimer >= movingDownRate)
        {
            goDown();
            // Reset of the timer to move down
            moveDownTimer = Time.time;
            timeSinceLastFall = Time.time;
            Grid._grid.fastFallScore++;
        }
    }

    /// <summary>
    /// Called when the player rotates the falling tetromino. Makes the falling tetromino rotates by 90°.
    /// If the tetromino rotation should be invalid, try to move the tetromino to the left or the right to rotate.
    /// </summary>
    /// <returns>void</returns>
    public void rotate()
	{
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
            // See if valid
            if (!isValidGridPos())
            {
                if (!tryTranslate())
                {
                    // It's not valid, revert
                    transform.Rotate(0, 0, 90);
                    transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
                }
            }               
        }
	}
	

    /// <summary>
    /// Called when a rotation is not immediatly possible we try to move the tetromino to the left/right to do the rotation
    /// We only try it for a translation by 3 on the left, then to the right
    /// </summary>
    /// <returns>void</returns>
    bool tryTranslate()
    {
        int nbTry=0;
        Vector3 originPos = transform.position;
        while (!isValidGridPos() && nbTry < 3)
        {
            transform.position += new Vector3(-1 * Grid._grid.xScale,0,0);
            nbTry++;
        }
        if (nbTry < 3)
            return true;
        nbTry = 0;
        transform.position = originPos;
        while(!isValidGridPos() && nbTry < 3)
        {
            transform.position += new Vector3(1 * Grid._grid.xScale, 0, 0);
            nbTry++;
        }
        if (nbTry < 3)
            return true;
        transform.position = originPos;
        return false;
    }


    /// <summary>
    /// Function that converts position into grid position
    /// </summary>
    /// <param name="pos">The position we want to convert</param>
    /// <returns>Vector2 the position in the grid (colomn, rows)</returns>
    Vector2 convertPos(Vector2 pos)
    {
        if (isMobile)
        {
            return (new Vector2((pos.x + 4.5f * Grid._grid.xScale) / Grid._grid.xScale, (pos.y) / Grid._grid.yScale));
        }
        else
            return pos;
    }

    /// <summary>
    /// Function that checks if the position of all the child blocks of a tetromino 
	/// is correct (doesn't collide with a block in the grid, or doesn't go out of the boarders)
    /// </summary>
    /// <returns>
    /// boolean
    /// True if it is correct
    /// False otherwise.
    /// </returns>
	bool isValidGridPos()
	{
		foreach(Transform child in transform)
		{
			Vector2 v = Grid._grid.roundVec2(convertPos(child.position));
			
			// Check if it's inside the borders
			if (!Grid._grid.isInsideBorders(v))
				return false;

			// Check if a child	is superposed with another block of the grid
			if (Grid._grid.grid[(int)v.x, (int)v.y] != null)
            {
                return false; 
            }
				
		}
		return true;
	}
	

    /// <summary>
	/// Called when a tetromino is set down
	/// Updates grid and deletes complete rows
	/// Updates falling and foreseen tetrominos
    /// </summary>
    /// <returns>void</returns>
	void updateGrid()
	{
		// yMax stocks the highest child position of the tetromino that has just fallen
		int yMax = 0;

		// Add the new tetromino to the grid	
		foreach (Transform child in transform) 
		{
			Vector2 v = Grid._grid.roundVec2(convertPos(child.position));
			Grid._grid.grid[(int)v.x, (int)v.y] = child;
			if ((int) v.y > yMax)
				yMax = (int) v.y;
		}
		// Now we delete potential full rows
		Grid._grid.deleteFullRowsFrom(yMax);		
	}
	

    /// <summary>
    /// Called when a tetromino is set down and do the transition (foreseen tetromino becomes the fallingtetromino).
    /// It also handles resize (foreseen tetromino scale is different from the tetrominos' scale of the board.
    /// </summary>
    /// <returns>void</returns>
	void nextFalling()
	{
        if (foreSeenTetromino != null)
        {
            foreSeenTetromino.transform.position = spawnPosition;
            // Check if this is a valid position, if it's not, then game over.

            // Rescale for the game (foreseen can be smaller)
            foreSeenTetromino.transform.localScale = new Vector3(Grid._grid.xScale, Grid._grid.yScale, 1);

            if (!foreSeenTetromino.isValidGridPos())
            {
                gameOver();
                return;
            }

            fallingTetromino = foreSeenTetromino;
            fallingTetromino.timeSinceLastFall = Time.time;
            foreSeenTetromino = null;

            Spawner._spawner.spawnNext();
            // when the next spawns, the function starts put it in foreSeenTetromino
        }
	}
	

    /// <summary>
    /// Called when the player has lost.
    /// </summary>
    /// <returns>void</returns>
    void gameOver()
    {
        EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
    }
}
