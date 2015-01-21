using UnityEngine;
using System;
using System.Collections;

public class PongManagerScript : MonoBehaviour {

    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject[] texts;
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject playerPaddle, enemyPaddle;
    [SerializeField]
    private Sprite origBall, specialBall;

    [SerializeField]
    private int timerSeconde;
    private float initTime;

    [SerializeField]
    private float specialSpeed;

    private bool coupSpecial;
    private bool coupSpecialCharging = true;
    private bool fireBall;
    private int player;
    private Vector2 oldSpeed;
    private Vector3 currentAngles;
    private int currentDirection;

    public Parameter param;

    private float coupSpecialTimer = -1;
    private int frameTimer = 0;
    private bool animationEnded = true;
    private int coupSpecialTimerTotal;
    private int colorSide = 0;
    [SerializeField]
    private GameObject afficheCoupSpecial;

    private float lastUpdateTime = -1;

    public void onRestart()
    {
        initTime = Time.time;
        lastUpdateTime = -1;
        colorSide = 0;
        animationEnded = true;
        frameTimer = 0;
        coupSpecialTimer = -1;
        coupSpecialCharging = true;
        ball.GetComponent<BallMoving>().onRestart();
        arrow.SetActive(false);
        playerPaddle.GetComponent<PlayerControl>().onRestart();
        enemyPaddle.GetComponent<SuperBasicIA>().onRestart();

    }

	// Use this for initialization
	void Start () {
        initTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time == lastUpdateTime)
        {
            return;
        }
        lastUpdateTime = Time.time;
	    if(ball.transform.position.x < -7)
        {
            texts[0].GetComponent<GUIText>().text = (int.Parse(texts[0].GetComponent<GUIText>().text) + 1).ToString();
            ball.transform.position = new Vector3(0, 0, 0);
            if (fireBall)
            {
                fireBall = false;
                coupSpecialCharging = true;
                ball.GetComponent<BallMoving>().setFireBall(false);
                ball.GetComponent<SpriteRenderer>().sprite = origBall;
                ball.GetComponent<BallMoving>().speed = new Vector2((oldSpeed.x < 0) ? oldSpeed.x : -oldSpeed.x, oldSpeed.y);
                ball.renderer.material.color = Color.white;
            }
        }
        else if (ball.transform.position.x > 7)
        {
            texts[1].GetComponent<GUIText>().text = (int.Parse(texts[1].GetComponent<GUIText>().text) + 1).ToString();
            ball.transform.position = new Vector3(0, 0, 0);
            if (fireBall)
            {
                fireBall = false;
                coupSpecialCharging = true;
                ball.GetComponent<BallMoving>().setFireBall(false);
                ball.GetComponent<SpriteRenderer>().sprite = origBall;
                ball.GetComponent<BallMoving>().speed = new Vector2((oldSpeed.x > 0) ? oldSpeed.x : -oldSpeed.x, oldSpeed.y);
                ball.renderer.material.color = Color.white;
            }
        }

        if (coupSpecialCharging)
        {
            if(Time.time - initTime >= timerSeconde)
            {
                coupSpecialCharging = false;
                initTime = Time.time;
                startCoupSpecialAnimation();
            }
        }
        checkCoupSpecial();
        afficheCoupSpecial.transform.position = new Vector3(colorSide, afficheCoupSpecial.transform.position.y, 0);

        if (coupSpecial)
        {
            ball.transform.position = new Vector3(ball.transform.position.x, playerPaddle.transform.position.y, 0);
            arrow.transform.position = ball.transform.position;
            if (currentAngles.z * player >= 160 || currentAngles.z * player <= 20)
            {
                currentDirection *= -1;
            }
            currentAngles.z += currentDirection;
            arrow.transform.localEulerAngles = currentAngles;

            if (Input.GetKey(KeyCode.Space))
            {
                coupSpecial = false;
                fireBall = true;
                ball.GetComponent<BallMoving>().setFireBall(true);
                ball.transform.Rotate(0,0,currentAngles.z);
                ball.GetComponent<SpriteRenderer>().sprite = specialBall;
                ball.GetComponent<BallMoving>().speed = new Vector2(specialSpeed * player * (float)Math.Sin((double)currentAngles.z * Math.PI / 180), specialSpeed * (float)Math.Cos((double)currentAngles.z * Math.PI / 180));
                arrow.SetActive(false);
            }
        }
	}

    public void activateCoupSpecial(Vector2 oldSpeed)
    {
        afficheCoupSpecial.SetActive(false);
        this.oldSpeed = oldSpeed;
        coupSpecial = true;
        arrow.SetActive(true);
        currentAngles = new Vector3(0, 0, player * 90);
        currentDirection = 1;
        arrow.transform.localEulerAngles = currentAngles;
    }

    public void setParameter(Parameter p)
    {
        param = p;
    }

    private void startCoupSpecialAnimation()
    {
        System.Random rand = new System.Random();
        player = (rand.Next(0,2) == 0) ? -1 : 1;
        coupSpecialTimerTotal = rand.Next(600, 660);
        coupSpecialTimer = Time.time;
        afficheCoupSpecial.SetActive(true);
        animationEnded = false;
    }

    private void checkCoupSpecial()
    {
        if (animationEnded)
        {
            return;
        }
        frameTimer++;
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 0 / 100 && Time.time - coupSpecialTimer < coupSpecialTimerTotal * 10 / 100)
        {
            changeColorSide();
        }
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 10 / 100 && Time.time - coupSpecialTimer < coupSpecialTimerTotal * 30 / 100)
        {
            if (frameTimer % 2 == 0)
            {
                changeColorSide();
            }
        }
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 30 / 100 && Time.time - coupSpecialTimer < coupSpecialTimerTotal * 50 / 100)
        {
            if (frameTimer % 4 == 0)
            {
                changeColorSide();
            }
        }
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 50 / 100 && Time.time - coupSpecialTimer < coupSpecialTimerTotal * 80 / 100)
        {
            if (frameTimer % 8 == 0)
            {
                changeColorSide();
            }
        }
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 80 / 100 && Time.time - coupSpecialTimer < coupSpecialTimerTotal * 90 / 100)
        {
            if (frameTimer % 16 == 0)
            {
                changeColorSide();
            }
        }
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 90 / 100 && Time.time - coupSpecialTimer < coupSpecialTimerTotal * 100 / 100)
        {
            if (frameTimer % 30 == 0)
            {
                changeColorSide();
            }
        }
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 100 / 100)
        {
            Debug.Log("Coup Special granted to " + player);
            colorSide = player;
            ball.GetComponent<BallMoving>().OnCoupSpecialStart(player);
            animationEnded = true;
        }
    }

    private void changeColorSide()
    {
        colorSide = (colorSide == -1) ? 1 : -1;
    }
}
