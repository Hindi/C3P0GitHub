using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject projectile_;

    [SerializeField]
    int speed;

    [SerializeField]
    int projectileMaxDistance_;

    [SerializeField]
    float changeSizeCooldown;
    float lastTimeChangeSize;
    float goalScale;

    private int currentSpeed;

	[SerializeField]
	float fireCooldown;
	float lastShotTime;

    // Use this for initialization
    void Start()
	{
        lastTimeChangeSize = Time.time;
        goalScale = transform.localScale.x;
        projectile_.SetActive(false);
        lastShotTime = Time.time;
        EventManager.AddListener(EnumEvent.RESTARTGAME, onGameRestart);
	}

    public void onGameRestart()
    {
        recallProjectile();
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

	private bool canFire()
	{
		return (Time.time - lastShotTime > fireCooldown && !projectile_.activeSelf);
	}

    public void fire()
    {
		if (canFire ())
        {
			lastShotTime = Time.time;
            projectile_.SetActive(true);
            projectile_.transform.position = transform.position;
        }
    }

	public void hit()
	{
		EventManager<bool>.Raise (EnumEvent.GAMEOVER, false);
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
    void Update()
    {
		move();
		if (projectile_.activeSelf)
		{
			if (Vector3.Distance(transform.position, projectile_.transform.position) > projectileMaxDistance_)
				recallProjectile();
		}
		if (Time.time - lastTimeChangeSize > changeSizeCooldown)
		{
			goalScale = Random.Range(4, 14);
			lastTimeChangeSize = Time.time;
		}
		transform.localScale -= new Vector3((transform.localScale.x - goalScale) / (transform.localScale.x + goalScale), 0, 0);
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