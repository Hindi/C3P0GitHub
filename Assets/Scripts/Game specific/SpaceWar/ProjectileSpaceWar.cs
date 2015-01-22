﻿using UnityEngine;
using System.Collections;

public class ProjectileSpaceWar : MonoBehaviour {

    [SerializeField]
    private float lifeTime;
    private float timer;

    public void exitZone(GameObject spiral)
    {
        Vector2 delta = spiral.transform.position - transform.position;
        transform.position = (Vector2)spiral.transform.position + delta;
    }

    public void activate()
    {
        timer = Time.time;
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && Time.time - timer >= lifeTime)
        {
            rigidbody2D.isKinematic = false;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        try
        {
            collider.gameObject.GetComponent<Spaceship>().onHit();
        }
        catch(System.NullReferenceException e)
        {

        }
    }
}
