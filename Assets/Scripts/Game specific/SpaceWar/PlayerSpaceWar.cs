using UnityEngine;
using System.Collections;

public class PlayerSpaceWar : Spaceship {

	// Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow))
            rotate(1);
        if (Input.GetKey(KeyCode.RightArrow))
            rotate(-1);
        if (Input.GetKey(KeyCode.UpArrow))
            rigidbody2D.AddRelativeForce(new Vector3(0, linearSpeed * Time.deltaTime, 0));
        if (Input.GetKeyDown(KeyCode.Space))
            fire();
	}

    public override void onHit()
    {
        base.onHit();
        EventManager<bool>.Raise(EnumEvent.SPACESHIPDESTROYED, true);
    }
}
