using UnityEngine;
using System.Collections;

public class PlayerSpaceWar : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float linearSpeed;

    public void rotate(float speedFactor)
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * speedFactor) * Time.deltaTime);
    }

	// Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow))
            rotate(1);
        if (Input.GetKey(KeyCode.RightArrow))
            rotate(-1);
        if (Input.GetKey(KeyCode.UpArrow))
            rigidbody2D.AddRelativeForce(new Vector3(0, linearSpeed * Time.deltaTime, 0));
	}
}
