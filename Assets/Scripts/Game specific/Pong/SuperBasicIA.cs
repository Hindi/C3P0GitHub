using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// The script moving the opponent paddle
/// </summary>
public class SuperBasicIA : MonoBehaviour {

    /// <summary>
    /// Reference to the ball
    /// </summary>
    [SerializeField]
    private GameObject ball;
    /// <summary>
    /// Fixed value setting the maximum speed
    /// </summary>
    [SerializeField]
    private float speed;
    /// <summary>
    /// The position after a restart
    /// </summary>
    private Vector3 defaultPos;
    /// <summary>
    /// The original size. Deprecated, should be 1
    /// </summary>
    private Vector3 originalScale;

    private bool coupSpecial = false;
    private float timer;
    private float initTimer;
    private PongManagerScript pms;

    public void onRestart(float resizeWidth, float resizeHeight)
    {
        transform.position = new Vector3(defaultPos.x * resizeWidth, defaultPos.y * resizeHeight, 0);
        transform.localScale = new Vector3(originalScale.x * resizeWidth, originalScale.y * resizeHeight, originalScale.z);
        coupSpecial = false;
    }

    // Use this for initialization
    void Start()
    {
        pms = GameObject.FindGameObjectWithTag("PongManagerScript").GetComponent<PongManagerScript>();
        defaultPos = transform.position;
        originalScale = transform.localScale;
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
                transform.Translate(new Vector3(0, -speed * Time.deltaTime * pms.resizeHeight, 0));
            }
            else
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime * pms.resizeHeight, 0));
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
