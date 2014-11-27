using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

    [SerializeField]
    private AliensManager alienManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        alienManager.reverseDirection();
    }
}
