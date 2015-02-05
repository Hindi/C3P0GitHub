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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="target"></param>
    /// <param name="hp"></param>
    /// <param name="b"></param>
    /// 
    // le booléen sert à indiquer si on demande un nouvel asteroid pour s'en servir immédiatement ou juste pour 
    // le chargement afin que tout le monde ait une pool d'astéroids déjà instanciés.
    public void createAsteroid(Vector3 pos, Vector3 target, int hp, bool b)
    {
        if (asteroidNotInUse.Count > 0 && b)
        {
            asteroidNotInUse.Pop().initAsteroid(pos, target, hp, b);
        }
        else
        {
            // Instantiate a new Asteroid so it gets the start phase and initialise it after
            ((GameObject)Instantiate(asteroids)).GetComponent<Asteroid>().initAsteroid(pos, target, hp, b);
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

            _factory.asteroidNotInUse = new Stack<Asteroid>();
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
