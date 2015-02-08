using UnityEngine;
using System.Collections;

public class GoombaKillScript : MonoBehaviour {

    [SerializeField]
    private GoombaBehaviour goomba;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player" && !goomba.Dieing && !collider.GetComponent<FirstPersonController>().Dieing)
        {
            goomba.die();
            collider.GetComponent<FirstPersonController>().bounce();
        }
    }
}
