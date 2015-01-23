using UnityEngine;
using System.Collections;

public class SpaceWarScript : MonoBehaviour {

    [SerializeField]
    private GameObject playerShip, enemyShip, playerProjectile, enemyProjectile;
    private Vector3 basePlayerPos, baseEnemyPos, basePPPos, baseEPPos;
    public int score = 0;

	// Use this for initialization
	void Start () {
        EventManager<bool>.AddListener(EnumEvent.SPACESHIPDESTROYED, spaceShipDestroyed);
        basePlayerPos = playerShip.transform.position;
        baseEnemyPos = enemyShip.transform.position;
        basePPPos = playerProjectile.transform.position;
        baseEPPos = enemyProjectile.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onRestart()
    {
        playerShip.transform.position = basePlayerPos;
        enemyShip.transform.position = baseEnemyPos;
        playerProjectile.transform.position = basePPPos;
        enemyProjectile.transform.position = baseEPPos;
        playerShip.rigidbody2D.velocity = new Vector2(0, 0);
        enemyShip.rigidbody2D.velocity = new Vector2(0, 0);
        playerProjectile.rigidbody2D.velocity = new Vector2(0, 0);
        enemyProjectile.rigidbody2D.velocity = new Vector2(0, 0);
        playerShip.transform.localEulerAngles = new Vector3(0, 0, 0);
        enemyShip.transform.localEulerAngles = new Vector3(0, 0, 180);


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
}
