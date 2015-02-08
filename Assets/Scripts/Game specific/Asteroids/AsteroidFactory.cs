using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidFactory : MonoBehaviour {

    public static AsteroidFactory _factory;

    public Stack<Asteroid> asteroidNotInUse;

    [SerializeField]
    public int nbAsteroidInitalized;

    [SerializeField]
    List<GameObject> asteroidsPrefabList;

	// Use this for initialization
	void Start () {
        Asteroid ast;
        // We create some asteroids when the scene is loaded
        for(int i = 0; i < nbAsteroidInitalized; i++)
        {
            // When an Asteroid is created, he isn't used immediatly so it adds itself
            // in the asteroidNotInUse stack
            ast = ((GameObject)Instantiate(getRandomAsteroids())).GetComponent<Asteroid>();

            push(ast);
        }
	}

    private GameObject getRandomAsteroids()
    {
        return asteroidsPrefabList[Random.Range(0, asteroidsPrefabList.Count - 1)];
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void createAsteroid(Vector3 pos, Vector3 target, int hp, int id, int prefabId)
    {
        if (asteroidNotInUse.Count > 0)
        {
            asteroidNotInUse.Pop().init(pos, target, hp, id, EnumColor.NONE);
        }
        else
        {
            // Instantiate a new Asteroid so it gets the start phase and initialise it after
            Debug.Log(prefabId);
            ((GameObject)Instantiate(asteroidsPrefabList[prefabId])).GetComponent<Asteroid>().init(pos, target, hp, id, EnumColor.NONE);
        }
    }

    public void createAsteroid(Vector3 pos, Vector3 target, int hp, int id, int prefabId, EnumColor color)
    {
        if (asteroidNotInUse.Count > 0)
        {
            asteroidNotInUse.Pop().init(pos, target, hp, id, color);
        }
        else
        {
            // Instantiate a new Asteroid so it gets the start phase and initialise it after
            Debug.Log(prefabId);
            ((GameObject)Instantiate(asteroidsPrefabList[prefabId])).GetComponent<Asteroid>().init(pos, target, hp, id, color);
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
        ast.gameObject.SetActive(false);
    }
}
