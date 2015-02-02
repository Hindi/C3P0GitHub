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
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (goingOut)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPosition) > distance)
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
        if (!bounce && collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain obstacles"))
        {
            triggerTime = Time.time;
            bounce = true;
            if (direction == Vector3.right)
                direction = Vector3.left;
            else
                direction = Vector3.right;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            collider.GetComponent<FirstPersonController>().eatChamp();
            Destroy(gameObject);
        }

    }
}
