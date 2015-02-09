using UnityEngine;
using System.Collections;

/// <summary>
/// Stores and update the Aliens.
/// </summary>
public class AliensManager : MonoBehaviour {


    /// <summary>Reference to the player script.</summary>
    [SerializeField]
    Player player;

    /// <summary>Reference to the ball (breakout mode).</summary>
    [SerializeField]
    GameObject ball;

    /// <summary>Prefab of the first alien.</summary>
    [SerializeField]
    private GameObject alien1;

    /// <summary>Prefab of the second alien.</summary>
    [SerializeField]
    private GameObject alien2;

    /// <summary>Prefab of the third alien.</summary>
    [SerializeField]
    private GameObject alien3;

    /// <summary>Alien move cooldown.</summary>
    [SerializeField]
    private float coolDown;

    /// <summary>X position of the first alien at start.</summary>
    [SerializeField]
    private int startX;

    /// <summary>Y position of the first alien at start.</summary>
    [SerializeField]
    private int startZ;

    /// <summary>Space between aliens (X axis).</summary>
    [SerializeField]
    private int spaceBetweenAliens;

    /// <summary>Space between aliens (Y axis).</summary>
    [SerializeField]
    private int spaceBetweenLines;

    /// <summary>Alien count per line.</summary>
    [SerializeField]
	private int aliensPerLine;

    /// <summary>Alien fire cooldown.</summary>
	[SerializeField]
	private float fireCooldown;

    /// <summary>STime before and between breakout mode.</summary>
    [SerializeField]
    private float breakOutCooldown;

    /// <summary>Time reference to check if it's beakout time.</summary>
    private float breakOutStartTime;

    /// <summary>Direction of the aliens.</summary>
    private int direction;

    /// <summary>True if we need to change direction.</summary>
    private bool changeDirection;

    /// <summary>True if we're in breakout mode.</summary>
    private bool breakOutMode;

    /// <summary>Time when alien moved the last time.</summary>
    private float lastMoveTime;

    /// <summary>WAlien death count.</summary>
	private int deathCount;

    /// <summaryAlien count.</summary>
	private int alienCount;

    /// <summary>Victory count.</summary>
	private int victoryCount;

    /// <summary>Last time an alien fired.</summary>
	private float lastFireTime;

    /// <summary>Spawn the aliens and register callbacks for events.</summary>
    /// <returns>void</returns>
	void Start () {
        start();
        EventManager.AddListener(EnumEvent.ENEMYDEATH, onAlienDeath);
        EventManager.AddListener(EnumEvent.RESTARTGAME, onRestartGame);
	}

    /// <summary>Unregister callbacks for events.</summary>
    /// <returns>void</returns>
    void OnDestroy()
    {
        EventManager.RemoveListener(EnumEvent.ENEMYDEATH, onAlienDeath);
        EventManager.RemoveListener(EnumEvent.RESTARTGAME, onRestartGame);
    }

    /// <summary>Spawn the aliens and init timers.</summary>
    /// <returns>void</returns>
    void start()
    {
        spawnAliens();
        alienCount = transform.childCount;
        deathCount = 0;
        victoryCount = 1;
        direction = 1;
        changeDirection = false;
        breakOutMode = false;
        lastFireTime = Time.time;
        breakOutStartTime = Time.time;
        lastMoveTime = Time.time;
    }

    /// <summary>Switch to normal mode and destroy the aliens.</summary>
    /// <returns>void</returns>
    void end()
    {
        switchBackToNormal();
        foreach (Transform o in transform)
        {
            o.GetComponent<Invader>().destroy();
            Destroy(o.gameObject);
        }
    }

    /// <summary>Check if aliens can fire.</summary>
    /// <returns>True if can fire</returns>
	bool canFire()
	{
		return (Time.time - lastFireTime > fireCooldown / (2+victoryCount));
	}

    /// <summary>Check if it's time to switch to breakout mode.</summary>
    /// <returns>True if it's time</returns>
    bool isBreakOutTime()
    {
        return (Time.time - breakOutStartTime > breakOutCooldown && !breakOutMode);
    }

    /// <summary>Spawn the aliens.</summary>
    /// <returns>void</returns>
	private void spawnAliens()
	{
		summonLine(0, alien1);
		summonLine(spaceBetweenLines, alien1);
		summonLine(spaceBetweenLines * 2, alien2);
		//summonLine(spaceBetweenLines * 3, alien2);
		summonLine(spaceBetweenLines * 3, alien3);
	}

    /// <summary>Called when an alien dies. Increase the counts and check for victory.</summary>
    /// <returns>void</returns>
	public void onAlienDeath()
	{
		deathCount++;
		if (deathCount == alienCount)
        {
            EventManager<bool>.Raise(EnumEvent.RESTARTGAME, true);
            victoryCount++;
        }
	}

    /// <summary>Calls end and start when it's time to restart.</summary>
    /// <returns>void</returns>
    public void onRestartGame()
    {
        end();
        start();
    }

    /// <summary>Spawn a line of alien.</summary>
    /// <returns>void</returns>
    private void summonLine(int z, GameObject prefab)
    {
        GameObject temp;
        for (int i = 0; i <= aliensPerLine; ++i)
        {
            temp = (GameObject)Instantiate(prefab, new Vector3(startX + i * spaceBetweenAliens, 1, startZ + z), Quaternion.identity);
            temp.transform.parent = transform;
        }
    }

    /// <summary>Called when an alien reaches the border of the terrain. Start eh change direction process.</summary>
    /// <returns>void</returns>
    public void reverseDirection()
    {
        changeDirection = true;
    }

    /// <summary>Update the aliens position.</summary>
    /// <returns>void</returns>
    private void moveAliens()
    {
        Vector3 deltaPos = new Vector3(direction * spaceBetweenAliens / 3, 0, 0);
        coolDown = 0.8f + (float)transform.childCount / 50;
        if(changeDirection)
        {
            direction = -direction;
            deltaPos.x = 0;
            deltaPos.z = spaceBetweenLines/3;
            changeDirection = false;
        }

        foreach(Transform t in transform)
        {
            t.position = t.position - deltaPos;
        }
    }

    /// <summary>Pick random alien to shoot.</summary>
    /// <returns>void</returns>
    private void alienFire()
    {
		if (canFire()) 
            if(transform.childCount > 0)
            {
                transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Invader>().fire();
                lastFireTime = Time.time;
            }
    }

    /// <summary>Check for breakout, fire, move, etc and start the necessary processes.</summary>
    /// <returns>void</returns>
    void Update()
	{
        alienFire();
        if (transform.childCount == 0)
        {
            spawnAliens();
            victoryCount++;
        }
		if (!breakOutMode && Time.time - lastMoveTime > coolDown) {
				moveAliens ();
				lastMoveTime = Time.time;
		}

        if (breakOutMode && player.transform.position.z > ball.transform.position.z)
            switchBackToNormal();

        if (isBreakOutTime())
            switchToBreakout();
	}

    /// <summary>Switch to breakout mode : stop the aliens and activate the ball.</summary>
    /// <returns>void</returns>
	void switchToBreakout()
    {
        breakOutMode = true;
        ball.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 2);
        ball.gameObject.SetActive(true);
        ball.GetComponent<Ball>().switchToBreakOut();
    }

    /// <summary>Switch to normal mode : The aliens start moving again and the ball is disactivated.</summary>
    /// <returns>void</returns>
    void switchBackToNormal()
    {
        breakOutStartTime = Time.time;
        breakOutMode = false;
        ball.gameObject.SetActive(false);
        ball.GetComponent<Ball>().switchToNormal();
    }
}
