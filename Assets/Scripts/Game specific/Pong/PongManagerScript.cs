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
    private GameObject playerPaddle;
    [SerializeField]
    private Sprite origBall, specialBall;

    [SerializeField]
    private int timerSeconde;
    private int timerFrame;

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

    private int coupSpecialTimer = -1;
    private int coupSpecialTimerTotal;
    private int colorSide = 0;
    [SerializeField]
    private GameObject afficheCoupSpecial;

	// Use this for initialization
	void Start () {
        timerFrame = timerSeconde * 60;
	}
	
	// Update is called once per frame
	void Update () {
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

        if(timerFrame <= 0)
        {
            coupSpecialCharging = false;
            timerFrame = timerSeconde * 60;
            startCoupSpecialAnimation();
        }
        if (coupSpecialCharging)
        {
            timerFrame--;
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
        coupSpecialTimer = coupSpecialTimerTotal;
        afficheCoupSpecial.SetActive(true);
    }

    private void checkCoupSpecial()
    {
        if (coupSpecialTimer < 0)
        {
            return;
        }
        if (coupSpecialTimer <= coupSpecialTimerTotal * 100 / 100 && coupSpecialTimer > coupSpecialTimerTotal * 90 / 100)
        {
            changeColorSide();
        }
        if (coupSpecialTimer <= coupSpecialTimerTotal * 90 / 100 && coupSpecialTimer > coupSpecialTimerTotal * 70 / 100)
        {
            if (coupSpecialTimer % 2 == 0)
            {
                changeColorSide();
            }
        }
        if (coupSpecialTimer <= coupSpecialTimerTotal * 70 / 100 && coupSpecialTimer > coupSpecialTimerTotal * 50 / 100)
        {
            if (coupSpecialTimer % 4 == 0)
            {
                changeColorSide();
            }
        }
        if (coupSpecialTimer <= coupSpecialTimerTotal * 50 / 100 && coupSpecialTimer > coupSpecialTimerTotal * 20 / 100)
        {
            if (coupSpecialTimer % 8 == 0)
            {
                changeColorSide();
            }
        }
        if (coupSpecialTimer <= coupSpecialTimerTotal * 20 / 100 && coupSpecialTimer > coupSpecialTimerTotal * 10 / 100)
        {
            if (coupSpecialTimer % 16 == 0)
            {
                changeColorSide();
            }
        }
        if (coupSpecialTimer <= coupSpecialTimerTotal *  10 / 100 && coupSpecialTimer > 0)
        {
            if (coupSpecialTimer % 30 == 0)
            {
                changeColorSide();
            }
        }
        if (coupSpecialTimer == 0)
        {
            Debug.Log("Coup Special granted to " + player);
            colorSide = player;
            ball.GetComponent<BallMoving>().OnCoupSpecialStart(player);
        }
        coupSpecialTimer--;
    }

    private void changeColorSide()
    {
        colorSide = (colorSide == -1) ? 1 : -1;
    }
}
