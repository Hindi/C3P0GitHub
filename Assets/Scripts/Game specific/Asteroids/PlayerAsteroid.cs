using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class PlayerAsteroid : MonoBehaviour {

    [SerializeField]
    private List<Transform> targetList;


    [SerializeField]
    private Camera playerCamera;

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
        upDownRange = 10f;
        leftRightRange = 20f;
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
            rotLeftRight -= 0.1f;
        if (Input.GetKey(KeyCode.RightArrow))
            rotLeftRight += 0.1f;
        if (Input.GetKey(KeyCode.UpArrow))
            rotUpDown -= 0.1f;
        if (Input.GetKey(KeyCode.DownArrow))
            rotUpDown += 0.1f;

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
                Debug.Log("un asteroid a été touche en pos : " + hit.point);
                hit.transform.gameObject.GetComponent<Asteroid>().hit();
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
            Debug.Log("Je tente de tirer");
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
