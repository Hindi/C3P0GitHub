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
    [SerializeField]
    private Text scoreLabel;

    int reactorState;

    private Vector3 currentVelocity;

    private int fontSize;
    private bool landed;

    private float timeBeforeRestart;
    private float restartTimerStart;

    private int maxFuel;

    private int score;
    public int Score { get { return score; } }

	// Use this for initialization
    void Start()
    {
        maxFuel = fuel;
        horizontalBar.init(20);
        verticalBar.init(10);
        altitudeBar.init(10);

        fontSize = 1;
        consumCooldown = 0.1f;
        lastConsumTime = Time.time;
        reactorState = 0;
        timeBeforeRestart = 1;
	}

    public void onGameRestart()
    {
        fuel = maxFuel;
        landed = false;
        transform.position = new Vector3(-10, 6, 0);
        resetReactorState();
        resetForce();
        Physics2D.gravity = new Vector3(-horizontalForce, -gravityForce, 0);
        rigidbody2D.AddForce(new Vector3(8000, -100, 0));
    }

    void resetForce()
    {
        rigidbody2D.isKinematic = true;
        rigidbody2D.isKinematic = false;
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

        if (landed)
        {
            if (Time.time - restartTimerStart > timeBeforeRestart)
                EventManager.Raise(EnumEvent.RESTARTGAME);

        }
        else
        {
            currentVelocity = rigidbody2D.velocity * 10;
            altitudeBar.updateValue(transform.position.y);
            horizontalBar.updateValue(Mathf.Abs(currentVelocity.x));
            verticalBar.updateValue(Mathf.Abs(currentVelocity.y));
            if (fuel > 0)
            {
                if (reactorState > 0)
                {
                    rigidbody2D.AddRelativeForce(new Vector3(0, propulsorForce * reactorState, 0));
                    useFuel();
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    increaseReactorState();
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    decreaseReactorState();
                if (Input.GetKey(KeyCode.LeftArrow) && transform.rotation.z > -0.7f)
                    rotate(1);
                if (Input.GetKey(KeyCode.RightArrow) && transform.rotation.z < 0.7f)
                    rotate(-1);
            }
            else
            {
                EventManager<bool>.Raise(EnumEvent.GAMEOVER, true);
            }
        }

        fuelLabel.text = "Fuel " + fuel.ToString();
        scoreLabel.text = "Score " + score.ToString();
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        landed = true;
        restartTimerStart = Time.time;
        if (collision.collider.tag == "Platform")
            if (collision.relativeVelocity.y > -0.3f && collision.relativeVelocity.x > -0.2f)
                score += 10;
        //Physics.gravity = new Vector3(-0, -gravityForce, 0);
    }

}
