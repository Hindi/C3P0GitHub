using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// GameManager for SpaceWar. Mostly manages restarts and score.
/// </summary>
public class SpaceWarScript : MonoBehaviour {

    [SerializeField]
    private GameObject playerShip;
    [SerializeField]
    private GameObject enemyShip;
    [SerializeField]
    private GameObject playerProjectile;
    [SerializeField]
    private GameObject enemyProjectile;

    [SerializeField]
    private Text scoreText;
    private Vector3 basePlayerPos, baseEnemyPos, basePPPos, baseEPPos;
    private int score = 0;
    public int Score {
        get{return score;} private set{}
    }
    private float aspectRatio;
    [SerializeField]
    private GameObject mainCamera;

	// Use this for initialization
	void Awake () {
        EventManager<bool, int>.AddListener(EnumEvent.SPACESHIPDESTROYED, spaceShipDestroyed);
        basePlayerPos = playerShip.transform.position;
        baseEnemyPos = enemyShip.transform.position;
        basePPPos = playerProjectile.transform.position;
        baseEPPos = enemyProjectile.transform.position;
	}

    void OnDestroy()
    {
        EventManager<bool, int>.RemoveListener(EnumEvent.SPACESHIPDESTROYED, spaceShipDestroyed);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onRestart(bool scoreReset = true)
    {
        playerShip.transform.position = basePlayerPos;
        enemyShip.transform.position = baseEnemyPos;
        playerProjectile.SetActive(false);
        enemyProjectile.SetActive(false);
        playerProjectile.transform.position = basePPPos;
        enemyProjectile.transform.position = baseEPPos;
        playerShip.rigidbody2D.velocity = new Vector2(0, 0);
        enemyShip.rigidbody2D.velocity = new Vector2(0, 0);
        playerProjectile.rigidbody2D.velocity = new Vector2(0, 0);
        enemyProjectile.rigidbody2D.velocity = new Vector2(0, 0);
        playerShip.transform.localEulerAngles = new Vector3(0, 0, 0);
        enemyShip.transform.localEulerAngles = new Vector3(0, 0, 180);
        enemyShip.GetComponent<EnemySpaceWar>().onRestart();
        playerShip.GetComponent<PlayerSpaceWar>().onRestart();
        if (scoreReset)
        {
            scoreText.text = 0.ToString();
            score = 0;
        }

    }

    /// <summary>
    /// Called by the state to set the AI's parameter
    /// </summary>
    /// <param name="p">The new parameter</param>
    public void setParameter(Parameter p)
    {
        enemyShip.GetComponent<EnemySpaceWar>().setParameter(p);
    }

    /// <summary>
    /// Delegate called when one of the ships is destroyed
    /// </summary>
    /// <param name="b">True if player ship, false otherwise</param>
    public void spaceShipDestroyed(bool b, int deltaScore)
    {
        score += deltaScore;
        scoreText.text = score.ToString();
        if (b) // the player has been destroyed
        {
            EventManager<bool>.Raise(EnumEvent.GAMEOVER, true);
        }
        else
        {
            onRestart(false);
        }
    }

    /// <summary>
    /// Used to change the camera's size according to screen size ratio to keep consistent across all platforms
    /// </summary>
    public void updateElementsResolution()
    {
        float width, height;
        width = Math.Max(Screen.width, Screen.height);
        height = Math.Min(Screen.width, Screen.height);
        aspectRatio = width / height;
        mainCamera.GetComponent<Camera>().projectionMatrix = Matrix4x4.Ortho(-5.0f * aspectRatio, 5.0f * aspectRatio, -5.0f, 5.0f, 0.3f, 1000f);
    }
}
