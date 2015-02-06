﻿using UnityEngine;
using System.Collections;

public class BallMoving : MonoBehaviour {

    private float upScreen, downScreen;
    [SerializeField]
    public Vector2 speed;

    [SerializeField]
    private GameObject manager;
    private PongManagerScript managerScript;

    private bool coupSpecial = false;
    private int coupPlayer;
    private bool fireBall;

    private Vector3 defaultPos;
    private Vector2 defaultSpeed;

    private float restartTimer;

    public void onRestart()
    {
        transform.position = defaultPos;
        speed = new Vector2(defaultSpeed.x, defaultSpeed.y);
        cancelCoupSpecial();
    }

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

    public void onScore()
    {
        restartTimer = Time.time;
    }

    public void OnCoupSpecialStart(int player)
    {
        coupSpecial = true;
        coupPlayer = player;
    }

    public void cancelCoupSpecial()
    {
        coupSpecial = false;
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
