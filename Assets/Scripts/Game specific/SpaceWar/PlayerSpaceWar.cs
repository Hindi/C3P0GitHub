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
        base.Update();
	}

    public override void onHit()
    {
        base.onHit();
        EventManager<bool>.Raise(EnumEvent.SPACESHIPDESTROYED, true);
    }
}
