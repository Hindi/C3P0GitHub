using UnityEngine;
using System.Collections;

public class Galton : MonoBehaviour {

    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject objects;


	// Use this for initialization
	void Start () {
        objects.SetActive(false);
	}

    public void start()
    {
        objects.SetActive(true);
        ball.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        ball.rigidbody.velocity = Vector3.zero;
        ball.rigidbody.angularVelocity = Vector3.zero;
    }
	
    public void end()
    {
        objects.SetActive(false);
    }

	// Update is called once per frame
    void Update()
    {
	}
}
