using UnityEngine;
using System.Collections;

public class PlayerSpaceWar : Spaceship {

	// Use this for initialization
    void Start()
    {
	}

	
	// Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) <= 0.4)
        {
            onHit();
        }
	}

    public override void onHit()
    {
        base.onHit();
        EventManager<bool>.Raise(EnumEvent.SPACESHIPDESTROYED, true);
    }
}
