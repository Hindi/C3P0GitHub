using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Libellule : MonoBehaviour {

    [SerializeField]
    private GameObject point;
    [SerializeField]
    private float timer = 10, speed;
    private float initTime;
    private Vector3 initPos;
    private int nbPoints = 0;
    private List<GameObject> points;


	// Use this for initialization
	void Awake () {
        initTime = Time.time;
        initPos = transform.position;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Time.time - initTime >= timer)
        {
            restart();
            return;
        }
        if (Time.time - initTime >= nbPoints * timer / 20)
        {
            nbPoints++;
            points.Add((GameObject) Instantiate(point, transform.position, transform.rotation));
        }
	}

    public void restart()
    {
        EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
        transform.position = initPos;
        gameObject.SetActive(false);
    }

    public void activate()
    {
        points = new List<GameObject>();
        initTime = Time.time;
        nbPoints = 0;
    }

    public void destroyPoints()
    {
        if (points == null)
        {
            return;
        }
        foreach (GameObject p in points)
        {
            Destroy(p, 0);
        }
    }
}
