﻿using UnityEngine;
using System;
using System.Collections;

public class SuperBasicIA : MonoBehaviour {

    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private float speed;
    private Vector3 defaultPos;

    public void onRestart()
    {
        transform.position = defaultPos;
    }

    // Use this for initialization
    void Start()
    {
        defaultPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y - ball.transform.position.y > 0)
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        else
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
	}
}
