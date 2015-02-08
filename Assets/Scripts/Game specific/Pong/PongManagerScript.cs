using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// The main script of Pong that manages everything
/// </summary>
public class PongManagerScript : MonoBehaviour {

    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject textCanvas;
    [SerializeField]
    private GameObject perduCanvas;
    [SerializeField]
    private Text[] texts;
    [SerializeField]
    private GameObject[] limits;
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject playerPaddle;
    [SerializeField]
    private GameObject enemyPaddle;
    [SerializeField]
    private Sprite origBall;
    [SerializeField]
    private Sprite specialBall;
    [SerializeField]
    private GameObject pointilles;

    [SerializeField]
    private float upScreen;
    [SerializeField]
    private float downScreen;

    [NonSerialized]
    public float resizeHeight;
    [NonSerialized]
    public float resizeWidth;
    private float aspectRatio;


    [SerializeField]
    private float specialSpeed;

    /* Déplacement de la balle */
    private Vector2 oldSpeed;
    private Vector3 currentAngles;
    private int currentDirection;

    public Parameter param;

    /* Coup spécial */
    private float coupSpecialTimer = -1;
    private int frameTimer = 0;
    private bool animationEnded = true;
    private float coupSpecialTimerTotal;
    private int colorSide = 0;
    [SerializeField]
    private GameObject afficheCoupSpecial;
    private float afficheCoupSpecialInitialY;
    private bool coupSpecial;
    private bool coupSpecialCharging = true;
    private bool fireBall;
    private int player;
    [SerializeField]
    private int timerSeconde;
    private float initTime;

    /* Spectateurs pendant le jeu */
    [SerializeField]
    private GameObject[] spectateursGauche;
    [SerializeField]
    private GameObject[] spectateursDroite;
    [SerializeField]
    private GameObject groupeGauche;
    [SerializeField]
    private GameObject groupeDroite;
    [SerializeField]
    private float applaudTimer;
    private float initTimerGauche, initTimerDroite;
    private bool applaudsGauche = false, applaudsDroite = false;


    /* Fin du jeu */
    /* Victoire */
    [SerializeField]
    private GameObject spectateursFin;
    [SerializeField]
    private GameObject gradins;
    private bool gameOver = false, gameLost = false;
    private Matrix4x4 initCameraMatrix;
    private Vector3 lookAtTarget = new Vector3(0, 0, 0);
    /* Defaite */
    [SerializeField]
    private GameObject libellule;

    /* Ne pas faire les updates si le jeu est en pause */
    private float lastUpdateTime = -1;

    /* Utilisé pour le score envoyé au serveur */
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

