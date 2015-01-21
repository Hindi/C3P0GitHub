using UnityEngine;
using System.Collections;

public class TetrisCamera : MonoBehaviour {

    [SerializeField]
    private NoiseEffect noiseEffect;

    [SerializeField]
    private BlurEffect blurEffect;

	// Use this for initialization
	void Start () {
        var blur = GetComponent("Blur");
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
