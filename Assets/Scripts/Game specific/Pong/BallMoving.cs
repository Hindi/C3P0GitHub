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


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y > upScreen)
        {
            speed.y = (speed.y < 0) ? speed.y : -1 * speed.y;
            if (fireBall)
            {
                transform.Rotate(new Vector3(0, 0, 2 * Mathf.Atan2(speed.y, speed.x) * Mathf.Rad2Deg));
            }
        }
        else if (transform.position.y < -downScreen)
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

        if (((coupPlayer == -1 && transform.position.x < 0) || (coupPlayer == 1 && transform.position.x > 0)) && coupSpecial)
        {
            manager.GetComponent<PongManagerScript>().activateCoupSpecial(speed);
            speed = new Vector2(0, 0);
            coupSpecial = false;
        }
        else
        {
            speed.x *= -1;
            if (fireBall)
            {
                transform.Rotate(new Vector3(0, 0, 180));
            }
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
