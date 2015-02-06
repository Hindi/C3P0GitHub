using UnityEngine;
using System;
using System.Collections;

public class SpaceWarScript : MonoBehaviour {

    [SerializeField]
    private GameObject playerShip, enemyShip, playerProjectile, enemyProjectile;
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
        EventManager<bool>.AddListener(EnumEvent.SPACESHIPDESTROYED, spaceShipDestroyed);
        basePlayerPos = playerShip.transform.position;
        baseEnemyPos = enemyShip.transform.position;
        basePPPos = playerProjectile.transform.position;
        baseEPPos = enemyProjectile.transform.position;
	}

    void OnDestroy()
    {
        EventManager<bool>.RemoveListener(EnumEvent.SPACESHIPDESTROYED, spaceShipDestroyed);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onRestart()
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

    }

    public void setParameter(Parameter p)
    {
        enemyShip.GetComponent<EnemySpaceWar>().setParameter(p);
    }

    public void spaceShipDestroyed(bool b)
    {
        if (b) // the player has been destroyed
        {
            EventManager<bool>.Raise(EnumEvent.GAMEOVER, true);
        }
        else
        {
            score++;
            onRestart();
        }
    }

    public void updateElementsResolution()
    {
        float width, height;
        width = Math.Max(Screen.width, Screen.height);
        height = Math.Min(Screen.width, Screen.height);
        aspectRatio = width / height;
        mainCamera.GetComponent<Camera>().projectionMatrix = Matrix4x4.Ortho(-5.0f * aspectRatio, 5.0f * aspectRatio, -5.0f, 5.0f, 0.3f, 1000f);
    }
}
