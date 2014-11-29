using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]
    private GameObject projectile_;

    [SerializeField]
    int speed;

    [SerializeField]
    int projectileMaxDistance_;

    private int currentSpeed;

	// Use this for initialization
    void Start()
    {
        projectile_.SetActive(false);
	}

    public void setDirection(bool right)
    {
        if (right)
            currentSpeed = speed;
        else
            currentSpeed = -speed;
    }

    public void stop()
    {
        currentSpeed = 0;
    }

    private void fire()
    {
        if (!projectile_.activeSelf)
        {
            projectile_.SetActive(true);
            projectile_.transform.position = transform.position;
        }
    }

    void move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            setDirection(false);
        else if (Input.GetKey(KeyCode.RightArrow))
            setDirection(true);
        else
            stop();
        transform.position = new Vector3(transform.position.x + currentSpeed * Time.deltaTime, transform.position.y, transform.position.z);
    }

    public void recallProjectile()
    {
        projectile_.transform.position = transform.position;
        projectile_.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        move();
        if (Input.GetKey(KeyCode.Space))
            fire();
        if (projectile_.activeSelf)
        {
            if (Vector3.Distance(transform.position, projectile_.transform.position) > projectileMaxDistance_)
                recallProjectile();

        }
	}

    float calcBouncingForce(float delta)
    {
        return - Mathf.Sign(delta) * Mathf.Pow(delta / 3, 2);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            float delta = transform.position.x - collision.transform.position.x;
            collision.rigidbody.AddForce(new Vector3(calcBouncingForce(delta), 0, 0));
        }
    }

}
