using UnityEngine;
using System.Collections;

public class PathfindingTest : MonoBehaviour {
	public Transform target = null;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		GetComponent<NavMeshAgent>().destination = target.transform.position;
	}
}
