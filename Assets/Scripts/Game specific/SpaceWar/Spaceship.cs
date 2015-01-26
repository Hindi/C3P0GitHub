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

    public void rotate(float speedFactor)
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * speedFactor) * Time.deltaTime);
    }

    public void exitZone()
    {
        Vector2 delta = spiral.transform.position - transform.position;
        transform.position = (Vector2)spiral.transform.position + delta;
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
	void Update () {
	}

    public void goForward()
    {
        rigidbody2D.AddRelativeForce(new Vector3(0, linearSpeed * Time.deltaTime, 0));
        if (rigidbody2D.velocity.magnitude > maxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxSpeed;
        }
    }

    public virtual void onHit()
    {
        //Debug.Log("GameOver");
    }
}
