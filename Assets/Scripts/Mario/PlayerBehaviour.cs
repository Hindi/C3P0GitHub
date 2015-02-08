using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    private bool jumping = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown("space"))
        {
            jump();
        }
        rigidbody.AddRelativeForce(Vector3.down * 9.8f, ForceMode.Acceleration);
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Terrain" && jumping)
        {
            jumping = false;
        }
    }

	public void jump()
	{
        if (!jumping)
        {
            rigidbody.AddRelativeForce(Vector3.up * 750, ForceMode.Acceleration);
            jumping = true;
        }
	}

    public void pushDown()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        rigidbody.AddRelativeForce(Vector3.down * 250, ForceMode.Acceleration);
    }
}
