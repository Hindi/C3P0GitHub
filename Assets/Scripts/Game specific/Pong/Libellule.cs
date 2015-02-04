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


	// Use this for initialization
	void Start () {
        initTime = Time.time;
        initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Time.time - initTime >= timer)
        {
            EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
            transform.position = initPos;
            gameObject.SetActive(false);
            return;
        }
        if (Time.time - initTime >= nbPoints * timer / 10)
        {
            nbPoints++;
            Instantiate(point, transform.position, transform.rotation);
        }
	}

    public void activate()
    {
        initTime = Time.time;
        nbPoints = 0;
    }
}
