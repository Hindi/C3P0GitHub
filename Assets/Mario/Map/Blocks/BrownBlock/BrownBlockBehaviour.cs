using UnityEngine;
using System.Collections;

public class BrownBlockBehaviour : MonoBehaviour {

    public AudioClip bumpSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void trigger(Collider collider)
    {
        if (collider.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(bumpSound, transform.position);

            collider.GetComponent<FirstPersonController>().resetVerticalVelocity();
        }
    }
}
