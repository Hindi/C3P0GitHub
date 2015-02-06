using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    private Vector3 initPos;

    [SerializeField]
    private Player playerScript_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void switchToBreakOut()
    {
        rigidbody.AddForce(new Vector3(0, 0, 1.2f));
    }

    private void resetForce()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void switchToNormal()
    {
        resetForce();
        transform.position = new Vector3(playerScript_.transform.position.x, playerScript_.transform.position.y, playerScript_.transform.position.z + 2);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerScript_.enemyDestroyed();
            collision.gameObject.GetComponent<Invader>().startDestruction();
        }
    }
}
