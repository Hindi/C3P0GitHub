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

    [SerializeField]
    private GameObject viseur;

    public float mouseSensitivity;

    float upDownRange;
    float leftRightRange;



    public float speed;

    public float rotLeftRight;
    public float rotUpDown;

    private EnumColor color;

    private float cdShoot, cdMoveHorizontal, cdMoveVertical;
    private float timeSinceLastShoot, timeSinceLastMoveHorizontal, timeSinceLastMoveVertical;

	// Use this for initialization
	void Start () {
        upDownRange = 15f;
        leftRightRange = 25f;
        mouseSensitivity = 1f;
        cdShoot = 1f;
        cdMoveHorizontal = 1f / 60f;
        cdMoveVertical = cdMoveHorizontal;
        timeSinceLastShoot = 0;
        timeSinceLastMoveVertical = 0;
        timeSinceLastMoveHorizontal = 0;
        color = EnumColor.NONE;
	}
	
	// Update is called once per frame
	void Update () {
        rotLeftRight += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotUpDown += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        // TO DO remove les input locales
        //if (Input.GetKeyDown(KeyCode.Space))
        //    fire();
        setRotation();
	}

    public Vector3 getTarget()
    {
        return targetList[(int)Random.Range(0, targetList.Count)].position;
    }

    public void rayCast(Ray ray)
    {
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1500, playerCamera.cullingMask);
        if (hit.transform !=null)
        {
            if (hit.transform.CompareTag("Asteroid"))
            {
                Asteroid ast = hit.transform.gameObject.GetComponentInParent<Asteroid>();
                Debug.Log(ast.getColor());
                asteroidNetwork.hitAsteroid(ast.id);
                if (ast.getColor() == color)
                {
                    asteroidNetwork.hitAsteroid(ast.id);
                    Debug.Log("J'ai touché un asteroid de la même couleur que moi");
                }
                else
                {
                    Debug.Log("J'ai pas touché la bonne couleur");
                }
            }
            else
            {
                Debug.Log("Je sais pas ce que j'ai touché");
                Debug.Log(hit.transform.gameObject);
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
            Ray ray= new Ray();

            // The ratio we use are calculated with the relative position of the target in the cockpit image. 
            // TO DO modifier ça avec le resize
            ray.origin = playerCamera.transform.position;
            Vector3 dir = viseur.transform.position - playerCamera.transform.position;
            ray.direction = dir;
            Debug.Log("boite");
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

    /*
    /// <summary>
    /// Used to change the camera's size according to screen size ratio to keep consistent across all platforms
    /// </summary>
    public void updateElementsResolution()
    {
        float width, height;
        width = Math.Max(Screen.width, Screen.height);
        height = Math.Min(Screen.width, Screen.height);
        aspectRatio = width / height;
        playerCamera.projectionMatrix = Matrix4x4.Ortho(-5.0f * aspectRatio, 5.0f * aspectRatio, -5.0f, 5.0f, 0.3f, 1000f);
     * playerCamera.aspect = (float)Screen.width /(float) Screen.height
    }
     * */

    public void setColor(EnumColor col)
    {
        Debug.Log("Je suis de couleur : " + col);
        color = col;
    }


}
