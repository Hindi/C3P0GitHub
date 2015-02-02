using UnityEngine;
using System.Collections;

public class CannonBehaviour : MonoBehaviour {

	public GameObject muzzlePosition;
	public GameObject missilePrefab;
	public AudioClip sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider collider)
	{
		fire ();
		}

	public void fire()
	{
        if(sound)
		    AudioSource.PlayClipAtPoint (sound, muzzlePosition.transform.position);
		Instantiate (missilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation);
	}

    public void trigger(Collider collider)
    {
        fire();
    }
}
