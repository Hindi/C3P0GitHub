using UnityEngine;
using System.Collections;

/// <summary>Alien scripts.</summary>
public class Invader : MonoBehaviour {

    /// <summary>If true starts the destroyin animation.</summary>
    bool destroying;

    /// <summary>The time when the destroying animation starts.</summary>
    private float startTime;

    /// <summary>The attached projectile.</summary>
    [SerializeField]
    private GameObject projectile_;

    /// <summary>The destroying animation time.</summary>
    [SerializeField]
    private float cooldown;

    /// <summary>The animated object.</summary>
    [SerializeField]
    private GameObject animated;

    /// <summary>The eyes.</summary>
    [SerializeField]
    private GameObject[] eyes;

    /// <summary>Initialise the object.</summary>
    /// <returns>void</returns>
	void Start () {
		projectile_.transform.parent = null;
        destroying = false;
        projectile_.SetActive(false);
        projectile_.transform.Rotate(new Vector3(1, 0, 0), 90);
	}

    /// <summary>Recall the projectile.</summary>
    /// <returns>void</returns>
    public void recallProjectile()
    {
        projectile_.transform.position = transform.position;
        projectile_.SetActive(false);
    }

    /// <summary>Check if the destroying animation ended, then destroy everything.</summary>
    /// <returns>void</returns>
	void Update () {

        if (destroying && Time.time - startTime > cooldown)
        {
            Destroy(animated);
            Destroy(gameObject);
			EventManager.Raise(EnumEvent.ENEMYDEATH);
        }
	}

    /// <summary>Starts the destruction process.</summary>
    /// <returns>void</returns>
    public void startDestruction()
    {
        destroying = true;
        animated.animation.Play();
        startTime = Time.time;
        collider.enabled = false;
        foreach (GameObject e in eyes)
            Destroy(e);
    }

    /// <summary>Shoot a projectile.</summary>
    /// <returns>void</returns>
    public void fire()
    {
		if (!projectile_.activeSelf && !destroying)
		{
            projectile_.SetActive(true);
            projectile_.transform.position = new Vector3(transform.position.x, transform.position.y+2.5f, transform.position.z);
        }
    }

    /// <summary>Destroy the projectile.</summary>
    /// <returns>void</returns>
    public void destroy()
    {
        Destroy(projectile_);
    }
}
