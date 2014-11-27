using UnityEngine;
using System.Collections;

public class Cylinder : MonoBehaviour {

    private Vector3 eulerAngleVelocity;
	// Use this for initialization
    void Start()
    {
        init();
	}

    private void init()
    {
        if (Random.value > 0.5)
            eulerAngleVelocity = new Vector3(0, 5000, 0);
        else
            eulerAngleVelocity = new Vector3(0, -10000, 0);
    }

    void OnEnable()
    {
        init();
    }
	
	// Update is called once per frame
    void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
	}
}
