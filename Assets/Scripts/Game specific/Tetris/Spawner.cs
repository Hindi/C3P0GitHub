using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


	public static Spawner _spawner;

	// Vector use to store all the tetrominos and choose randomly the next one to 
	// be spawned
	public GameObject[] tetrominos;

    [SerializeField]
    Vector3 posMobile;

    [SerializeField]
    Vector3 posPC;

    // Use for easier tests for mobile UI
    private bool isMobile;

	// Use this for initialization
	void Start () {
        isMobile = true;// Remove Application.isMobilePlatform;
		spawnNext();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	
	public void spawnNext()
    {
        // Create a random index that will choose the one to be drawn in tetrominos
        int i = Random.Range(0, tetrominos.Length);

        // Create a Tetromino at the spawner Position
        Instantiate(tetrominos[i],
                    transform.position,
                    Quaternion.identity);

	}
	
	
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


        // Position differs if it's mobile or not
        if (isMobile)
        {
            transform.position = posMobile;
        }
        else
        {
            transform.position = posPC;
        }
	}
}
