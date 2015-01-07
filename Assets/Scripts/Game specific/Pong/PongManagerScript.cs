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
    private bool fireBall;
    private int player;
    private Vector2 oldSpeed;
    private Vector3 currentAngles;
    private int currentDirection;

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
                ball.GetComponent<BallMoving>().setFireBall(false);
                ball.GetComponent<SpriteRenderer>().sprite = origBall;
                ball.GetComponent<BallMoving>().speed = new Vector2((oldSpeed.x > 0) ? oldSpeed.x : -oldSpeed.x, oldSpeed.y);
                ball.renderer.material.color = Color.white;
            }
        }

        if(timerFrame <= 0)
        {
            timerFrame = timerSeconde * 60;
            ball.GetComponent<BallMoving>().OnCoupSpecialStart(-1);
            player = -1;
        }
        timerFrame--;

        if (coupSpecial)
        {
            ball.transform.position = new Vector3(ball.transform.position.x, playerPaddle.transform.position.y, 0);
            arrow.transform.position = ball.transform.position;
            if (currentAngles.z * player >= 180 || currentAngles.z * player <= 0)
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
        this.oldSpeed = oldSpeed;
        coupSpecial = true;
        arrow.SetActive(true);
        currentAngles = new Vector3(0, 0, player * 90);
        currentDirection = 1;
        arrow.transform.localEulerAngles = currentAngles;
    }
}
