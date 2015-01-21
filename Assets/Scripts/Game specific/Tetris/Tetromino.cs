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
    private int moveRightTimer, moveLeftTimer, moveDownTimer, movingRate;
	
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
        // TO DO create REAL timers 
        movingRate = 5;

        // Timers initialization
        moveDownTimer = movingRate/2;
        moveRightTimer = movingRate;
        moveLeftTimer = movingRate;
		
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
	void Update () 
	{
		if (this == fallingTetromino)
		{
			// Move Left
			if (Input.GetKey(KeyCode.LeftArrow)) 
			{
                if (moveLeftTimer == movingRate)
                {
                    moveLeft();
                    moveLeftTimer = 0;
                }

                moveLeftTimer++;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
                if (moveRightTimer == movingRate)
                {
                    moveRight();
                    moveRightTimer = 0;
                }

                moveRightTimer++;
			}
            else
            {
                moveRightTimer = movingRate;
                moveLeftTimer = movingRate;
            }
            // We can move to the left or to the right and go up or down at the same time
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				rotate();
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				// MoveDown handles grid updates if the tetromino is set
                // Two ways of moving down, when it ticks and by pressing down
                if (moveDownTimer == movingRate/2)
                {
                    moveDown();
                    moveDownTimer = 0;
                    timeSinceLastFall = Time.time;
                    Grid._grid.fastFallScore++;
                }
                moveDownTimer++;
			}
            else
            {
                moveDownTimer = movingRate/2;
            }

            if ((Time.time - timeSinceLastFall) >= fallingSpeed)
            {
                moveDown();
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
	void moveLeft()
	{
		// We move the tetromino to the left
		transform.position += new Vector3(-1, 0, 0);
        
		// See if it's still a valid position
		if (!isValidGridPos())
			// Its not valid. revert.
			transform.position += new Vector3(1, 0, 0);
	}
	
	void moveRight()
	{
		// We move the tetromino to the right
		transform.position += new Vector3(1, 0, 0);
        
		// See if it's still a valid position
		if (!isValidGridPos())
			// Its not valid. revert.
			transform.position += new Vector3(-1, 0, 0);
	}
	
	void moveDown()
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
	

	void rotate()
	{
        if (canRotate)
        {
            int nbTry = 0;
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
	


    // TO DO game over screen and menu
    void gameOver()
    {
        EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
    }
}
