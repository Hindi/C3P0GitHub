using UnityEngine;
using System.Collections;

/// <summary>
/// The script on the object in the center of the screen
/// </summary>
public class Spiral : MonoBehaviour {
    /// <summary>
    /// The speed at which the image rotates
    /// </summary>
    private Vector3 rotSpeed;

    /// <summary>
    /// The factor by which galactic objects are attracted
    /// </summary>
    [SerializeField]
    private float gravityAcceleration;

	// Use this for initialization
	void Start () {
        rotSpeed = new Vector3(0,0,1);
	}

    /// <summary>
    /// Not actually used, gravity is done using OnTriggerStay2D
    /// </summary>
    void applyGravity()
    {

    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotSpeed);
	}

    /// <summary>
    /// Unity Delegate : Applies gravity to all galactic objects
    /// </summary>
    /// <param name="collider">The current object having gravity applied</param>
    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player" || collider.tag == "Enemy")
            collider.rigidbody2D.velocity += (Vector2)((transform.position - collider.transform.position) * gravityAcceleration);
        else if (collider.tag == "Projectile")
            collider.rigidbody2D.velocity += (Vector2)((transform.position - collider.transform.position) * gravityAcceleration * 20);
    }

    /// <summary>
    /// Unity Delegate : If an object is trying to escape, it kindly asks it not to
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player" || collider.tag == "Enemy")
            collider.GetComponent<Spaceship>().exitZone();
        else if (collider.tag == "Projectile")
            collider.GetComponent<ProjectileSpaceWar>().exitZone(gameObject);
    }
}
