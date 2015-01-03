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
        initPos = transform.position;
	}

    public void switchToBreakOut()
    {
        transform.parent = null;
        rigidbody.AddForce(new Vector3(0, 0, 1.5f));
    }

    public void switchToNormal()
    {
        transform.position = initPos;
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
