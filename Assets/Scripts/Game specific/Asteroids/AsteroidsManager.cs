using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidsManager : MonoBehaviour {

    private float timeSinceLastSpawn;

    [SerializeField]
    Camera mainCamera;

    private AsteroidShip shipScript;


    private static Dictionary<int, Asteroid> asteroidInUse;

    [SerializeField]
    int nbAsteroidToSpawn;

	// Use this for initialization
	void Start () {
        Vector3 pos, target;
        pos = new Vector3(0, 0, 0);
        target = pos;
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();
        for (int i = 0; i < nbAsteroidToSpawn; nbAsteroidToSpawn++)
        {
            AsteroidFactory._factory.createAsteroid(pos, target, Random.Range(1, 2), false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (asteroidInUse == null)
            asteroidInUse = new Dictionary<int, Asteroid>();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 pos = new Vector3(Random.value, Random.value, Random.Range(50, 200));
            pos = mainCamera.ViewportToWorldPoint(pos);

            Vector3 target = shipScript.getTarget();
            AsteroidFactory._factory.createAsteroid(pos, target, Random.Range(1, 2), true);
            timeSinceLastSpawn = Time.time;
        }
        
	}

    /// <summary>
    /// Add an element to the dictionary
    /// </summary>
    /// <param name="id">Key of the element added</param>
    /// <param name="ast">Element added</param>
    public void add(int id, Asteroid ast)
    {
        Debug.Log("Add asteroid of id : " + id);
        asteroidInUse.Add(id, ast);

        Debug.Log(asteroidInUse.Count);
    }

    /// <summary>
    /// Remove the asteroid designed from the dictionary of used asteroids
    /// </summary>
    /// <param name="id">The ID of the asteroid to remove</param>
    /// <returns>void</returns>
    public void remove(int id)
    {
        Debug.Log(asteroidInUse.Count);
        if (asteroidInUse.Remove(id)) 
            Debug.Log("I removed asteroid with the id : " + id);
        else
            Debug.Log("Problem there is no asteroid with the id : " + id);
        Debug.Log(asteroidInUse.Count);
    }

    public void hit(int id)
    {
        Asteroid ast;
        if (asteroidInUse.TryGetValue(id, out ast))
        {
            ast.hit();
        }
        else
        {
            Debug.Log("Problem there is no asteroid with the id : " + id);
        }
    }
}
