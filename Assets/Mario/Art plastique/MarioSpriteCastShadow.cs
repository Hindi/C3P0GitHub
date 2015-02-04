using UnityEngine;
using System.Collections;

public class MarioSpriteCastShadow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        renderer.receiveShadows = true;
        renderer.castShadows = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
