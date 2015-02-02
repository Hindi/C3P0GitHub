using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float rotationspeed;
    public float goalRotation;

    private bool onOrthographicFollow = false;
    private PlayerBehaviour playerScript;
    private int sign;

    private bool cameraMovement = false;
    private double dezoomDistance;
    private Vector3 startPosition;
    private float dezoomSpeed;

    private bool turned;
	// Use this for initialization
    void Start()
    {
        turned = false;
        orthographicFollow();
        Screen.SetResolution(1920, 1200, true);
        playerScript = player.GetComponent<PlayerBehaviour>();
        transform.position = player.transform.position;
        goalRotation = (int)transform.rotation.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (onOrthographicFollow)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!turned)
            {
                turnLeft();
                turned = true;
            }
            else
            {
                turnRight();
                turned = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!turned)
            {
                turnRight();
                turned = true;
            }
            else
            {
                turnLeft();
                turned = false;
            }
        }
        if (Input.GetKeyDown("p"))
        {
            cameraMovement = true;
            initCameraMoveScene1();
        }
        if (Mathf.Abs(goalRotation - (float)transform.rotation.eulerAngles.y) > 0.1)
        {
            transform.Rotate(Vector3.up * rotationspeed * sign);
            player.transform.Rotate(Vector3.up * rotationspeed * sign);
        }

        if (cameraMovement)
        {
            cameraMoveScene1();
        }
	}

    private void cameraMoveScene1()
    {
        transform.Rotate(new Vector3(0, 1.5f, 0));
        Camera.main.transform.LookAt(player.transform);
        Debug.Log(Vector3.Distance(startPosition, Camera.main.transform.position));
        if (Vector3.Distance(startPosition, Camera.main.transform.position) < dezoomDistance)
        {
            Camera.main.transform.Translate(Vector3.back * (Time.deltaTime * dezoomSpeed));
            Camera.main.transform.Translate(Vector3.up * (Time.deltaTime));
        }
        else
            cameraMovement = false;
    }

    private void initCameraMoveScene1()
    {
        Camera.main.orthographic = false;
        Camera.main.fieldOfView = 23;
        dezoomDistance = 100;
        startPosition = Camera.main.transform.position;
        dezoomSpeed = 10;
    }

    public void orthographicFollow()
    {
        onOrthographicFollow = true;
        if (!Camera.main.orthographic)
        {
            Camera.main.orthographic = true;
        }
    }

    public void turnRight()
    {
        sign = -1;
        if (goalRotation == 0)
        {
            goalRotation = 270;
        }
        else
        {
            goalRotation-= 90;
        }
    }

    public void turnLeft()
    {
        sign = 1;
        if (goalRotation == 270)
        {
            goalRotation = 0;
        }
        else
        {
            goalRotation += 90;
        }
    }

    public void eatChamp()
    {

    }
}
