using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public int id;
    private Vector3 target;
    public int hp;
    public bool isUsed;



    [SerializeField]
    private float speed;


    private AsteroidShip shipScript;
    private AsteroidsManager asteroidManager;
    private Behaviour halo;

    [SerializeField]
    GameObject blenderObject;
    [SerializeField]
    private Material blueMat;
    [SerializeField]
    private Material greenMat;
    [SerializeField]
    private Material redMat;

	// Use this for initialization
	void Start () {
	}
	
    void Awake()
    {
        asteroidManager = GameObject.FindGameObjectWithTag("asteroidManager").GetComponent<AsteroidsManager>();
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();
        halo = (Behaviour) gameObject.GetComponent("Halo");
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

            // Now we add it to the unuse asteroid stack
            // Factry disables the gameobject 
            AsteroidFactory._factory.push(this);
        }
    }


    // bool b indicates if it is activated or not
    public void initAsteroid(Vector3 posSpawn, Vector3 cible, int health, int id, EnumColor color)
    {
        target = cible;
        transform.position = posSpawn;
        hp = health;
        isUsed = true;
        this.id = id;
        //gameObject.renderer.enabled = true;

        //setColor(color);

        // We activate the asteroid and add it to use asteroid
        gameObject.SetActive(true);
        asteroidManager.add(this.id, this);
    }

    public void setColor(EnumColor color)
    {
        switch(color)
        {
            case EnumColor.NONE :
                (halo.GetType().GetProperty("enabled")).SetValue(halo, false, null); // it disables the halo
                break;
            case EnumColor.GREEN :
                blenderObject.renderer.material = greenMat;
                break;
            case EnumColor.BLUE:
                blenderObject.renderer.material = blueMat;
                break;
            case EnumColor.RED:
                blenderObject.renderer.material = redMat;
                break;
            default :
                Debug.Log("Forgot to had this color " + color + " in the switch setColor");
                break;
        }
    }
}
