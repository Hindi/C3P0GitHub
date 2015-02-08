using UnityEngine;
using System.Collections;

public class BrokenPartBehaviour : MonoBehaviour {

    public float force = 200;

    private Vector3 startPosition;

	// Use this for initialization
	void Start () 
    {
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        rigidbody.AddRelativeForce(Vector3.down * 20f, ForceMode.Acceleration);
        if (Vector3.Distance(startPosition, transform.position) > 50)
        {
            Destroy(gameObject);
        }
	}

    public void jump(Vector3 translation, Quaternion rotation)
    {
        rigidbody.AddRelativeForce(translation * force, ForceMode.Acceleration);
    }
}
