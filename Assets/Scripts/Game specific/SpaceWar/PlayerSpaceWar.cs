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
        if (isEnd)
        {
            if (Time.time - initEndTimer >= 2)
            {
                isEnd = false;
                EventManager<bool>.Raise(EnumEvent.SPACESHIPDESTROYED, true);
            }
        }
	}

    public override void onHit()
    {
        base.onHit();
    }

    public override void onRestart()
    {
        base.onRestart();
    }
}
