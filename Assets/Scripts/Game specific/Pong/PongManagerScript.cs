﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class PongManagerScript : MonoBehaviour {

    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private Text[] texts;
    [SerializeField]
    private GameObject[] limits;
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject playerPaddle, enemyPaddle;
    [SerializeField]
    private Sprite origBall, specialBall;

    [SerializeField]
    private float upScreen, downScreen;

    [NonSerialized]
    public float resizeHeight, resizeWidth;

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
    private float coupSpecialTimerTotal;
    private int colorSide = 0;
    [SerializeField]
    private GameObject afficheCoupSpecial;
    private float afficheCoupSpecialInitialY;
    [SerializeField]
    private GameObject[] spectateursGauche, spectateursDroite;
    [SerializeField]
    private float applaudTimer;
    private float initTimerGauche, initTimerDroite;
    private bool applauds = false;

    private float lastUpdateTime = -1;

    public int playerScore
    {
        get
        {
            return int.Parse(texts[1].text);
        }
        private set { }
    }
    public int enemyScore
    {
        get
        {
            return int.Parse(texts[0].text);
        }
        private set {}
    }

    public void onRestart()
    {
        lastUpdateTime = -1;
        colorSide = 0;
        animationEnded = true;
        frameTimer = 0;
        coupSpecialTimer = -1;

        reset(-1);

        arrow.SetActive(false);
        playerPaddle.GetComponent<PlayerControl>().onRestart(resizeWidth, resizeHeight);
        enemyPaddle.GetComponent<SuperBasicIA>().onRestart(resizeWidth, resizeHeight);
        texts[0].text = 0.ToString();
        texts[1].text = 0.ToString();

    }

	// Use this for initialization
	void Start () {
        initTime = Time.time;
        ball.GetComponent<BallMoving>().setScreenSize(upScreen, downScreen);
        afficheCoupSpecialInitialY = afficheCoupSpecial.transform.position.y;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Time.time == lastUpdateTime)
        {
            return;
        }
        lastUpdateTime = Time.time;
        /*
         * Updates sur l'état du jeu
         */
        // Vérification de la sortie de la balle
	    if(ball.transform.position.x < -7 * resizeWidth)
        {
            onScore(1);
        }
        else if (ball.transform.position.x > 7 * resizeWidth)
        {
            onScore(-1);
        }
        // Mise à jour de la direction de l'aiguille pendant un coup spécial
        if (coupSpecial)
        {
            if (player == -1)
                ball.transform.position = new Vector3(playerPaddle.transform.position.x + 0.1f, playerPaddle.transform.position.y, 0);
            else
                ball.transform.position = new Vector3(enemyPaddle.transform.position.x - 0.1f, enemyPaddle.transform.position.y, 0);
            arrow.transform.position = ball.transform.position;
            if (currentAngles.z * player >= 160 || currentAngles.z * player <= 20)
            {
                currentDirection *= -1;
            }
            currentAngles.z += currentDirection;
            arrow.transform.localEulerAngles = currentAngles;

        }

        /*
         * Updates des animations
         */
        // Animation du coup spécial
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
        afficheCoupSpecial.transform.position = new Vector3(0.5f * colorSide * resizeWidth, afficheCoupSpecialInitialY * resizeHeight, 0);

        // Animation des personnages qui applaudissent
        if (applauds)
        {
            if (Time.time - initTimerGauche > applaudTimer)
            {
                foreach (GameObject g in spectateursGauche)
                {
                    if (g != null)
                        g.GetComponent<Animator>().SetBool("applaud", false);
                }
                applauds = false;
            }
            if (Time.time - initTimerDroite > applaudTimer)
            {
                foreach (GameObject g in spectateursDroite)
                {
                    if (g != null)
                        g.GetComponent<Animator>().SetBool("applaud", false);
                }
                applauds = false;
            }
        }
	}

    private void onScore(int player)
    {
        texts[((player == 1) ? 0 : 1)].text = (int.Parse( texts[((player == 1) ? 0 : 1)].text ) + 1).ToString();
        if (int.Parse(texts[((player == 1) ? 0 : 1)].text ) >= 10)
        {
            EventManager<bool>.Raise(EnumEvent.GAMEOVER, false); // la partie est finie
        }
        if (player == -1)
        {
            initTimerGauche = Time.time;
            foreach (GameObject g in spectateursGauche)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", true);
            }
            applauds = true;
        }
        else
        {
            initTimerGauche = Time.time;
            foreach (GameObject g in spectateursDroite)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", true);
            }
            applauds = true;
        }
        reset(player);
    }

    private void reset(int player)
    {
        initTime = Time.time;
        coupSpecialCharging = true;
        afficheCoupSpecial.SetActive(false);
        animationEnded = true;
        ball.transform.position = new Vector3(0, 0, 0);
        ball.GetComponent<SpriteRenderer>().sprite = origBall;
        ball.renderer.material.color = Color.white;
        ball.GetComponent<BallMoving>().cancelCoupSpecial();
        coupSpecial = false;

        if (fireBall)
        {
            fireBall = false;
            ball.GetComponent<BallMoving>().setFireBall(false);
            ball.GetComponent<BallMoving>().speed = new Vector2((oldSpeed.x < 0) ? player * oldSpeed.x : player * -oldSpeed.x, oldSpeed.y);
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
        if (player == 1)
            enemyPaddle.GetComponent<SuperBasicIA>().getCoupSpecial(this);
    }

    public void setParameter(Parameter p)
    {
        param = p;
    }

    private void startCoupSpecialAnimation()
    {
        System.Random rand = new System.Random();
        player = (rand.Next(0,2) == 0) ? -1 : 1;
        coupSpecialTimerTotal = rand.Next(10, 11);
        coupSpecialTimer = Time.time;
        afficheCoupSpecial.SetActive(true);
        animationEnded = false;
    }

    public void launchCoupSpecial(int p)
    {
        if (!coupSpecial || p != player)
            return;
        coupSpecial = false;
        fireBall = true;
        ball.GetComponent<BallMoving>().setFireBall(true);
        ball.transform.Rotate(0, 0, currentAngles.z);
        ball.GetComponent<SpriteRenderer>().sprite = specialBall;
        ball.GetComponent<BallMoving>().speed = new Vector2(specialSpeed * -1 * (float)Math.Sin((double)currentAngles.z * Math.PI / 180), specialSpeed * (float)Math.Cos((double)currentAngles.z * Math.PI / 180));
        arrow.SetActive(false);
    }

    private void checkCoupSpecial()
    {
        if (animationEnded)
        {
            return;
        }
        frameTimer++;
        /*
         * Cette section a été commentée parce que l'indicateur changeait trop souvent de côté pour que ce soit visible
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
         * */
        if (Time.time - coupSpecialTimer >= coupSpecialTimerTotal * 0 / 100 && Time.time - coupSpecialTimer < coupSpecialTimerTotal * 50 / 100)
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
            colorSide = player;
            ball.GetComponent<BallMoving>().OnCoupSpecialStart(player);
            animationEnded = true;
        }
    }

    private void changeColorSide()
    {
        colorSide = (colorSide == -1) ? 1 : -1;
    }

    public void updateElementsResolution()
    {
        if (Application.isMobilePlatform) // application en mode portrait, on doit inverser width et height
        {
            resizeWidth = Screen.height / 1024f;
            resizeHeight = Screen.width / 768f;
        }
        else
        {
            resizeWidth = Screen.width / 1024f;
            resizeHeight = Screen.height / 768f;
        }
        limits[0].transform.position = new Vector3(0, resizeHeight * upScreen + 0.5f, 0);
        limits[1].transform.position = new Vector3(0, resizeHeight * downScreen * -1 - 0.4f, 0);
    }
}
