using UnityEngine;
using System.Collections;

public class PlayerSpaceWar : Spaceship {

    private Vector3 basePos;

	// Use this for initialization
    void Start()
    {
        basePos = transform.position;
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

    public override void onRestart()
    {
        base.onRestart();
        transform.position = basePos;
    }
}
