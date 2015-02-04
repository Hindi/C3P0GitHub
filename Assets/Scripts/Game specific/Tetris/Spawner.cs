using UnityEngine;
using System.Collections;


/// <summary>
/// Tetris spawner ie the element that instanciates new tetrominos when necessary.
/// </summary>
public class Spawner : MonoBehaviour {

    /// <summary>
    /// _spawner is a singleton so we refer himself as static
    /// </summary>
	public static Spawner _spawner;

	// Vector use to store all the tetrominos and choose randomly the next one to 
	// be spawned

    /// <summary>
    /// Vector used to store all the tetrominos' type and choose randomly the next one to 
    /// be spawned.
    /// </summary>
	public GameObject[] tetrominos;

    /// <summary>
    /// Position where the spawner has to be for PC. 
    /// </summary>
    [SerializeField]
    Vector3 posPC;

    ///<summary>
    /// Use for easier tests for mobile UI
    ///</summary>
    private bool isMobile;

	// Use this for initialization
    /// <summary>
    /// Called when a Spawner has been instantiate. Set the spawner to the right place and spawns a tetromino 
    /// </summary>
    /// <returns>void</returns>
	void Start () {
        isMobile = Application.isMobilePlatform;

        // Position differs if it's mobile or not
        if (isMobile)//Application.isMobilePlatform
        {
            transform.position = new Vector3(-4.75f * Grid._grid.xScale, 21f * Grid._grid.yScale, 0);
        }
        else
        {
            transform.position = posPC;
        }
        spawnNext();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	
    /// <summary>
    /// Create a new Tetromino
    /// </summary>
    /// <returns>void</returns>
	public void spawnNext()
    {
        // Create a random index that will choose the one to be drawn in tetrominos
        int i = Random.Range(0, tetrominos.Length);

        // This makes sure we don't draw the max value for the random, so we can't have an out of range for the array
        while( i == tetrominos.Length)
        {
            i = Random.Range(0, tetrominos.Length);
        }
        // Create a Tetromino at the spawner Position
        Instantiate(tetrominos[i],
                    transform.position,
                    Quaternion.identity);

	}
	
	
    /// <summary>
    /// Called before Start. Make sure that the spawner is a singleton.
    /// </summary>
    /// <returns>void</returns>
	void Awake()
	{
		if (_spawner != null)
		{
			Debug.Log("There should only be one Spawner instance");
		}
		else 
		{
			_spawner = this;

		}


	}
}
