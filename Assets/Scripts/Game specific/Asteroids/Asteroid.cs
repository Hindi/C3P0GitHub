using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    private AsteroidShip shipScript;
    private Vector3 target;

    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start () {
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();
        target = shipScript.getTarget();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
	}

}
