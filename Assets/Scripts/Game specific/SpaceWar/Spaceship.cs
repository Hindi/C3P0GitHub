using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour
{

    [SerializeField]
    protected GameObject spiral;

    [SerializeField]
    protected GameObject projectile;

    [SerializeField]
    protected float rotationSpeed;

    [SerializeField]
    protected float linearSpeed;

    [SerializeField]
    protected Transform projectileStartPosition;

    [SerializeField]
    protected float maxSpeed;

    [SerializeField]
    protected GameObject explosion;

    protected float initEndTimer;
    protected bool isEnd;

    public void rotate(float speedFactor)
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * speedFactor) * Time.deltaTime);
    }

    public void goForward()
    {
        rigidbody2D.AddRelativeForce(new Vector3(0, linearSpeed * Time.deltaTime, 0));
        if (rigidbody2D.velocity.magnitude > maxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;
        }
    }

    public void exitZone()
    {
        Vector2 delta = spiral.transform.position - transform.position;
        transform.position = (Vector2)spiral.transform.position + 0.99f * delta;
    }

    private bool canFire()
    {
        return (!projectile.activeSelf);
    }

    public void fire()
    {
        if (canFire())
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
    protected void Update()
    {
        if (isEnd)
        {
            return;
        }
        if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) <= 0.4)
        {
            onHit();
        }
        if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) >= 4.5)
        {
            transform.position /= 2;
        }
	}

    public virtual void onHit()
    {
        if (!isEnd)
        {
            GameObject explo = (GameObject)GameObject.Instantiate(explosion);
            explo.transform.position = transform.position;
            renderer.enabled = false;
            isEnd = true;
            initEndTimer = Time.time;
        }
    }

    public virtual void onRestart(){
        isEnd = false;
        renderer.enabled = true;
    }
}
