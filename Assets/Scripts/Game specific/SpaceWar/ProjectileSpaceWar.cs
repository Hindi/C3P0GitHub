using UnityEngine;
using System.Collections;

/// <summary>
/// Script used on projectiles in SpaceWar
/// </summary>
public class ProjectileSpaceWar : MonoBehaviour {

    /// <summary>
    /// Fixed value for duration in which the projectile stays alive once fired
    /// </summary>
    [SerializeField]
    private float lifeTime;
    /// <summary>
    /// Timer to count to lifeTime
    /// </summary>
    private float timer;

    /// <summary>
    /// Called by the spiral when going too far
    /// </summary>
    /// <param name="spiral">The object at the center of the screen</param>
    public void exitZone(GameObject spiral)
    {
        Vector2 delta = spiral.transform.position - transform.position;
        transform.position = (Vector2)spiral.transform.position + delta * 0.99f;
    }

    /// <summary>
    /// Called when the projectile is fired
    /// </summary>
    public void activate()
    {
        timer = Time.time;
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && Time.time - timer >= lifeTime)
        {
            rigidbody2D.isKinematic = false;
            gameObject.SetActive(false);
        }
        if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) <= 0.4)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Delegate used when colliding, hopefully with a ship
    /// </summary>
    /// <param name="collider">The object collided</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        try
        {
            collider.gameObject.GetComponent<Spaceship>().onHit(); 
            rigidbody2D.isKinematic = false;
            gameObject.SetActive(false);
        }
        catch(System.NullReferenceException e)
        {
            Debug.Log(e);
        }
    }
}
