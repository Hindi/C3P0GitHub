using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidsManager : MonoBehaviour {

    private float timeSinceLastSpawn;
    private float cdSpawnAsteroid;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Camera playerCamera;

    private AsteroidShip shipScript;


    private static Dictionary<int, Asteroid> asteroidInUse = new Dictionary<int,Asteroid>();
    private static int nbAsteroid;
    private static int colorChoice;
    private static int zoneChoice;
    private int nbColor;
    private bool isColor;

    private bool spawn;

    [SerializeField]
    int nbAsteroidToSpawn;

    [SerializeField]
    AsteroidNetwork asteroidNetwork;

    [SerializeField]
    PlayerAsteroid playerAsteroid;

    [SerializeField]
    GameObject canvas;

	// Use this for initialization
    void Start()
    {
        nbColor = 5; // Bleu, Vert, Rouge, Jaune, Violet
        asteroidInUse = new Dictionary<int, Asteroid>();
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();
        cameraInitialisation();
        if (!asteroidNetwork.isServer())
        {
            asteroidNetwork.askInitPlayer();
            canvas.SetActive(false);
        }
        else
            canvas.SetActive(true);
        timeSinceLastSpawn = Time.time;
        cdSpawnAsteroid = 1.0f;
        isColor = false;
        spawn = false;
	}
	
	// Update is called once per frame
	void Update () {
      if (asteroidNetwork.isServer())
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 pos = new Vector3(Random.value, Random.value, Random.Range(200, 500));
                pos = mainCamera.ViewportToWorldPoint(pos);

                Vector3 target = shipScript.getTarget();
                if (nbAsteroid % 2 == 0)
                {
                    asteroidNetwork.createAsteroidColor(pos, target, Random.Range(1, 2), nbAsteroid, 2, EnumColor.NONE);
                }
                else
                {
                    asteroidNetwork.createAsteroidColor(pos, target, Random.Range(1, 2), nbAsteroid, 1, EnumColor.NONE);
                }
                nbAsteroid++;
            }
            spawnAsteroid(isColor);
        }
	}

    void spawnAsteroid(bool isColor)
    {
        if (spawn)
        {
            EnumColor col;
            if (isColor)
            {
                col = (EnumColor)((nbAsteroid % nbColor) + 1);
            }
            else
                col = EnumColor.NONE;
            if (Time.time - timeSinceLastSpawn > cdSpawnAsteroid)
            {
                Vector3 pos = new Vector3(Random.value, Random.value, Random.Range(200, 500));
                pos = mainCamera.ViewportToWorldPoint(pos);
                Vector3 target = shipScript.getTarget();
                asteroidNetwork.createAsteroidColor(pos, target, Random.Range(1, 2), nbAsteroid, 2, col);
                nbAsteroid++;
                cdSpawnAsteroid = (cdSpawnAsteroid + 200f) / (float)(nbAsteroid + 199);
                timeSinceLastSpawn = Time.time;
            }
        }
    }

    void cameraInitialisation()
    {
        if(asteroidNetwork.isServer())
        {
            playerCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Add an element to the dictionary
    /// </summary>
    /// <param name="id">Key of the element added</param>
    /// <param name="ast">Element added</param>
    public void add(int id, Asteroid ast)
    {
        asteroidInUse.Add(id, ast);
    }

    /// <summary>
    /// Remove the asteroid designed from the dictionary of used asteroids
    /// </summary>
    /// <param name="id">The ID of the asteroid to remove</param>
    /// <returns>void</returns>
    public void remove(int id)
    {
        if (asteroidNetwork.isServer())
            asteroidNetwork.destroyAsteroid(id);
        if(asteroidInUse.ContainsKey(id))
        {
            asteroidInUse[id].destroy();
            if (!asteroidInUse.Remove(id))
                Debug.Log("Problem there is no asteroid with the id : " + id);
        }
        
    }

    public void hit(int id)
    {
        Asteroid ast;
        if (asteroidInUse.TryGetValue(id, out ast))
        {
            // We should always be in this case.
            ast.hit();
        }
        else
        {
            Debug.Log("Problem there is no asteroid with the id : " + id);
        }
    }

    public EnumColor chooseColor()
    {
        colorChoice++;
        int colorReturn = (colorChoice % nbColor) + 1; // return a number from 1 to 5
        return ((EnumColor) colorReturn);
    }



    public void initColor(EnumColor color)
    {
        playerAsteroid.setColor(color);
    }

    public void gameOver()
    {
        foreach(KeyValuePair<int, Asteroid> p in asteroidInUse)
        {
            // Give the asteroid to the factory
            p.Value.destroy();              
        }
        asteroidInUse = new Dictionary<int, Asteroid>();
        spawn = false;
        if (!asteroidNetwork.isServer())
            canvas.SetActive(false);
        else
            canvas.SetActive(true);
    }

    public void launchColor(bool b)
    {
        timeSinceLastSpawn = Time.time;
        cdSpawnAsteroid = 1.0f;
        isColor = b;
        spawn = true;
        canvas.SetActive(false);
    }

}
