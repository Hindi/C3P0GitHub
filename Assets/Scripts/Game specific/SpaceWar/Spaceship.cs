using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour
{
    /// <summary>
    /// The object marking the center of the screen
    /// </summary>
    [SerializeField]
    protected GameObject spiral;

    /// <summary>
    /// This ship's projectile
    /// </summary>
    [SerializeField]
    protected GameObject projectile;

    /// <summary>
    /// The factor by which this ship can alter its rotating speed
    /// </summary>
    [SerializeField]
    protected float rotationSpeed;

    /// <summary>
    /// The factor by which this ship can alter its moving speed
    /// </summary>
    [SerializeField]
    protected float linearSpeed;

    /// <summary>
    /// GameObject marking the position at which this ship's projectile starts when fired
    /// </summary>
    [SerializeField]
    protected Transform projectileStartPosition;

    /// <summary>
    /// This ship's maximum speed
    /// </summary>
    [SerializeField]
    protected float maxSpeed;

    /// <summary>
    /// A prefab used to create an explosion when killed
    /// </summary>
    [SerializeField]
    protected GameObject explosion;

    /// <summary>
    /// Timer used to count to game restart once killed
    /// </summary>
    protected float initEndTimer;
    /// <summary>
    /// Bool used to prevent actions once killed
    /// </summary>
    protected bool isEnd;
    /// <summary>
    /// Value to multiply the score delta when destroyed
    /// </summary>
    protected int scoreMult = 1;

    /// <summary>
    /// Rotates the ship according to rotationSpeed
    /// </summary>
    /// <param name="speedFactor">1 or -1 to turn left or right</param>
    public void rotate(float speedFactor)
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * speedFactor) * Time.deltaTime);
    }

    /// <summary>
    /// Adds forward momentum according to linearSpeed
    /// </summary>
    public void goForward()
    {
        rigidbody2D.AddRelativeForce(new Vector3(0, linearSpeed * Time.deltaTime, 0));
        if (rigidbody2D.velocity.magnitude > maxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;
        }
    }

    /// <summary>
    /// Called when exiting the screen
    /// </summary>
    public void exitZone()
    {
        Vector2 delta = spiral.transform.position - transform.position;
        transform.position = (Vector2)spiral.transform.position + 0.99f * delta;
    }

    /// <summary>
    /// Checks if firing a new projectile is possible
    /// </summary>
    /// <returns>True if firing is possible</returns>
    private bool canFire()
    {
        return (!projectile.activeSelf);
    }

    /// <summary>
    /// Tries to fire a new projectile
    /// </summary>
    public void fire()
    {
        if (canFire() && !isEnd)
        {
            projectile.SetActive(true);
            projectile.transform.position = projectileStartPosition.position;
            projectile.rigidbody2D.AddRelativeForce((Vector2)((transform.up) * 100));
            projectile.GetComponent<ProjectileSpaceWar>().activate();
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
        if (isEnd)
        {
            return;
        }
        if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) <= 0.4)
        {
            scoreMult = 10;
            onHit();
        }
        if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) >= 4.5)
        {
            transform.position /= 2;
        }
	}

    /// <summary>
    /// Called when the ship is destroyed
    /// </summary>
    public virtual void onHit()
    {
        if (!isEnd)
        {
            if(scoreMult == 1) scoreMult = 100;
            GameObject explo = (GameObject)GameObject.Instantiate(explosion);
            explo.transform.position = transform.position;
            renderer.enabled = false;
            isEnd = true;
            initEndTimer = Time.time;
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    /// <summary>
    /// Called when the round restarts to reset attributes
    /// </summary>
    public virtual void onRestart(){
        isEnd = false;
        renderer.enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;
        scoreMult = 1;
    }
}
