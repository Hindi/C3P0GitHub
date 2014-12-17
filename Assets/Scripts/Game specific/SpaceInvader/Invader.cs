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

	// Use this for initialization
	void Start () {
		projectile_.transform.parent = null;
        destroying = false;
        projectile_.SetActive(false);
        projectile_.transform.Rotate(new Vector3(1, 0, 0), 90);
	}

    public void recallProjectile()
    {
        projectile_.transform.position = transform.position;
        projectile_.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (destroying && Time.time - startTime > cooldown)
        {
            Destroy(animated);
            Destroy(gameObject);
			EventManager.Raise(EnumEvent.ENEMYDEATH);
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

    public void fire()
    {
		if (!projectile_.activeSelf && !destroying)
		{
            projectile_.SetActive(true);
            projectile_.transform.position = new Vector3(transform.position.x, transform.position.y+2.5f, transform.position.z);
        }
    }

    public void destroy()
    {
        Destroy(projectile_);
    }
}
