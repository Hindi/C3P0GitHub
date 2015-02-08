using UnityEngine;
using System.Collections;

public class MushrommBlockBehaviour : MonoBehaviour {

    public Object mushroomPrefab;
    public Material triggeredMaterial;
    public AudioClip bumpSound;

    public double distanceWhenTriggered = 0.2;
    public float speed = 2;

    private Vector3 startPosition = new Vector3();
    private bool moving = false;
    private bool containCoin = true;
    private string direction = "up";
    private double deltaDistance = 0.1;

    private double lastMoveTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
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
        if (!moving && lastMoveTime > 0.3 && containCoin)
        {
            if (collider.tag == "Player")
            {
                renderer.material = triggeredMaterial;
                moving = true;
                AudioSource.PlayClipAtPoint(bumpSound, transform.position);
                containCoin = false;
                Instantiate(mushroomPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            }
        }
        collider.GetComponent<FirstPersonController>().resetVerticalVelocity();
    }
}
