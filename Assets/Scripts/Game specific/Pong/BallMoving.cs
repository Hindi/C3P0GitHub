using UnityEngine;
using System.Collections;

/// <summary>
/// Script attached to the ball in Pong
/// </summary>
public class BallMoving : MonoBehaviour {

    /// <summary>
    /// The size of the screen, not used anymore
    /// </summary>
    private float upScreen, downScreen;
    /// <summary>
    /// The ball's speed
    /// </summary>
    [SerializeField]
    public Vector2 speed;

    /// <summary>
    /// The Pong Manager gameobject, that contains the PongManagerScript
    /// </summary>
    [SerializeField]
    private GameObject manager;
    /// <summary>
    /// The manager script to interact with for the special shot
    /// </summary>
    private PongManagerScript managerScript;

    /// <summary>
    /// True if the special shot is granted to a player, so the ball has to stop on his pallet
    /// </summary>
    private bool coupSpecial = false;
    /// <summary>
    /// -1 if special shot is to be granted to left player, 1 if right player
    /// </summary>
    private int coupPlayer;
    /// <summary>
    /// True if the current form of the ball is a fireball
    /// </summary>
    private bool fireBall;

    /// <summary>
    /// The position at the beginning of the game
    /// </summary>
    private Vector3 defaultPos;
    /// <summary>
    /// The speed at the beginning of the game
    /// </summary>
    private Vector2 defaultSpeed;

    /// <summary>
    /// Timer to restart moving when the ball is reset
    /// </summary>
    private float restartTimer;

    /// <summary>
    /// Called when the game restarts
    /// </summary>
    public void onRestart()
    {
        restartTimer = 3;
        transform.position = defaultPos;
        speed = new Vector2(defaultSpeed.x, defaultSpeed.y);
        cancelCoupSpecial();
    }

    /// <summary>
    /// Sets the screen size in Unity's measures.<br/>
    /// Used to be used to resize neatly, do not use anymore
    /// </summary>
    /// <param name="uScreen"></param>
    /// <param name="dScreen"></param>
    public void setScreenSize(float uScreen, float dScreen)
    {
        upScreen = uScreen;
        downScreen = dScreen;
    }


	// Use this for initialization
    void Start()
    {
        managerScript = manager.GetComponent<PongManagerScript>();
        defaultPos = transform.position;
        defaultSpeed = new Vector2(speed.x, speed.y);
        //Laws.setUniformValues(new double[] { 45, -45 });
	}
	
	// Update is called once per frame
    /// <summary>
    /// Checks if the ball is going out of screen vertically, and applies speed
    /// </summary>
	void Update () {
        if (Time.time - restartTimer <= 3)
        {
            return;
        }

        if (transform.position.y > upScreen * managerScript.resizeHeight && speed.y > 0)
        {
            speed.y = (speed.y < 0) ? speed.y : -1 * speed.y;
            if (fireBall)
            {
                transform.Rotate(new Vector3(0, 0, 2 * Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg));
            }
        }
        else if (transform.position.y < -downScreen * managerScript.resizeHeight && speed.y < 0)
        {
            speed.y = (speed.y > 0) ? speed.y : -1 * speed.y;
            if (fireBall)
            {
                transform.Rotate(new Vector3(0, 0, 2 * Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg));
            }
        }

        transform.Translate(new Vector3(speed.x * Time.deltaTime * managerScript.resizeWidth, speed.y * Time.deltaTime * managerScript.resizeHeight, 0), Space.World);
	}

    /**
     * When the ball hits a Pallet and goes back
     * This is where the whole "random" collision has to be made
     **/
    /// <summary>
    /// Called when the ball hits a pallet
    /// </summary>
    void OnTriggerEnter2D(Collider2D obstacle)
    {
        PongManagerScript managerScript = manager.GetComponent<PongManagerScript>();
        if (((coupPlayer == -1 && transform.position.x < 0) || (coupPlayer == 1 && transform.position.x > 0)) && coupSpecial)
        {
            managerScript.activateCoupSpecial(speed);
            speed = new Vector2(0, 0);
            coupSpecial = false;
        }
        else
        {
            speed.x *= -1;
            float norm = Mathf.Sqrt(speed.x * speed.x + speed.y * speed.y);
            float Angle = 0;
            if (transform.position.x < 0) // côté joueur
            {
                Angle = Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg;
            }
            else
            {
                /* Here is the magic stuff ! */
                if (managerScript.param.id == 0)
                {
                    Angle = (float) Laws.uniforme(-80,81);
                }
                else if (managerScript.param.id == 1)
                {
                    Angle += -80 + 2 * (float)Laws.poisson(4);
                }
                else if (managerScript.param.id == 2)
                {
                    Angle += ((float)Laws.binom(0.5,10) - 5) * 16;
                }
                if (Angle > 80)
                    Angle = 80;
                if (Angle < -80)
                    Angle = 80;

                speed.x = Mathf.Cos(Angle * Mathf.Deg2Rad) * norm * ((transform.position.x > 0) ? -1 : 1);
                speed.y = Mathf.Sin(Angle * Mathf.Deg2Rad) * norm;

            }

            if (fireBall)
            {
                transform.localEulerAngles = new Vector3(0, 0, 90 + ((transform.position.x > 0) ? 0 : 180) + ((transform.position.x > 0) ? -1 : 1) * Angle);
            }
        }
        
    }

    /// <summary>
    /// Called when a player gets a point
    /// </summary>
    /// <param name="posX">The position to respawn at on X axis</param>
    public void onScore(float posX)
    {
        speed = new Vector2(defaultSpeed.x * -1 * posX/Mathf.Abs(posX), defaultSpeed.y);
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        restartTimer = Time.time;
    }

    /// <summary>
    /// Activates a special shot and sets the player that has it
    /// </summary>
    /// <param name="player">-1 if left player, 1 if right player</param>
    public void OnCoupSpecialStart(int player)
    {
        coupSpecial = true;
        coupPlayer = player;
    }

    /// <summary>
    /// Cancels a special shot that had already been granted
    /// </summary>
    public void cancelCoupSpecial()
    {
        coupSpecial = false;
    }

    /// <summary>
    /// Sets if the fireball mode is activated
    /// </summary>
    /// <param name="arg">True if fireball is activated, false otherwise</param>
    public void setFireBall(bool arg)
    {
        fireBall = arg;
        if (!arg)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
