using UnityEngine;
using System.Collections;
/*********************************************************************************** 
 * This class handles Tetrominos (foreseen, movements, inputs ...)                 *
 ***********************************************************************************/



public class Tetromino : MonoBehaviour {
	// Time since last fall (gravity tick)
	private float timeSinceLastFall;
	
	// Time between two "natural" falls of the tetromino
	private float fallingSpeed;

    // "Timers" not to move left, right, or down at every frames when we hold the key down
    // A movement every movingRate frames
    private float moveRightTimer, moveLeftTimer, moveDownTimer, movingRate, movingDownRate;
	
	// Place where the tetromino will spawn on the board
	private Vector3 spawnPosition;
	
	
	public static Tetromino fallingTetromino = null ;
	public static Tetromino foreSeenTetromino = null ;


    // Only the Tetromino O can't rotate 
    [SerializeField]
    private bool canRotate;

	// Use this for initialization
	void Start () {		
		// Timer to maje a tetromino falls
		timeSinceLastFall = Time.time;
		
        // Falling speed, increases with the lvl
		fallingSpeed = 1.0f / (Grid._grid.level + 1);
		
		// Place where a tetromino will spawn
		spawnPosition = new Vector3(4,20,0);

        // movingRate initialization
        // Thus we can go left or right 12 times per second
        movingRate = 1.0f / 12;
        // Thus we can do a fast fall that will go down 30 times/sec
        movingDownRate = 1.0f / 30;

        // Timers initialization
        moveDownTimer = Time.time;
        moveRightTimer = Time.time;
        moveLeftTimer = Time.time;
		
		// This block handles wether the tetromino is falling in the board or is 
		// just foreseen
		
		// First case should happen only when the game starts
		if (fallingTetromino == null && foreSeenTetromino == null)
		{
			fallingTetromino = this;
			fallingTetromino.transform.position = spawnPosition;
			
			// We create the foreseen tetromino
			Spawner._spawner.spawnNext();
		}
		else if (foreSeenTetromino == null)
		{
			foreSeenTetromino = this;
		}
	}
	
	// Update is called once per frame
    // Handles automatic drop and automatic delete when all the tetromino doesn't have any block
	void Update () 
	{
        // Automatic move down handles here
		if (this == fallingTetromino)
        {
            if ((Time.time - timeSinceLastFall) >= fallingSpeed)
            {
                goDown();
                timeSinceLastFall = Time.time;
            }
		}
        // When a tetromino doesn't have a child anymore he is deleted
        if (transform.childCount == 0)
            Destroy(this.gameObject);
          
	}
	


	/*******************************************************************************
	 *                          Movement functions                                 *
	 *******************************************************************************/
	public void moveLeft()
	{
        // First we check if we can move left this frame
        if (Time.time - moveLeftTimer >= movingRate)
        {
            // We move the tetromino to the left
            transform.position += new Vector3(-1, 0, 0);

            // See if it's still a valid position
            if (!isValidGridPos())
                // Its not valid. revert.
                transform.position += new Vector3(1, 0, 0);
            moveLeftTimer = Time.time;
        }
		
	}

    public void moveRight()
	{
        // Check if we can move to the right this frame
        if (Time.time - moveRightTimer >= movingRate)
        {
            // We move the tetromino to the right
            transform.position += new Vector3(1, 0, 0);

            // See if it's still a valid position
            if (!isValidGridPos())
                // Its not valid. revert.
                transform.position += new Vector3(-1, 0, 0);

            // We set the timer 
            moveRightTimer = Time.time;
        }
	}


    // goDown handles grid updates if the tetromino is set
    public void goDown()
    {
		// We move the tetromino to the bottom
		transform.position += new Vector3(0, -1, 0);
        
		// See if it's still a valid position
		if (!isValidGridPos())
		{
			// Its not valid. revert.
			transform.position += new Vector3(0, 1, 0);
			// The tetromino is set and can't move anymore
			// So we update the grid and delete rows if they are full (done in updateGrid)
			updateGrid();
			// Then the next tetromino appears
			nextFalling();
		}
	}

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

    public void rotate()
	{
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);

            // See if valid
            if (!isValidGridPos())
            {
                if (!tryTranslate())
                {
                    // It's not valid, revert
                    transform.Rotate(0, 0, 90);
                }
            }
               
        }
	}
	
    // When a rotation is impossible we try to move the tetromino to the left/right to do the rotation
    // We only try it for a translation by 2 on the left, then to the right
    bool tryTranslate()
    {
        int nbTry=0;
        Vector3 originPos = transform.position;
        while (!isValidGridPos() && nbTry < 3)
        {
            transform.position += new Vector3(-1,0,0);
            nbTry++;
        }
        if (nbTry < 3)
            return true;
        nbTry = 0;
        transform.position = originPos;
        while(!isValidGridPos() && nbTry < 3)
        {
            transform.position += new Vector3(1, 0, 0);
            nbTry++;
        }
        if (nbTry < 3)
            return true;
        transform.position = originPos;
        return false;
    }


	// Function that checks if the position of all the child blocks of a tetromino 
	// is correct
	bool isValidGridPos()
	{
		foreach(Transform child in transform)
		{
			Vector2 v = Grid._grid.roundVec2(child.position);
			
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
	
	// Grid is updated only when a tetromino sets down
	// Updates grid and deletes complete rows
	// updates falling and foreseen tetrominos
	void updateGrid()
	{
		// yMax stocks the highest child position of the tetromino that has just fallen
		int yMax = 0;

		// Add the new tetromino to the grid	
		foreach (Transform child in transform) 
		{
			Vector2 v = Grid._grid.roundVec2(child.position);
			Grid._grid.grid[(int)v.x, (int)v.y] = child;
			if ((int) v.y > yMax)
				yMax = (int) v.y;
		}
		// Now we delete potential full rows
		Grid._grid.deleteFullRowsFrom(yMax);		
	}
	

    // 
	void nextFalling()
	{
        if (foreSeenTetromino != null)
        {
            foreSeenTetromino.transform.position = spawnPosition;
            // Check if this is a valid position, if it's not, then game over.
            if (!foreSeenTetromino.isValidGridPos())
            {
                gameOver();
                return;
            }

            fallingTetromino = foreSeenTetromino;
            foreSeenTetromino = null;

            Spawner._spawner.spawnNext();
            // when the next spawns, the function starts put it in foreSeenTetromino
        }
	}
	


    void gameOver()
    {
        EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
    }
}
