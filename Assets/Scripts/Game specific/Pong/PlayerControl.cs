﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -1 * speed * Time.deltaTime, 0));
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
	}
}