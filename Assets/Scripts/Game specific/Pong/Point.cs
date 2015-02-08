using UnityEngine;
using System.Collections;

/// <summary>
/// Script making dots self destruct after some time
/// </summary>
public class Point : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