    /// <summary>
    /// Full restart of the game
    /// </summary>
    public void onRestart()
    {
        gameOver = false;
        gameLost = false;

        mainCamera.GetComponent<Camera>().projectionMatrix = initCameraMatrix;
        mainCamera.transform.position = new Vector3(0, 0, -10);
        textCanvas.SetActive(true);
        perduCanvas.SetActive(false);
        lookAtTarget = new Vector3(0, 0, 0);
        mainCamera.transform.LookAt(lookAtTarget);
        mainCamera.GetComponent<Camera>().backgroundColor = new Color(0, 0, 0);

        lastUpdateTime = -1;
        colorSide = 0;
        animationEnded = true;
        frameTimer = 0;
        coupSpecialTimer = -1;

        reset(-1);

        arrow.SetActive(false);
        playerPaddle.GetComponent<PlayerControl>().onRestart(resizeWidth, resizeHeight);
        enemyPaddle.GetComponent<SuperBasicIA>().onRestart(resizeWidth, resizeHeight);
        ball.SetActive(true);
        ball.GetComponent<BallMoving>().onRestart();

        groupeGauche.SetActive(true);
        groupeDroite.SetActive(true);
        spectateursFin.SetActive(false);

        texts[0].text = 0.ToString();
        texts[1].text = 0.ToString();


        if (applaudsGauche)
        {
            foreach (GameObject g in spectateursGauche)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", false);
            }
            applaudsGauche = false;
        }
        if (applaudsDroite)
        {
            foreach (GameObject g in spectateursDroite)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", false);
            }
            applaudsDroite = false;
        }

        libellule.GetComponent<Libellule>().destroyPoints();
        libellule.GetComponent<Libellule>().restart();

    }

	// Use this for initialization
	void Start () {
        initTime = Time.time;
        ball.GetComponent<BallMoving>().setScreenSize(upScreen, downScreen);
        afficheCoupSpecialInitialY = afficheCoupSpecial.transform.position.y;
        initCameraMatrix = mainCamera.GetComponent<Camera>().projectionMatrix;
        perduCanvas.SetActive(false);
	}
	
	// Update is called once per frame
    /// <summary>
    /// Checks if a player scored, special shot and animations
    /// </summary>
    void Update()
    {
        if (gameOver)
        {
            onGameEnd();
            return;
        }
        if (gameLost)
        {
            return;
        }

        if (Time.time == lastUpdateTime)
        {
            return;
        }
        lastUpdateTime = Time.time;
        /*
         * Updates sur l'état du jeu
         */
        // Vérification de la sortie de la balle
	    if(ball.transform.position.x < -6 * resizeWidth)
        {
            onScore(1);
        }
        else if (ball.transform.position.x > 6 * resizeWidth)
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
        if (Time.time - initTimerGauche > applaudTimer && applaudsGauche)
        {
            foreach (GameObject g in spectateursGauche)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", false);
            }
            applaudsGauche = false;
        }
        if (Time.time - initTimerDroite > applaudTimer && applaudsDroite)
        {
            foreach (GameObject g in spectateursDroite)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", false);
            }
            applaudsDroite = false;
        }
	}

    /// <summary>
    /// Makes a player get a point
    /// </summary>
    /// <param name="player">-1 if left player scored, 1 if right player</param>
    private void onScore(int player)
    {
        texts[((player == 1) ? 0 : 1)].text = (int.Parse( texts[((player == 1) ? 0 : 1)].text ) + 1).ToString();
        if (player == -1)
        {
            initTimerGauche = Time.time;
            foreach (GameObject g in spectateursGauche)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", true);
            }
            applaudsGauche = true;
        }
        else
        {
            initTimerDroite = Time.time;
            foreach (GameObject g in spectateursDroite)
            {
                if (g != null)
                    g.GetComponent<Animator>().SetBool("applaud", true);
            }
            applaudsDroite = true;
        }
        reset(player);

        if (int.Parse(texts[((player == 1) ? 0 : 1)].text) >= 10)
        {
            ball.transform.position = new Vector3(10, 3, -20);
            ball.SetActive(false);
            if (player == -1)
            {
                gameOver = true;
                mainCamera.GetComponent<MatrixBlender>().BlendToMatrix(Matrix4x4.Perspective(53, aspectRatio, 0.3f, 1000), 0);
                textCanvas.SetActive(false);
            }
            else onGameLost();
        }

    }

    /// <summary>
    /// Resets the round after a player scored
    /// </summary>
    /// <param name="player">-1 if left player scored, 1 if right player</param>
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
        ball.GetComponent<BallMoving>().onScore(((player == -1) ? playerPaddle : enemyPaddle).transform.position.x * 4 / (float)5);
        coupSpecial = false;

        if (fireBall)
        {
            fireBall = false;
            ball.GetComponent<BallMoving>().setFireBall(false);
            ball.GetComponent<BallMoving>().speed = new Vector2((oldSpeed.x < 0) ? player * oldSpeed.x : player * -oldSpeed.x, oldSpeed.y);
        }
    }

    /// <summary>
    /// Activates a special shot, stores the speed before the special shot
    /// </summary>
    /// <param name="oldSpeed"></param>
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

    /// <summary>
    /// Sets the current game parameter
    /// </summary>
    /// <param name="p">The new parameter</param>
    public void setParameter(Parameter p)
    {
        param = p;
    }

    /// <summary>
    /// Starts the special shot animation
    /// </summary>
    private void startCoupSpecialAnimation()
    {
        System.Random rand = new System.Random();
        player = (rand.Next(0,2) == 0) ? -1 : 1;
        coupSpecialTimerTotal = rand.Next(10, 11);
        coupSpecialTimer = Time.time;
        afficheCoupSpecial.SetActive(true);
        animationEnded = false;
    }

    /// <summary>
    /// Starts the actual shot instead of having the ball stuck on the paddle
    /// </summary>
    /// <param name="p">The player who shoots</param>
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

    /// <summary>
    /// Checks if the special shot charging animation needs to be rendered and makes the animation
    /// </summary>
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

    /// <summary>
    /// Changes the charging animation's side
    /// </summary>
    private void changeColorSide()
    {
        colorSide = (colorSide == -1) ? 1 : -1;
    }

    private bool firstTimeTurning;
    /// <summary>
    /// Animation chen the player wins
    /// </summary>
    private void onGameEnd()
    {
        if (Vector3.Distance(mainCamera.transform.position, new Vector3(0, 0, 0)) > 0.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(0, 0, 0), 3 * Time.unscaledDeltaTime);
            firstTimeTurning = true;
        }
        else
        {
            if (firstTimeTurning)
            {
                firstTimeTurning = false;
                groupeGauche.SetActive(false);
                groupeDroite.SetActive(false);
                spectateursFin.SetActive(true);
                EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
            }
            lookAtTarget = Vector3.Lerp(lookAtTarget, new Vector3(0,10,-0.4f), Time.unscaledDeltaTime);
            mainCamera.transform.LookAt(lookAtTarget);
        }
    }

    /// <summary>
    /// Activates the animation when the player has lost
    /// </summary>
    private void onGameLost()
    {
        gameLost = true;
        //mainCamera.GetComponent<Camera>().backgroundColor = new Color(120f/255, 120f/255, 120f/255);
        textCanvas.SetActive(false);
        perduCanvas.SetActive(true);
        mainCamera.transform.position = new Vector3(0, 0, 20);
        libellule.SetActive(true);
        libellule.GetComponent<Libellule>().activate();
    }

    /// <summary>
    /// Changes the camera size to ajust to screen size. In game element size not changed anymore
    /// </summary>
    public void updateElementsResolution()
    {
        resizeHeight = 1;
        resizeWidth = 1;
        aspectRatio = (float) Screen.width / Screen.height;
        mainCamera.GetComponent<Camera>().projectionMatrix = Matrix4x4.Ortho(-5.0f * aspectRatio, 5.0f * aspectRatio, -5.0f, 5.0f, 0.3f, 1000f);
    }
}
