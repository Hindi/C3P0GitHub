using UnityEngine;
using System.Collections;

public class BallMoving : MonoBehaviour {

    [SerializeField]
    private float upScreen, downScreen;
    [SerializeField]
    public Vector2 speed;

    [SerializeField]
    private GameObject manager;

    private bool coupSpecial = false;
    private int coupPlayer;
    private bool fireBall;

    private Vector3 defaultPos;
    private Vector2 defaultSpeed;

    public void onRestart()
    {
        transform.position = defaultPos;
        speed = defaultSpeed;
        coupSpecial = false;
    }


	// Use this for initialization
	void Start () {
        defaultPos = transform.position;
        defaultSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y > upScreen && speed.y > 0)
        {
            speed.y = (speed.y < 0) ? speed.y : -1 * speed.y;
            if (fireBall)
            {
                transform.Rotate(new Vector3(0, 0, 2 * Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg));
            }
        }
        else if (transform.position.y < -downScreen && speed.y < 0)
        {
            speed.y = (speed.y > 0) ? speed.y : -1 * speed.y;
            if (fireBall)
            {
                transform.Rotate(new Vector3(0, 0, 2 * Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg));
            }
        }
        transform.Translate(new Vector3(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0), Space.World);
	}

    /**
     * When the ball hits a Pallet and goes back
     * This is where the whole "random" collision has to be made
     **/
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
            if (fireBall)
            {
                transform.Rotate(new Vector3(0, 0, -2 * Mathf.Atan2(speed.x, speed.y) * Mathf.Rad2Deg));
            }/*
            if (managerScript.param.id == 0)
            {
                // work some magic
            }
            else if (managerScript.param.id == 1)
            {
                // work some other magic
            }
            else if (managerScript.param.id == 2)
            {
                // work some more magic
            }*/
        }
        
    }

    public void OnCoupSpecialStart(int player)
    {
        coupSpecial = true;
        coupPlayer = player;
    }

    public void setFireBall(bool arg)
    {
        fireBall = arg;
        if (!arg)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
