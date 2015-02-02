using UnityEngine;
using System;
using System.Collections;

public class SuperBasicIA : MonoBehaviour {

    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private float speed;
    private Vector3 defaultPos;

    private bool coupSpecial = false;
    private float timer;
    private float initTimer;
    private PongManagerScript pms;

    public void onRestart(float resizeWidth, float resizeHeight)
    {
        transform.position = new Vector3(defaultPos.x * resizeWidth, defaultPos.y * resizeHeight, 0);
        coupSpecial = false;
    }

    // Use this for initialization
    void Start()
    {
        pms = GameObject.FindGameObjectWithTag("PongManagerScript").GetComponent<PongManagerScript>();
        defaultPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Math.Abs(transform.position.y - ball.transform.position.y) <= 0.1)
        {

        }
        else
        {
            if (transform.position.y - ball.transform.position.y > 0)
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            }
            else
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }
        }
        if (Time.time - initTimer > timer && coupSpecial)
        {
            pms.launchCoupSpecial(1);
            coupSpecial = false;
        }
	}

    public void getCoupSpecial(PongManagerScript s)
    {
        pms = s;
        coupSpecial = true;
        initTimer = Time.time;
        timer = Math.Abs((float) Laws.gauss() + 10) % 3;
    }
}
