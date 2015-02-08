using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidsManager : MonoBehaviour {

    private float timeSinceLastSpawn;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Camera playerCamera;

    private AsteroidShip shipScript;


    private static Dictionary<int, Asteroid> asteroidInUse = new Dictionary<int,Asteroid>();
    private static int nbAsteroid;
    private static int colorChoice;
    private static int zoneChoice;

    [SerializeField]
    int nbAsteroidToSpawn;

    [SerializeField]
    AsteroidNetwork asteroidNetwork;

    [SerializeField]
    PlayerAsteroid playerAsteroid;

	// Use this for initialization
    void Start()
    {
        asteroidInUse = new Dictionary<int, Asteroid>();
        Vector3 pos, target;
        pos = new Vector3(0, 0, 0);
        target = pos;
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();
        cameraInitialisation();
        if (!asteroidNetwork.isServer())
        {
            asteroidNetwork.askInitPlayer();
        }
	}
	
	// Update is called once per frame
	void Update () {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 pos = new Vector3(Random.value, Random.value, Random.Range(200, 500));
                pos = mainCamera.ViewportToWorldPoint(pos);

                Vector3 target = shipScript.getTarget();
                if (nbAsteroid % 2 == 0)
                {
                    asteroidNetwork.createAsteroidColor(pos, target, Random.Range(1, 2), nbAsteroid, 2, EnumColor.RED);
                }
                else
                {
                    asteroidNetwork.createAsteroid(pos, target, Random.Range(1, 2), nbAsteroid, 1);
                }
                nbAsteroid++;
                timeSinceLastSpawn = Time.time;
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
        if (asteroidNetwork.isServer())
        {
            colorChoice++;
            int colorReturn = colorChoice % 4;
            return ((EnumColor) colorReturn);
        }
        return EnumColor.NONE;
    }

    public void choosePos()
    {

    }


    public void initColor(EnumColor color)
    {
        playerAsteroid.setColor(color);
    }

    public void initZone()
    {
        //playerAsteroid.setZone();
    }
}
