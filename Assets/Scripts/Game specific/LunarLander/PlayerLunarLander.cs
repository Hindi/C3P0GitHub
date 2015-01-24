using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField]
    private ProgressBar altitudeBar;
    [SerializeField]
    private ProgressBar horizontalBar;
    [SerializeField]
    private ProgressBar verticalBar;

    [SerializeField]
    private Text fuelLabel;

    int reactorState;

    private Vector3 currentVelocity;

    private int fontSize;

	// Use this for initialization
    void Start()
    {
        horizontalBar.init(20);
        verticalBar.init(10);
        altitudeBar.init(10);

        fontSize = 1;
        consumCooldown = 0.1f;
        lastConsumTime = Time.time;
        reactorState = 0;
	}

    public void onGameRestart()
    {
        transform.position = new Vector3(-10, 6, 0);
        resetReactorState();
        rigidbody2D.isKinematic = true;
        rigidbody2D.isKinematic = false;
        Physics2D.gravity = new Vector3(-horizontalForce, -gravityForce, 0);
        rigidbody2D.AddForce(new Vector3(80000, -100, 0));
    }

    void useFuel()
    {
        if (Time.time - lastConsumTime > consumCooldown)
        {
            fuel--;
            lastConsumTime = Time.time;
        }
    }

    public void resetReactorState()
    {
        while (reactorState > 0)
            decreaseReactorState();
    }

    public void increaseReactorState()
    {
        if(reactorState < 3)
        {
            reactorState++;
            propulsor.transform.localScale = new Vector3(0.5f, propulsor.transform.localScale.y + 1, 0.5f);
        }
    }

    public void decreaseReactorState()
    {
        if (reactorState > 0)
        {
            reactorState--;
            propulsor.transform.localScale = new Vector3(0.5f, propulsor.transform.localScale.y - 1, 0.5f);
        }
    }

    public void rotate(float speedFactor)
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * speedFactor) * Time.deltaTime);
        useFuel();
    }
	
	// Update is called once per frame
	void Update () {
        currentVelocity = rigidbody2D.velocity * 10;
        altitudeBar.updateValue(transform.position.y);
        horizontalBar.updateValue(Mathf.Abs(currentVelocity.x));
        verticalBar.updateValue(Mathf.Abs(currentVelocity.y));
        if (fuel > 0)
        {
            if(reactorState > 0 )
            {
                rigidbody2D.AddRelativeForce(new Vector3(0, propulsorForce * reactorState, 0));
                useFuel();
            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
                increaseReactorState();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                decreaseReactorState();
            if (Input.GetKey(KeyCode.LeftArrow) && transform.rotation.z > -0.7f)
                rotate(1);
            if (Input.GetKey(KeyCode.RightArrow) && transform.rotation.z < 0.7f )
                rotate(-1);
        }
        else
        {
            propulsor.SetActive(false);
        }

        fuelLabel.text = "Fuel " + fuel.ToString();
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform")
        {
            if (collision.relativeVelocity.y > -0.3f && collision.relativeVelocity.x > -0.2f)
                EventManager.Raise(EnumEvent.RESTARTGAME);
        }
        else
            Debug.Log("BOOM");
        Physics.gravity = new Vector3(-0, -gravityForce, 0);
    }


}
