using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

	public float speed = -5;
	public double range = 30;

	private Vector3 startPosition = new Vector3();

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		if (Vector3.Distance (transform.position, startPosition) > range) {
			Destroy(gameObject);
				}
	}
}
