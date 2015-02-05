using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public int id;
    private AsteroidShip shipScript;
    private Vector3 target;
    public int hp;
    public bool isUsed;

    private static int nbAsteroid = 0;


    [SerializeField]
    private float speed;

    private AsteroidsManager asteroidManager;
    
	// Use this for initialization
	void Start () {
        /*
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();
        
        //target = shipScript.getTarget();
        isUsed = false;
        id = nbAsteroid;
        nbAsteroid++;
        AsteroidFactory._factory.push(this);
        //gameObject.renderer.enabled = false;
        gameObject.SetActive(false);
         * */
	}
	
    void Awake()
    {
        asteroidManager = GameObject.FindGameObjectWithTag("asteroidManager").GetComponent<AsteroidsManager>();
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();

        //target = shipScript.getTarget();
        isUsed = false;
        id = nbAsteroid;
        nbAsteroid++;
    }

	// Update is called once per frame
	void Update () {
        if (isUsed)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
	}



    /// <summary>
    /// Called by a RPC to signify that this asteroid has been hit
    /// Everyone receives this one so the maj it does is done for everyone
    /// </summary>
    public void hit()
    {
        hp--;
        if (hp <= 0)
        {
            isUsed = false;
            //gameObject.renderer.enabled = false;
            // We suppress this asteroid from the used asteroid dictionary
            // Calls a RPC that does that for everyone
            asteroidManager.remove(this.id);

            // We disable the gameobject assiociated
            gameObject.SetActive(false);

            // Now we add it to the unuse asteroid stack
            AsteroidFactory._factory.push(this);
        }
    }


    // bool b indicates if it is activated or not
    public void initAsteroid(Vector3 posSpawn, Vector3 cible, int health, bool b)
    {
        target = cible;
        transform.position = posSpawn;
        hp = health;
        isUsed = true;
        //gameObject.renderer.enabled = true;

        // We activate the asteroid if needed 
        gameObject.SetActive(b);

        if (b)
        {
            // Now we add it to the use asteroids dictionary
            asteroidManager.add(this.id, this);
        }
        else
            AsteroidFactory._factory.push(this);
    }
}
