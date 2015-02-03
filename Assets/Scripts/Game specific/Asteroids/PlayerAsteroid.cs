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


    float rotLeftRight, rotUpDown;

    private Ray ray;
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

        rotLeftRight = Mathf.Clamp(rotLeftRight, -leftRightRange, leftRightRange);
        rotUpDown = Mathf.Clamp(rotUpDown, -upDownRange, upDownRange);
        transform.eulerAngles = new Vector3(rotUpDown, rotLeftRight, 0);


        if (Input.GetMouseButtonDown(0))
        {
            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            rayCast(ray);
        }

	}

    public Vector3 getTarget()
    {
        return targetList[(int)Random.Range(0, targetList.Count)].position;
    }

    public void rayCast(Ray ray)
    {
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100000);
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
}
