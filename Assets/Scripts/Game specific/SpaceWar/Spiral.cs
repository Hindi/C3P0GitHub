using UnityEngine;
using System.Collections;

public class Spiral : MonoBehaviour {

    private Vector3 rotSpeed;

    [SerializeField]
    private float gravityAcceleration;

    [SerializeField]
    private Transform player;

	// Use this for initialization
	void Start () {
        rotSpeed = new Vector3(0,0,1);
	}

    void applyGravity()
    {

    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotSpeed);
	}

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player" || collider.tag == "Enemy")
            collider.rigidbody2D.velocity += (Vector2)((transform.position - collider.transform.position) * gravityAcceleration);
        else if (collider.tag == "Projectile")
            collider.rigidbody2D.velocity += (Vector2)((transform.position - collider.transform.position) * gravityAcceleration * 20);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player" || collider.tag == "Enemy")
            collider.GetComponent<Spaceship>().exitZone();
        else if (collider.tag == "Projectile")
            collider.GetComponent<ProjectileSpaceWar>().exitZone(gameObject);
    }
}
