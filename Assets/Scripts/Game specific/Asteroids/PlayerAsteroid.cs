using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class PlayerAsteroid : MonoBehaviour {

    [SerializeField]
    private List<Transform> targetList;


    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private AsteroidNetwork asteroidNetwork;

    public float mouseSensitivity;

    float upDownRange;
    float leftRightRange;

    public float speed;

    public float rotLeftRight;
    public float rotUpDown;

    private float cdShoot, cdMoveHorizontal, cdMoveVertical;
    private float timeSinceLastShoot, timeSinceLastMoveHorizontal, timeSinceLastMoveVertical;

	// Use this for initialization
	void Start () {
        upDownRange = 15f;
        leftRightRange = 22f;
        mouseSensitivity = 1f;
        cdShoot = 1f;
        cdMoveHorizontal = 1f / 60f;
        cdMoveVertical = cdMoveHorizontal;
        timeSinceLastShoot = 0;
        timeSinceLastMoveVertical = 0;
        timeSinceLastMoveHorizontal = 0;
	}
	
	// Update is called once per frame
	void Update () {
        rotLeftRight += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotUpDown += -Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (Input.GetKeyDown(KeyCode.Space))
            this.fire();
        if (Input.GetKey(KeyCode.LeftArrow))
            moveRight(false);
        if (Input.GetKey(KeyCode.RightArrow))
            moveRight(true);
        if (Input.GetKey(KeyCode.UpArrow))
            moveUp(true);
        if (Input.GetKey(KeyCode.DownArrow))
            moveUp(false);

        setRotation();
	}

    public Vector3 getTarget()
    {
        return targetList[(int)Random.Range(0, targetList.Count)].position;
    }

    public void rayCast(Ray ray)
    {
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1500);
        if (hit.transform !=null)
        {
            if(hit.transform.CompareTag("Asteroid"))
            {
                // TO DO Envoyer à tout le monde qu'on a touché tel asteroid pour qu'il fasse la synchro 
                // On accède à l'astéroid avec le code en dessus (plus ou moins)
                //asteroidNetwork.hitAsteroid(hit.transform.gameObject.GetComponent<Asteroid>().id);
                asteroidNetwork.hitAsteroid(hit.transform.gameObject.GetComponent<Asteroid>().id);
            }
        }
        else
        {
            Debug.Log("j'ai raté");
        }
    }

    public void fire()
    {
        if (Time.time - timeSinceLastShoot > cdShoot)
        {
            Ray ray;
            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            rayCast(ray);
            timeSinceLastShoot = Time.time;
        }
    }

    public void moveRight(bool b)
    {
        if (Time.time - timeSinceLastMoveHorizontal > cdMoveHorizontal)
        {
            if (b)
            {
                rotLeftRight += speed;
            }
            else
            {
                rotLeftRight -= speed;
            }
            timeSinceLastMoveHorizontal = Time.time;
        }
    }

    public void moveUp(bool b)
    {
        if (Time.time - timeSinceLastMoveVertical > cdMoveVertical)
        {
            if (b)
                rotUpDown -= speed;
            else
                rotUpDown += speed;
            timeSinceLastMoveVertical = Time.time;
        }
    }


    public void setRotation()
    {
        // We clamp the x and y angles and then set the rotation
        rotLeftRight = Mathf.Clamp(rotLeftRight, -leftRightRange, leftRightRange);
        rotUpDown = Mathf.Clamp(rotUpDown, -upDownRange, upDownRange);
        transform.eulerAngles = new Vector3(rotUpDown, rotLeftRight, 0);
    }
}
