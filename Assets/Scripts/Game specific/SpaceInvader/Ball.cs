using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void switchToBreakOut()
    {
        transform.parent = null;
        rigidbody.AddForce(new Vector3(0, 0, 1.5f));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Invader>().startDestruction();
        }
    }
}
