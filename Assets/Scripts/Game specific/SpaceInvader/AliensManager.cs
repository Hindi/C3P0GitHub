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
    private GameObject dummyMax;
    [SerializeField]
    private GameObject dummyMin;

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

    private int direction;
    private bool changeDirection;

    private bool breakOutMode;

    private float lastMoveTime;

	// Use this for initialization
	void Start () {
        summonLine(0, alien1);
        summonLine(spaceBetweenLines, alien1);
        summonLine(spaceBetweenLines * 2, alien2);
        summonLine(spaceBetweenLines * 3, alien2);
        summonLine(spaceBetweenLines * 4, alien3);

        direction = 1;
        changeDirection = false;
        breakOutMode = false;

        dummyMin.transform.position = new Vector3(startX - spaceBetweenAliens, 0, startZ);
        dummyMax.transform.position = new Vector3(startX + spaceBetweenAliens * (aliensPerLine + 1), 0, startZ);

        lastMoveTime = Time.time;
	}

    private void summonLine(int z, GameObject prefab)
    {
        GameObject temp;
        for (int i = 0; i <= aliensPerLine; ++i)
        {
            temp = (GameObject)Instantiate(prefab, new Vector3(startX + i * spaceBetweenAliens, 0, startZ + z), Quaternion.identity);
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

    }
	
	// Update is called once per frame
	void Update () {
        if (!breakOutMode && Time.time - lastMoveTime > coolDown)
        {
            //moveAliens();
            fire();
            lastMoveTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Return))
            switchToBreakout();
	}

    void switchToBreakout()
    {
        breakOutMode = true;
        ball.gameObject.SetActive(true);
        ball.GetComponent<Ball>().switchToBreakOut();
    }
}
