using UnityEngine;
using System.Collections;

public class BrickBlockBehaviour : MonoBehaviour {

    public AudioClip breakSound;
    public AudioClip bumpSound;
    public Object brokenPart;
    public double distanceWhenTriggered = 0.2;
    public float speed = 2;

    private Vector3 startPosition = new Vector3();
    private bool moving = false;
    private double lastMoveTime = 0.0f;
    private string direction = "up";
    private double deltaDistance = 0.1;

	// Use this for initialization
	void Start ()
    {
        startPosition = transform.position;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (moving)
        {
            if (direction == "up")
            {
                if ((Vector3.Distance(transform.position, startPosition) < distanceWhenTriggered))
                {
                    transform.Translate(Vector3.up * Time.deltaTime * speed);
                }
                else
                {
                    direction = "down";
                }
            }
            if (direction == "down")
            {
                transform.Translate(Vector3.down * Time.deltaTime * speed);
                if ((Vector3.Distance(transform.position, startPosition) < deltaDistance))
                {
                    transform.position = startPosition;
                    moving = false;
                    direction = "up";
                    lastMoveTime = 0.0f;
                }
            }
        }
        lastMoveTime += Time.deltaTime;
	}

    void OnTriggerEnter(Collider collider)
    {
        
    }

    public void trigger(Collider collider)
    {
        if (collider.transform.tag == "TallPlayer")
        {
            //Création et déplacement des morceaux de brique quand le bloc se casse
            GameObject tempPart = (GameObject)Instantiate(brokenPart, transform.position, transform.rotation);
            tempPart.GetComponent<BrokenPartBehaviour>().jump(new Vector3(0.25f, 1, 0), Quaternion.identity);
            tempPart = (GameObject)Instantiate(brokenPart, transform.position, transform.rotation);
            tempPart.GetComponent<BrokenPartBehaviour>().jump(new Vector3(-0.25f, 1, 0), Quaternion.identity);
            tempPart = (GameObject)Instantiate(brokenPart, transform.position, transform.rotation);
            tempPart.GetComponent<BrokenPartBehaviour>().jump(new Vector3(-0.25f, 0.6f, 0), Quaternion.identity);
            tempPart = (GameObject)Instantiate(brokenPart, transform.position, transform.rotation);
            tempPart.GetComponent<BrokenPartBehaviour>().jump(new Vector3(0.25f, 0.6f, 0), Quaternion.identity);
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

            collider.GetComponent<FirstPersonController>().resetVerticalVelocity();

            Destroy(gameObject);
        }
        if (!moving && lastMoveTime > 0.3)
        {
            if (collider.tag == "Player")
            {
                moving = true;
                collider.GetComponent<FirstPersonController>().resetVerticalVelocity();
                AudioSource.PlayClipAtPoint(bumpSound, transform.position);
            }
        }
    }
}
