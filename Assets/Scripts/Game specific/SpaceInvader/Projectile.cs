using UnityEngine;
using System.Collections;

/// <summary>Player's projectile.</summary>
public class Projectile : MonoBehaviour {

    /// <summary>Projectile's speed.</summary>
    [SerializeField]
    int speed;

    /// <summary>Player script.</summary>
    [SerializeField]
    private Player playerScript_;

    /// <summary>Update the object's position.</summary>
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
	}

    /// <summary>If the projectile collides with an enemy, destroys it.</summary>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            playerScript_.enemyDestroyed();
            playerScript_.recallProjectile();
            collider.gameObject.GetComponent<Invader>().startDestruction();
        }
    }
}
