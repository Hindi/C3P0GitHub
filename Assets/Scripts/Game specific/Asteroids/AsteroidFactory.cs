using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidFactory : MonoBehaviour {

    public static AsteroidFactory _factory;
    public Stack<Asteroid> asteroidNotInUse;
    [SerializeField]
    public int nbAsteroidInitalized;

    [SerializeField]
    GameObject asteroids;

	// Use this for initialization
	void Start () {
        // TO DO régler le nombre max d'astéroids selon le nombre de joueurs
	    asteroidNotInUse = new Stack<Asteroid>();

        // We create some asteroids when the scene is loaded
        for(int i = 0; i < nbAsteroidInitalized; i++)
        {
            // When an Asteroid is created, he isn't used immediatly so it adds itself
            // in the asteroidNotInUse stack
            Instantiate(asteroids);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void createAsteroid(Vector3 pos, Vector3 target, int hp)
    {
        if (asteroidNotInUse.Count > 0)
        {
            asteroidNotInUse.Pop().initAsteroid(pos, target, hp);
        }
        else
        {
            // Instantiate a new Asteroid so it gets the start phase and initialise it after
            ((GameObject)Instantiate(asteroids)).GetComponent<Asteroid>().initAsteroid(pos, target, hp);
        }
    }


    /* not used anymore
    public int getFreeId()
    {
        
        for (int i = 0; i < maxAsteroid; i++ )
        {
            if (asteroidList[i] == null)
                return i;
        }
        
        foreach (Asteroid ast in asteroidList)
        {
            if (!ast.isUsed)
            {
                Debug.Log(ast.id);
                return ast.id;
            }
        }
        return -1;
    }
    */
    void Awake()
    {
        if (_factory != null)
        {
            Debug.Log(" AsteroidFactory should be unique");
        }
        else
        {
            _factory = this; 
        }
    }

    /// <summary>
    /// Add an Asteroid to the stack of unused asteroids
    /// We make sure that all the changes required to have a disabled asteroid are done before
    /// </summary>
    /// <param name="ast"></param>
    public void push(Asteroid ast)
    {
        asteroidNotInUse.Push(ast);
    }
}
