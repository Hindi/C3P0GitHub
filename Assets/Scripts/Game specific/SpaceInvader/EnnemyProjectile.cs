using UnityEngine;
using System.Collections;

/// <summary>Ennemy projectile.</summary>
public class EnnemyProjectile : MonoBehaviour
{
    /// <summary>the speed of the projectile.</summary>
    [SerializeField]
    int speed;

    /// <summary>cript of the attached invader.</summary>
    [SerializeField]
    private Invader InvaderScript_;

    /// <summary>Update the position.</summary>
    /// <returns>void</returns>
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
    }

    /// <summary>When collide with the player, destroys it.</summary>
    /// <returns>void</returns>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Player>().hit();
            recall();
		}
		else if (collider.gameObject.tag == "Wall")
		{
            recall();
		}
    }

    void recall()
    {
        if (InvaderScript_)
            InvaderScript_.recallProjectile();
        else
            Destroy(gameObject);
    }
}
