using UnityEngine;
using System.Collections;

public class PlayerLunarLander : MonoBehaviour {

    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    float propulsorForce;

    [SerializeField]
    float gravityForce;
    [SerializeField]
    float horizontalForce;

    [SerializeField]
    GameObject propulsor;

    [SerializeField]
    int fuel;
    float consumCooldown;
    float lastConsumTime;

    int reactorState;

    private Rect labelRectHorizontal;
    private Rect labelRectVertical;
    private Rect labelRectAltitude;
    private Rect labelRectFuel;

    private Vector3 currentVelocity;

	// Use this for initialization
	void Start () {
        labelRectHorizontal = new Rect(Screen.width - 200, 20, 200, 20);
        labelRectVertical = new Rect(Screen.width - 200,40, 200, 20);
        labelRectAltitude = new Rect(Screen.width - 200, 60, 200, 20);
        labelRectFuel = new Rect(10, 20, 200, 20);
        consumCooldown = 0.1f;
        lastConsumTime = Time.time;
        reactorState = 0;
        start();
	}

    void start()
    {
        transform.position = new Vector3(-10, 6, 5);
        rigidbody.isKinematic = true;
        rigidbody.isKinematic = false;
        Physics.gravity = new Vector3(-horizontalForce, -gravityForce, 0);
        rigidbody.AddForce(new Vector3(80000, -100, 0));
    }

    void useFuel()
    {
        if (Time.time - lastConsumTime > consumCooldown)
        {
            fuel--;
            lastConsumTime = Time.time;
        }
    }

    void increaseReactorState()
    {
        if(reactorState < 3)
        {
            reactorState++;
            propulsor.transform.localScale = new Vector3(0.5f, propulsor.transform.localScale.y + 1, 0.5f);
        }
    }

    void decreaseReactorState()
    {
        if (reactorState > 0)
        {
            reactorState--;
            propulsor.transform.localScale = new Vector3(0.5f, propulsor.transform.localScale.y - 1, 0.5f);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (fuel > 0)
        {
            if(reactorState > 0 )
            {
                rigidbody.AddRelativeForce(new Vector3(0, propulsorForce * reactorState, 0));
                useFuel();
            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
                increaseReactorState();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                decreaseReactorState();
            if (Input.GetKey(KeyCode.LeftArrow) && transform.rotation.z > -0.7f)
            {
                transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
                useFuel();
            }
            if (Input.GetKey(KeyCode.RightArrow) && transform.rotation.z < 0.7f )
            {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed) * Time.deltaTime);
                useFuel();
            }
        }
        else
        {
            propulsor.SetActive(false);
        }
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Platform")
        {
            if (collision.relativeVelocity.y > -0.3f && collision.relativeVelocity.x > -0.2f)
                Debug.Log(collision.relativeVelocity);
        }
        else
            Debug.Log("BOOM");
        Physics.gravity = new Vector3(-0, -gravityForce, 0);
    }

    void OnGUI()
    {
        currentVelocity = rigidbody.velocity * 10;
        GUI.Label(labelRectHorizontal, "Horizontal Speed " + currentVelocity.x.ToString("F2"));
        GUI.Label(labelRectVertical, "Vertical Speed " + currentVelocity.y.ToString("F2"));
        GUI.Label(labelRectAltitude, "Altitude " + (transform.position.y * 10).ToString("F2"));
        GUI.Label(labelRectFuel, "Fuel " + fuel.ToString());
    }

}
