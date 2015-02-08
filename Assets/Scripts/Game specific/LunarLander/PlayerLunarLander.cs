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
    private CameraLunarLander cameraLL;
    [SerializeField]
    private Text gratzText;

    int reactorState;

    private Vector3 currentVelocity;

    private int fontSize;
    private bool landed;

    private float timeBeforeRestart;
    private float restartTimerStart;

    private int maxFuel;

    private int score;
    public int Score { get { return score; } }

    RaycastHit2D hit;
    private PlatformLunarLander lastPlatformLanded;


	// Use this for initialization
    void Start()
    {
        maxFuel = fuel;
        horizontalBar.init(20);
        verticalBar.init(10);
        altitudeBar.init(10);

        consumCooldown = 0.2f;
        lastConsumTime = Time.time;
        reactorState = 0;
        timeBeforeRestart = 1;
        onGameRestart();
	}

    public void onGameRestart()
    {
        score = 0;
        fuel = maxFuel;
        resetAfterLanding();
        fuelLabel.color = Color.white;
    }

    private void resetAfterLanding()
    {
        resetForce();
        gratzText.gameObject.SetActive(false);
        if (lastPlatformLanded)
            lastPlatformLanded.lightDown();
        transform.rotation = Quaternion.identity;
        GetComponent<SpriteRenderer>().enabled = true;
        explosion.SetActive(false);
        landed = false;
        transform.position = terrain.getRandomSpawn();
        resetCamera();
        resetReactorState();
        Physics2D.gravity = new Vector3(-horizontalForce, -gravityForce, 0);
        rigidbody2D.AddForce(new Vector3(3000, -100, 0));
    }

    private void resetCamera()
    {
        cameraLL.resetCameraPosition(new Vector3(transform.position.x, 4, Camera.main.transform.position.z));
    }

    void OnDestroy()
    {
        Physics2D.gravity = new Vector2(0, 9.81f);
    }
    void resetForce()
    {
        rigidbody2D.isKinematic = true;
        rigidbody2D.isKinematic = false;
    }

    void useReactorFuel()
    {
        useFuel(reactorState);
    }

    void useRotationFuel()
    {
        useFuel(1);
    }

    void useFuel(int c)
    {
        if (Time.time - lastConsumTime > consumCooldown)
        {
            fuel = Mathf.Max(0, fuel - c);
            lastConsumTime = Time.time;

            if (fuel == 0)
            {
                fuelLabel.color = Color.red;
                hideReadctorSprites();
            }
        }
    }

    public void resetReactorState()
    {
        while (reactorState > 0)
            decreaseReactorState();
    }

    public void increaseReactorState()
    {
        if(!landed && reactorState < 3)
        {
            reactorState++;
            updateReactorSprite();
        }
    }

    private void hideReadctorSprites()
    {
        reactor1.SetActive(false);
        reactor2.SetActive(false);
        reactor3.SetActive(false);
    }

    public void decreaseReactorState()
    {
        if (!landed && reactorState > 0)
        {
            reactorState--;
            updateReactorSprite();
        }
    }

    private void updateReactorSprite()
    {
        hideReadctorSprites();
        if(fuel > 0)
        {
            switch (reactorState)
            {
                case 1:
                    reactor1.SetActive(true);
                    break;
                case 2:
                    reactor2.SetActive(true);
                    break;
                case 3:
                    reactor3.SetActive(true);
                    break;
            }
        }
    }

    public void rotate(float speedFactor)
    {
        if (fuel > 0 && ! landed)
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * speedFactor) * Time.deltaTime);
            useRotationFuel();
        }
    }
	
	// Update is called once per frame
	void Update () {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y -1), -Vector2.up*10);
        if (hit.collider != null && (hit.transform.tag == "Terrain" || hit.transform.tag == "Platform"))
        {
            float distance = Vector2.Distance(transform.position, hit.point);
            altitudeBar.updateValue(distance);
            if (distance < 5)
                cameraLL.Zooming = true;
        }

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
            horizontalBar.updateValue(Mathf.Abs(currentVelocity.x));
            verticalBar.updateValue(Mathf.Abs(currentVelocity.y));
            if (fuel > 0)
            {
                if (reactorState > 0)
                {
                    rigidbody2D.AddRelativeForce(new Vector3(0, propulsorForce * reactorState, 0));
                    useReactorFuel();
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
        timeBeforeRestart = 1;
        landed = true;
        explosion.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!landed)
        {
            restartTimerStart = Time.time;
            if (collision.collider.tag == "Platform" && collision.relativeVelocity.y > -0.3f && collision.relativeVelocity.x > -0.2f && Mathf.Abs(transform.rotation.eulerAngles.z) < 20)
            {
                timeBeforeRestart = 3;
                score += 10;
                lastPlatformLanded = collision.collider.gameObject.GetComponent<PlatformLunarLander>();
                lastPlatformLanded.lightUp();
                gratzText.gameObject.SetActive(true);
            }
            else
            {
                crash();
            }
            resetForce();
            landed = true;
        }
    }
}
