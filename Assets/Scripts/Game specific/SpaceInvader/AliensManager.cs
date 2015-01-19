using UnityEngine;
using System.Collections;

public class AliensManager : MonoBehaviour {


    [SerializeField]
    Player player;

    [SerializeField]
    GameObject ball;

    [SerializeField]
    private GameObject alien1;

    [SerializeField]
    private GameObject alien2;
    
    [SerializeField]
    private GameObject alien3;

    [SerializeField]
    private float coolDown;
    [SerializeField]
    private int startX;
    [SerializeField]
    private int startZ;
    [SerializeField]
    private int spaceBetweenAliens;
    [SerializeField]
    private int spaceBetweenLines;
    [SerializeField]
	private int aliensPerLine;
	[SerializeField]
	private float fireCooldown;

    [SerializeField]
    private float breakOutStartTime;
    private float gameStartTime;

	
    private int direction;
    private bool changeDirection;

    private bool breakOutMode;

    private float lastMoveTime;

	private int deathCount;
	private int alienCount;
	private int victoryCount;
	private float lastFireTime;
	
	// Use this for initialization
	void Start () {
        start();

        gameStartTime = Time.time;

        EventManager.AddListener(EnumEvent.ENEMYDEATH, onEnemyDeath);
        EventManager.AddListener(EnumEvent.RESTARTGAME, onRestartGame);
		EventManager<bool>.AddListener (EnumEvent.GAMEOVER, onGameOver);
	}

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

        lastMoveTime = Time.time;
    }

    void end()
    {
        switchBackToNormal();
        foreach (Transform o in transform)
        {
            o.GetComponent<Invader>().destroy();
            Destroy(o.gameObject);
        }
    }
	
	bool canFire()
	{
		return (Time.time - lastFireTime > fireCooldown / victoryCount && !breakOutMode);
	}

    bool isBreakOutTime()
    {
        return (Time.time - gameStartTime > breakOutStartTime && !breakOutMode);
    }
	
	private void spawnAliens()
	{
		summonLine(0, alien1);
		summonLine(spaceBetweenLines, alien1);
		summonLine(spaceBetweenLines * 2, alien2);
		summonLine(spaceBetweenLines * 3, alien2);
		summonLine(spaceBetweenLines * 4, alien3);
	}
	
	public void onEnemyDeath()
	{
		deathCount++;
		if (deathCount == alienCount)
			EventManager<bool>.Raise (EnumEvent.GAMEOVER, true);
	}

    public void onRestartGame()
    {
        end();
        start();
    }
	
	public void onGameOver(bool b)
	{
		if (b == true)
		    victoryCount++;
	}

    private void summonLine(int z, GameObject prefab)
    {
        GameObject temp;
        for (int i = 0; i <= aliensPerLine; ++i)
        {
            temp = (GameObject)Instantiate(prefab, new Vector3(startX + i * spaceBetweenAliens, 1, startZ + z), Quaternion.identity);
            temp.transform.parent = transform;
        }
    }

    public void reverseDirection()
    {
        changeDirection = true;
    }

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

    private void fire()
    {
		if (canFire()) 
            if(transform.childCount > 0)
            {
                transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Invader>().fire();
                lastFireTime = Time.time;
            }
    }
	
	// Update is called once per frame
    void Update()
	{
		fire ();
        if (transform.childCount == 0)
        {
            spawnAliens();
            victoryCount++;
        }
		if (!breakOutMode && Time.time - lastMoveTime > coolDown) {
				moveAliens ();
				lastMoveTime = Time.time;
		}

        if (isBreakOutTime())
            switchToBreakout();
	}
	
	void switchToBreakout()
    {
        breakOutMode = true;
        ball.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 0.5f);
        ball.gameObject.SetActive(true);
        ball.GetComponent<Ball>().switchToBreakOut();
    }

    void switchBackToNormal()
    {
        breakOutMode = false;
        ball.gameObject.SetActive(false);
        ball.GetComponent<Ball>().switchToNormal();
    }
}
