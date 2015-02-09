using UnityEngine;
using System.Collections;

/// <summary>Player's projectile.</summary>
public class Projectile : MonoBehaviour {

    [SerializeField]
    int speed;

    [SerializeField]
    private Player playerScript_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
	}

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
