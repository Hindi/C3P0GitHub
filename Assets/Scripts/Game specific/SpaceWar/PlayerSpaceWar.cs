using UnityEngine;
using System.Collections;

public class PlayerSpaceWar : Spaceship {

	// Use this for initialization
    void Start()
    {
	}

    public void goForward()
    {
        rigidbody2D.AddRelativeForce(new Vector3(0, linearSpeed * Time.deltaTime, 0));
    }
	
	// Update is called once per frame
	void Update () {

	}

    public override void onHit()
    {
        base.onHit();
        EventManager<bool>.Raise(EnumEvent.SPACESHIPDESTROYED, true);
    }
}
