using UnityEngine;
using System.Collections;

public class Invader : MonoBehaviour {

    bool destroying;

    private float startTime;

    [SerializeField]
    private GameObject projectile_;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private GameObject animated;

    [SerializeField]
    private GameObject[] eyes;

    [SerializeField]
    private float fireCooldown;
    private float lastFireTime;

    bool stopFire;

	// Use this for initialization
	void Start () {
        destroying = false;
        stopFire = true;
        projectile_.SetActive(false);
        projectile_.transform.Rotate(new Vector3(1, 0, 0), 90);
	}

    public void recallProjectile()
    {
        projectile_.transform.position = transform.position;
        projectile_.SetActive(false);
    }

    bool canFire()
    {
        return (Time.time - lastFireTime > fireCooldown && !stopFire);
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(projectile_.transform.position, transform.position) > 50)
        {
            projectile_.transform.position = transform.position;
            projectile_.SetActive(false);
        }
        if (canFire())
        {
            fire();
            lastFireTime = Time.time;
        }
        if (destroying && Time.time - startTime > cooldown)
        {
            Destroy(animated);
            Destroy(gameObject);
        }
	}

    public void startDestruction()
    {
        destroying = true;
        animated.animation.Play();
        startTime = Time.time;
        collider.enabled = false;
        foreach (GameObject e in eyes)
            Destroy(e);
    }

    private void fire()
    {
        if (!projectile_.activeSelf)
        {
            projectile_.SetActive(true);
            projectile_.transform.position = new Vector3(transform.position.x, transform.position.y+2.8f, transform.position.z);
        }
    }
}
