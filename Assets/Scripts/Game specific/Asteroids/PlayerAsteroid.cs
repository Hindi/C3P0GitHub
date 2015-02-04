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


    public float rotLeftRight;
    public float rotUpDown;

	// Use this for initialization
	void Start () {
        upDownRange = 10f;
        leftRightRange = 30f;
        mouseSensitivity = 1f;
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

        rotLeftRight = Mathf.Clamp(rotLeftRight, -leftRightRange, leftRightRange);
        rotUpDown = Mathf.Clamp(rotUpDown, -upDownRange, upDownRange);
        transform.eulerAngles = new Vector3(rotUpDown, rotLeftRight, 0);



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
                Destroy(hit.rigidbody.gameObject);
            }
        }
        else
        {
            Debug.Log("j'ai raté");
        }
    }

    public void fire()
    {
        Ray ray;
        Debug.Log("Je tente de tirer");
        ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        rayCast(ray);
    }
}
