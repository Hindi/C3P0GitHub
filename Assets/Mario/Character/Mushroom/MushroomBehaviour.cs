using UnityEngine;
using System.Collections;

public class MushroomBehaviour : MonoBehaviour {

    public double distance;

    private bool bounce = false;
    private bool goingOut = true;
    private Vector3 startPosition;
    private Vector3 direction = Vector3.right;

    double triggerTime = 0;


	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        rigidbody.useGravity = false;
        collider.enabled = false;
        EventManager.AddListener(EnumEvent.RESTARTGAME, onGameRestart);
    }

    public void onGameRestart()
    {
        destroy();
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(EnumEvent.RESTARTGAME, onGameRestart);
    }

    void destroy()
    {
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (goingOut)
        {
            //transform.Translate(Vector3.up * Time.deltaTime);
            //if (Vector3.Distance(transform.position, startPosition) > distance)
            {
                goingOut = false;
                rigidbody.useGravity = true;
                collider.enabled = true;
            }
        }
        else
        {
            transform.Translate(direction * Time.deltaTime * 2);
        }

        if(bounce && Time.time - triggerTime > 1)
        {
            bounce = false;
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!bounce && collision.collider.gameObject.layer == LayerMask.NameToLayer("terrain"))
        {
            triggerTime = Time.time;
            bounce = true;
            if (direction == Vector3.right)
                direction = Vector3.left;
            else
                direction = Vector3.right;
        }
    }

    public void changeDir(Vector3 dir)
    {
        direction = dir;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<MarioScoreManager>().addScore(11);
            collider.GetComponent<FirstPersonController>().eatChamp();
            Destroy(gameObject);
        }

    }
}
