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

    [SerializeField]
    private GameObject reactor1;
    [SerializeField]
    private GameObject reactor2;
    [SerializeField]
    private GameObject reactor3;

    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private TerrainLunarLander terrain;

    [SerializeField]
    private CameraLunarLander camera;

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

        consumCooldown = 0.1f;
        lastConsumTime = Time.time;
        reactorState = 0;
        timeBeforeRestart = 1;
	}

    public void onGameRestart()
    {
        score = 0;
        fuel = maxFuel;
        resetAfterLanding();
    }

    private void resetAfterLanding()
    {
        transform.rotation = Quaternion.identity;
        GetComponent<SpriteRenderer>().enabled = true;
        explosion.SetActive(false);
        landed = false;
        transform.position = terrain.getRandomSpawn();
        resetCamera();
        resetReactorState();
        resetForce();
        Physics2D.gravity = new Vector3(-horizontalForce, -gravityForce, 0);
        rigidbody2D.AddForce(new Vector3(6000, -100, 0));
    }

    private void resetCamera()
    {
        camera.resetCameraPosition(new Vector3(transform.position.x + Screen.width/100, camera.InitialPosition.y, Camera.main.transform.position.z));
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
            updateReactorSprite();
        }
    }

    public void decreaseReactorState()
    {
        if (reactorState > 0)
        {
            reactorState--;
            updateReactorSprite();
        }
    }

    private void updateReactorSprite()
    {
        switch(reactorState)
        {
            case 0:
                reactor1.SetActive(false);
                break;
            case 1:
                reactor1.SetActive(true);
                reactor2.SetActive(false);
                break;
            case 2:
                reactor1.SetActive(false);
                reactor2.SetActive(true);
                reactor3.SetActive(false);
                break;
            case 3:
                reactor2.SetActive(false);
                reactor3.SetActive(true);
                break;
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
                resetAfterLanding();
            if(fuel <= 0)
                EventManager<bool>.Raise(EnumEvent.GAMEOVER, true);
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
            }
        }

        fuelLabel.text = "Fuel " + fuel.ToString();
        scoreLabel.text = "Score " + score.ToString();
	}

    void OnBecameInvisible()
    {
        crash();
    }

    void crash()
    {
        landed = true;
        explosion.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        restartTimerStart = Time.time;
        if (collision.collider.tag == "Platform" && collision.relativeVelocity.y > -0.3f && collision.relativeVelocity.x > -0.2f)
        {
            score += 10;
            landed = true;
        }
        else
        {
            crash();
        }
        Physics.gravity = new Vector3(-0, -gravityForce, 0);
    }
}
